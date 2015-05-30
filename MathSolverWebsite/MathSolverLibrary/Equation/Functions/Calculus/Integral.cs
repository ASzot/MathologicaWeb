using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus
{
    class Integral : AppliedFunction
    {
        private bool _failure = false;
        protected AlgebraComp _dVar = null;
        private bool _addConstant = true;
        private IntegrationInfo _integralInfo = null;
        protected ExComp _upper = null;
        protected ExComp _lower = null;
        private bool _isInnerIntegral = false;


        public AlgebraTerm UpperLimitTerm
        {
            get { return _upper.ToAlgTerm(); }
        }

        public AlgebraTerm LowerLimitTerm
        {
            get { return _lower.ToAlgTerm(); }
        }

        public ExComp UpperLimit
        {
            get { return _upper; }
            set 
            {
                _upper = value;
            }
        }
        public ExComp LowerLimit
        {
            get { return _lower; }
            set
            {
                _lower = value;
            }
        }

        public bool AddConstant
        {
            set { _addConstant = value; }
        }

        public IntegrationInfo Info
        {
            set { _integralInfo = value; }
        }

        public AlgebraComp DVar
        {
            get { return _dVar; }
            set { _dVar = value; }
        }

        public bool IsDefinite
        {
            get { return LowerLimit != null && UpperLimit != null; }
        }

        public Integral(ExComp innerEx)
            : base(innerEx, FunctionType.AntiDerivative, typeof(Integral))
        {

        }

        public Integral(ExComp innerEx, bool isInnerIntegral)
            : this(innerEx)
        {
            _isInnerIntegral = isInnerIntegral;
        }


        public override ExComp Clone()
        {
            return ConstructIntegral(InnerTerm, _dVar, LowerLimit, UpperLimit, _isInnerIntegral);
        }

        public static Integral ConstructIntegral(ExComp innerEx, AlgebraComp dVar)
        {
            return ConstructIntegral(innerEx, dVar, null, null);
        }

        private static Dictionary<string, Integral> GetIntegralDepths(Integral integral, ref Dictionary<string, Integral> dict, out ExComp baseValue)
        {
            if (integral.DVar == null || dict.ContainsKey(integral.DVar.Var.Var) || !integral.IsDefinite)
            {
                baseValue = integral.InnerTerm;
                return dict;
            }

            dict[integral.DVar.Var.Var] = integral;

            ExComp innerEx = integral.InnerEx;
            if (innerEx is Integral)
            {
                return GetIntegralDepths(innerEx as Integral, ref dict, out baseValue);
            }

            baseValue = innerEx;
            return dict;
        }

        private static ExComp RearrangeIntegral(Integral inputIntegral)
        {
            ExComp baseValue;
            Dictionary<string, Integral> dict = new Dictionary<string, Integral>();

            dict = GetIntegralDepths(inputIntegral, ref dict, out baseValue);

            bool switched = false;

            foreach (KeyValuePair<string, Integral> kvPair in dict)
            {
                Integral integral = kvPair.Value;
                if (integral.LowerLimit.ToAlgTerm().Contains(integral.DVar) || integral.UpperLimit.ToAlgTerm().Contains(integral.DVar))
                {
                    // The variables need to be switched.
                    // Find a suitable place to switch the variable where the dvar is not the integration boundary.
                    foreach (KeyValuePair<string, Integral> compareKvPair in dict)
                    {
                        if (compareKvPair.Key != integral.DVar.Var.Var &&
                            compareKvPair.Value.LowerLimit.ToAlgTerm().Contains(integral.DVar) &&
                            compareKvPair.Value.UpperLimit.ToAlgTerm().Contains(integral.DVar) &&
                            integral.LowerLimit.ToAlgTerm().Contains(compareKvPair.Value.DVar) &&
                            integral.UpperLimit.ToAlgTerm().Contains(compareKvPair.Value.DVar))
                        {
                            AlgebraComp tmp = kvPair.Value.DVar;
                            kvPair.Value.DVar = compareKvPair.Value.DVar;
                            switched = true;
                            break;
                        }
                    }
                }
            }

            if (!switched)
                return inputIntegral;

            // Go back to the regular integral form.
            ExComp overallIntegral = baseValue;
            foreach (Integral value in dict.Values)
            {
                overallIntegral = ConstructIntegral(overallIntegral, value.DVar, value.LowerLimit, value.UpperLimit, false, false);
            }

            return overallIntegral;
        }

        public static Integral ConstructIntegral(ExComp innerEx, AlgebraComp dVar, ExComp lower, ExComp upper, bool isInner = false, bool rearrange = true)
        {
            Integral integral = new Integral(innerEx);
            integral._dVar = dVar;
            integral.LowerLimit = lower;
            integral.UpperLimit = upper;
            integral._isInnerIntegral = isInner;

            // In the case of multidimensional integrals variable boundaries will potentially have to be rearranged.
            if (innerEx is Integral)
            {
                Integral innerInt = innerEx as Integral;
                innerInt._isInnerIntegral = true;
                if (lower != null && upper != null && rearrange)
                    return RearrangeIntegral(integral) as Integral;
            }

            return integral;
        }

        public static ExComp TakeAntiDeriv(ExComp innerEx, AlgebraComp dVar, ref TermType.EvalData pEvalData)
        {
            Integral integral = ConstructIntegral(innerEx, dVar);
            integral._addConstant = false;
            return integral.Evaluate(false, ref pEvalData);
        }

        public override ExComp CancelWith(ExComp innerEx, ref TermType.EvalData evalData)
        {
            if (innerEx is Derivative)
            {
                Derivative innerDeriv = innerEx as Derivative;
                if (innerDeriv.WithRespectTo.IsEqualTo(_dVar) && innerDeriv.DerivOf == null && innerDeriv.OrderInt == 1)
                {
                    evalData.WorkMgr.FromSides(this, null, "The integral and the derivative cancel.");
                    return innerDeriv.InnerTerm;
                }
            }

            return null;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            CallChildren(harshEval, ref pEvalData);

            AlgebraTerm innerTerm = InnerTerm;
            ExComp innerEx = Simplifier.Simplify(innerTerm, ref pEvalData);

            if (Number.IsUndef(innerEx))
                return Number.Undefined;

            if (innerEx is ExVector && !IsDefinite)
            {
                ExVector vec = innerEx as ExVector;

                // Take the anti derivative of each component separately.

                ExVector antiDerivVec = vec.CreateEmptyBody();

                for (int i = 0; i < vec.Length; ++i)
                {
                    ExComp antiDeriv = Indefinite(vec.Get(i), ref pEvalData);
                    antiDerivVec.Set(i, antiDeriv);
                }

                return antiDerivVec;
            }
            else if (innerEx is ExMatrix)
            {
                // Don't know if this works.
                return Number.Undefined;
            }

            string integralStr = FinalToDispStr();

            ExComp useUpper;
            ExComp useLower;

            if (UpperLimit is Number && (UpperLimit as Number).IsInfinity())
                useUpper = new AlgebraComp("$n");
            else
                useUpper = UpperLimit;

            if (LowerLimit is Number && (LowerLimit as Number).IsInfinity())
                useLower = new AlgebraComp("$n");
            else
                useLower = LowerLimit;


            if (useUpper != null && useLower != null && !useUpper.IsEqualTo(UpperLimit) && !useLower.IsEqualTo(LowerLimit))
            {
                // Evaluating from infinity in both directions. 
                // Split the integral up.
                Integral upperInt = Integral.ConstructIntegral(InnerTerm, _dVar, Number.Zero, Number.PosInfinity);
                Integral lowerInt = Integral.ConstructIntegral(InnerTerm, _dVar, Number.NegInfinity, Number.Zero);
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + integralStr + "=" + upperInt.FinalToDispStr() + "+" + 
                    lowerInt.FinalToDispStr() + WorkMgr.EDM,
                    "Split the integral.");

                pEvalData.WorkMgr.FromFormatted("", "Evaluate the upper integral.");
                WorkStep lastStep = pEvalData.WorkMgr.GetLast();

                lastStep.GoDown(ref pEvalData);
                ExComp upperSideEval = upperInt.Evaluate(harshEval, ref pEvalData);
                lastStep.GoUp(ref pEvalData);

                lastStep.WorkHtml = WorkMgr.STM + upperInt.FinalToDispStr() + "=" + WorkMgr.ToDisp(upperSideEval) + WorkMgr.EDM;

                pEvalData.WorkMgr.FromFormatted("", "Evaluate the lower integral.");
                lastStep = pEvalData.WorkMgr.GetLast();

                lastStep.GoDown(ref pEvalData);
                ExComp lowerSideEval = lowerInt.Evaluate(harshEval, ref pEvalData);
                lastStep.GoUp(ref pEvalData);

                lastStep.WorkHtml = WorkMgr.STM + lowerInt.FinalToDispStr() + "=" + WorkMgr.ToDisp(lowerSideEval) + WorkMgr.EDM;

                ExComp added = AddOp.StaticCombine(upperSideEval, lowerSideEval);

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + WorkMgr.ToDisp(upperSideEval) + "+" + WorkMgr.ToDisp(lowerSideEval) +
                    "=" + WorkMgr.ToDisp(added) + WorkMgr.EDM, "Combine the integral back together.");

                return added;
            }

            AlgebraTerm indefinite = Indefinite(innerEx, ref pEvalData);
            if (_failure)
                return indefinite;      // Just 'this'

            ExComp indefiniteEx = indefinite.RemoveRedundancies();

            if (LowerLimit == null || UpperLimit == null)
            {
                if (_addConstant && !_isInnerIntegral && !(indefiniteEx is Integral))
                {
                    // Add the constant.
                    ExComp retEx = AddOp.StaticWeakCombine(indefinite, new CalcConstant());
                    pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + retEx.ToAlgTerm().FinalToDispStr() + WorkMgr.EDM,
                        "Add the constant of integration.");
                    return retEx;
                }
                else
                    return indefinite;
            }

            AlgebraTerm upperEval = indefinite.Clone().ToAlgTerm().Substitute(_dVar, useUpper);
            ExComp upperEx = Simplifier.Simplify(new AlgebraTerm(upperEval), ref pEvalData);

            AlgebraTerm lowerEval = indefinite.Clone().ToAlgTerm().Substitute(_dVar, useLower);
            ExComp lowerEx = Simplifier.Simplify(new AlgebraTerm(lowerEval), ref pEvalData);


            AlgebraComp subVar = null;
            ExComp limVal = null;
            if (!useUpper.IsEqualTo(UpperLimit))
            {
                subVar = useUpper as AlgebraComp;
                limVal = Number.PosInfinity;
            }
            else if (!useLower.IsEqualTo(LowerLimit))
            {
                subVar = useLower as AlgebraComp;
                limVal = Number.NegInfinity;
            }

            integralStr = "\\int_{" + WorkMgr.ToDisp(useLower) + "}^{" + WorkMgr.ToDisp(useUpper) + "}(" + InnerTerm.FinalToDispStr() + ")d" + _dVar.ToDispString(); 

            if (subVar != null)
            {
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + integralStr + "=\\lim_{" + subVar.ToDispString() +
                    " \\to \\infty}" + integralStr + WorkMgr.EDM);
                integralStr = "\\lim_{" + subVar.ToDispString() +
                    " \\to \\infty}" + integralStr;
            }

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + integralStr + "=F(" +
                WorkMgr.ToDisp(useUpper) + ")-F(" + WorkMgr.ToDisp(useLower) + ")" + WorkMgr.EDM,
                "Evaluate the definite integral where F is the antiderivative.");

            string resultStr0 = SubOp.StaticWeakCombine(upperEx, lowerEx).ToAlgTerm().FinalToDispStr();

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + integralStr + "=" +
                resultStr0 + WorkMgr.EDM);

            ExComp result = SubOp.StaticCombine(upperEx, lowerEx);
            if (result is AlgebraTerm)
                result = (result as AlgebraTerm).CompoundFractions();

            result = TermType.SimplifyTermType.BasicSimplify(result, ref pEvalData);

            if (subVar != null)
            {
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\lim_{" + subVar.ToDispString() + " \\to \\infty}" +
                    WorkMgr.ToDisp(result) + WorkMgr.EDM, "Take the limit to infinity.");
                result = Limit.TakeLim(result, subVar, limVal, ref pEvalData);
            }

            pEvalData.AddInputType(TermType.InputAddType.IntDef);

            string resultStr1 = WorkMgr.ToDisp(result);
            integralStr = this.FinalToDispStr();
            if (resultStr0 != resultStr1)
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + integralStr + "=" + resultStr1 + WorkMgr.EDM);

            return result;
        }

        private AlgebraTerm Indefinite(ExComp takeEx, ref TermType.EvalData pEvalData)
        {
            string thisStr = takeEx.ToAlgTerm().FinalToDispStr();

            // Split the integral up by groups.
            List<ExComp[]> gps = takeEx.Clone().ToAlgTerm().GetGroupsNoOps();

            if (gps.Count == 0)
            {
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\int\\d" + _dVar.ToDispString() + "=" + _dVar.ToDispString() + WorkMgr.EDM,
                    "Use the antiderivative power rule.");
                return _dVar.ToAlgTerm();
            }

            string[] intStrs = new string[gps.Count];

            if (gps.Count > 1)
            {
                string overallStr = "";
                for (int i = 0; i < gps.Count; ++i)
                {
                    intStrs[i] = "\\int" + gps[i].ToAlgTerm().FinalToDispStr() + "\\d" + _dVar.ToDispString();
                    overallStr += intStrs[i];
                    if (i != gps.Count - 1)
                        overallStr += "+";
                }

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + overallStr + WorkMgr.EDM, "Split the integral up.");
            }

            // Independently take the derivative of each group.
            ExComp[] adGps = new ExComp[gps.Count];
            for (int i = 0; i < gps.Count; ++i) 
            {
                IntegrationInfo integrationInfo = _integralInfo ?? new IntegrationInfo();
                int prevStepCount = pEvalData.WorkMgr.WorkSteps.Count;

                string lowerStr = LowerLimit == null ? "" : LowerLimit.ToAlgTerm().FinalToDispStr();
                string upperStr = UpperLimit == null ? "" : UpperLimit.ToAlgTerm().FinalToDispStr();

                WorkStep last = null;
                if (gps.Count > 1)
                {
                    pEvalData.WorkMgr.FromFormatted("");
                    last = pEvalData.WorkMgr.GetLast();
                    last.GoDown(ref pEvalData);
                }

                ExComp aderiv = AntiDerivativeHelper.TakeAntiDerivativeGp(gps[i], _dVar, ref integrationInfo, ref pEvalData);

                if (gps.Count > 1)
                    last.GoUp(ref pEvalData);

                if (aderiv == null)
                {
                    pEvalData.WorkMgr.PopStepsCount(pEvalData.WorkMgr.WorkSteps.Count - prevStepCount);
                    _failure = true;
                    return this;
                }

                if (gps.Count > 1)
                    last.WorkHtml = WorkMgr.STM + intStrs[i] + "=" + WorkMgr.ToDisp(aderiv) + WorkMgr.EDM;
                adGps[i] = aderiv;
            }

            // Convert to a term.
            ExComp finalEx = adGps[0];
            for (int i = 1; i < adGps.Length; ++i)
            {
                finalEx = AddOp.StaticCombine(finalEx, adGps[i].ToAlgTerm());
            }

            AlgebraTerm finalTerm = finalEx.ToAlgTerm();
            finalTerm = finalTerm.Order();
            if (adGps.Length > 1)
            {
                string definiteStr = IsDefinite ? "|_{" + _lower.ToAlgTerm().FinalToDispStr() + "}^{" + _upper.ToAlgTerm().FinalToDispStr() + "}" : "";
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + thisStr + "=" + finalTerm.FinalToDispStr() + definiteStr + WorkMgr.EDM,
                    "Add all together.");
            }

            return finalTerm;
        } 

        protected override AlgebraTerm CreateInstance(params ExComp[] args)
        {
            return ConstructIntegral(args[0], this._dVar, LowerLimit, UpperLimit);
        }

        public override string FinalToAsciiString()
        {
            string boundariesStr = "";
            if (IsDefinite)
                boundariesStr = "_{" + LowerLimit.ToAsciiString() + "}^{" + UpperLimit.ToAsciiString() + "}";

            return "\\int" + boundariesStr + (InnerEx is Integral ? InnerTerm.FinalToAsciiString() : "(" + InnerTerm.FinalToAsciiString() + ")") + "\\d" + _dVar.ToAsciiString();
        }

        public override string FinalToTexString()
        {
            string boundariesStr = "";
            if (IsDefinite)
                boundariesStr = "_{" + LowerLimit.ToTexString() + "}^{" + UpperLimit.ToTexString() + "}";
            return "\\int" + boundariesStr + (InnerEx is Integral ? InnerTerm.FinalToTexString() : "(" + InnerTerm.FinalToTexString() + ")") + "\\d" + _dVar.ToTexString();
        }
        
        public override bool IsEqualTo(ExComp ex)
        {
            if (ex is Integral)
            {
                Integral integral = ex as Integral;
                return integral._dVar.IsEqualTo(this._dVar) && integral.InnerEx.IsEqualTo(this.InnerEx);
            }

            return false;
        }

        public override string ToAsciiString()
        {
            string boundariesStr = "";
            if (IsDefinite)
                boundariesStr = "_{" + LowerLimit.ToAsciiString() + "}^{" + UpperLimit.ToAsciiString() + "}";
            return "\\int" + boundariesStr + "(" + InnerTerm.ToAsciiString() + ")\\d" + _dVar.ToAsciiString();
        }

        public override string ToJavaScriptString(bool useRad)
        {
            return null;
        }

        public override string ToString()
        {
            string boundariesStr = "";
            if (IsDefinite)
                boundariesStr = "_{" + LowerLimit.ToString() + "}^{" + UpperLimit.ToString() + "}";
            return "\\int" + boundariesStr + "(" + InnerTerm.ToString() + ")\\d" + _dVar.ToString();
        }

        public override string ToTexString()
        {
            string boundariesStr = "";
            if (IsDefinite)
                boundariesStr = "_{" + LowerLimit.ToTexString() + "}^{" + UpperLimit.ToTexString()+ "}";
            return "\\int" + boundariesStr + "(" + InnerTerm.ToTexString() + ")\\d" + _dVar.ToTexString();
        }

    }
}
