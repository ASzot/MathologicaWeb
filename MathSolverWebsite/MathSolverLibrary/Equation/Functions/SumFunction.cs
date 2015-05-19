using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions
{
    internal class SumFunction : AppliedFunction_NArgs
    {
        private const int MAX_SUM_COUNT = 50;

        public ExComp IterCount
        {
            get { return _args[3]; }
        }

        public ExComp IterStart
        {
            get { return _args[2]; }
        }

        public AlgebraComp IterVar
        {
            get { return (AlgebraComp)_args[1]; }
        }

        public bool IsInfiniteSeries
        {
            get { return IterCount.IsEqualTo(Number.PosInfinity); }
        }

        public SumFunction(ExComp term, AlgebraComp iterVar, ExComp iterStart, ExComp iterCount)
            : base(FunctionType.Summation, typeof(SumFunction), (iterVar.Var.Var == "i" && term is AlgebraTerm) ? (term as AlgebraTerm).ConvertImaginaryToVar() : term, iterVar, iterStart, iterCount)
        {
        }

        public override ExComp Clone()
        {
            return new SumFunction(InnerEx.Clone(), (AlgebraComp)IterVar.Clone(), IterStart.Clone(), IterCount.Clone());
        }

        private bool? Converges(ref TermType.EvalData pEvalData, out ExComp result, ExComp[] gp)
        {
            result = null;
            if (!gp.GroupContains(IterVar))
            {
                return false;
            }

            // Take out the constants.
            ExComp[] varGp;
            ExComp[] constGp;
            gp.GetConstVarTo(out varGp, out constGp, IterVar);

            if (constGp.Length != 0)
            {
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + constGp.ToAlgTerm().FinalToDispStr() + "*\\sum_{" + IterVar.ToDispString() + "=" + 
                    IterStart.ToAlgTerm().FinalToDispStr() + "}^{" + IterCount.ToAlgTerm().ToDispString() + "}" + varGp.ToAlgTerm().FinalToDispStr() + WorkMgr.EDM,
                    "Take constants out.");
            }


            AlgebraTerm innerTerm = varGp.ToAlgTerm();
            string innerExStr = innerTerm.FinalToDispStr();
            string thisStrNoMathMark = "\\sum_{" + IterVar.ToDispString() + "=" + IterStart.ToAlgTerm().FinalToDispStr() + "}^{" +
                IterCount.ToAlgTerm().ToDispString() + "}" + innerExStr;
            string thisStr = WorkMgr.STM + thisStrNoMathMark + WorkMgr.EDM;

            // The basic divergence test.
            pEvalData.WorkMgr.FromFormatted(thisStr, "If " + WorkMgr.STM +
                "\\lim_{" + IterVar.ToDispString() + " \\to \\infty}" + gp.ToAlgTerm().FinalToDispStr() + "\\ne 0 " + WorkMgr.EDM + " then the series is divergent");

            ExComp divTest = Limit.TakeLim(innerTerm, IterVar, Number.PosInfinity, ref pEvalData);
            if (divTest is Limit)
                return null;
            if (divTest is AlgebraTerm)
                divTest = (divTest as AlgebraTerm).RemoveRedundancies();
            if (!divTest.IsEqualTo(Number.Zero))
            {
                pEvalData.WorkMgr.FromFormatted(thisStr, "The limit did not equal zero, the series is divergent.");
                return false;
            }

            // The p-series test.
            AlgebraTerm[] frac = innerTerm.GetNumDenFrac();
            if (frac != null && frac[0].RemoveRedundancies().IsEqualTo(Number.One))
            {
                ExComp den = frac[1].RemoveRedundancies();
                if (den is PowerFunction)
                {
                    PowerFunction powFunc = den as PowerFunction;
                    if (powFunc.Base.IsEqualTo(IterVar) && powFunc.Power is Number && (powFunc.Power as Number) > 0.0 &&
                        IterStart is Number && (IterStart as Number) >= 1.0)
                    {
                        Number nPow = powFunc.Power as Number;
                        bool isConvergent = nPow > 1.0;
                        pEvalData.WorkMgr.FromFormatted(thisStr, "In the form " + WorkMgr.STM + "1/n^p" + WorkMgr.EDM + " if " +
                            WorkMgr.STM + "p \\gt 1" + WorkMgr.EDM + " then the series is convergent.");
                        if (isConvergent)
                            pEvalData.WorkMgr.FromFormatted(thisStr, WorkMgr.STM + "p > 1" + WorkMgr.EDM + " so the series converges");
                        else
                            pEvalData.WorkMgr.FromFormatted(thisStr, WorkMgr.STM + "p <= 1" + WorkMgr.EDM + " so the series diverges");
                        return isConvergent;
                    }
                }
            }

            // Geometric series test.
            if (varGp.Length == 2 || varGp.Length == 1)
            {
                ExComp ele0 = varGp[0];
                ExComp ele1 = varGp.Length > 1 ? varGp[1] : Number.One;
                bool ele0Den = false;
                bool ele1Den = false;

                if (ele0 is PowerFunction && (ele0 as PowerFunction).Power.IsEqualTo(Number.NegOne))
                {
                    ele0Den = true;
                    ele0 = (ele0 as PowerFunction).Base;
                }
                if (ele1 is PowerFunction && (ele1 as PowerFunction).Power.IsEqualTo(Number.NegOne))
                {
                    ele1Den = true;
                    ele1 = (ele1 as PowerFunction).Base;
                }

                // Needs to be in the form a_n=a*r^n
                ExComp a = null;
                PowerFunction pf = null;
                bool pfDen = false;
                if (ele0 is PowerFunction && (ele0 as PowerFunction).Power.ToAlgTerm().Contains(IterVar))
                {
                    pf = ele0 as PowerFunction;
                    a = ele1;
                    pfDen = ele0Den;
                }
                else if (ele1 is PowerFunction && (ele1 as PowerFunction).Power.ToAlgTerm().Contains(IterVar))
                {
                    pf = ele1 as PowerFunction;
                    a = ele0;
                    pfDen = ele1Den;
                }

                if (a != null && pf != null && !a.ToAlgTerm().Contains(IterVar) && pf.Base is Number && !(pf.Base as Number).HasImaginaryComp() &&
                    IterStart is Number && (IterStart as Number) >= 0.0)
                {
                    Number nBase = pf.Base as Number;
                    ExComp exBase = nBase;
                    if (pfDen)
                        exBase = Number.One / nBase;

                    if (!(exBase is Number))
                        return null;

                    nBase = exBase as Number;
                    if (pfDen)
                        exBase = DivOp.StaticCombine(Number.One, pf.Base as Number);

                    nBase = Number.Abs(nBase);
                    AlgebraTerm pfPow = pf.Power.ToAlgTerm();
                    List<ExComp> powers = pfPow.GetPowersOfVar(IterVar);
                    if (powers.Count == 1 && powers[0].IsEqualTo(Number.One))
                    {
                        pEvalData.WorkMgr.FromFormatted(thisStr, "In the geometric series form " + WorkMgr.STM + "ar^{n-1}" + WorkMgr.EDM + " if " +
                            WorkMgr.STM + "|r| \\lt 1 " + WorkMgr.EDM + " than the series is convergent");
                        if (nBase >= 1.0)
                        {
                            pEvalData.WorkMgr.FromFormatted(thisStr, WorkMgr.STM + "|r| \\ge 1" + WorkMgr.EDM + ", the series is divergent.");
                            return false;
                        }


                        if (pfPow.RemoveRedundancies().IsEqualTo(SubOp.StaticCombine(IterVar, IterStart)))
                        {
                            ExComp tmpDen = SubOp.StaticCombine(Number.One, exBase);
                            tmpDen = tmpDen.ToAlgTerm().CompoundFractions();

                            result = DivOp.StaticCombine(a, tmpDen);

                            string resultStr = result.ToAlgTerm().FinalToDispStr();
                            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + thisStrNoMathMark + "=" + resultStr + WorkMgr.EDM, "Use the formula " + WorkMgr.STM + "\\sum_{n=1}^{\\infty}ar^{n-1}=\\frac{a}{1-r}" +
                                WorkMgr.EDM);

                            if (constGp.Length != 0)
                            {
                                result = MulOp.StaticCombine(result, constGp.ToAlgTerm());
                                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + constGp.ToAlgTerm().FinalToDispStr() + "*\\sum_{" + IterVar.ToDispString() + "=" + 
                                    IterStart.ToAlgTerm().FinalToDispStr() + "}^{" + IterCount.ToAlgTerm().ToDispString() + "}" + varGp.ToAlgTerm().FinalToDispStr() + "=" +
                                    WorkMgr.ToDisp(result) + WorkMgr.EDM,
                                    "Multiply the by the constants.");
                            }
                        }

                        pEvalData.WorkMgr.FromFormatted(thisStr, WorkMgr.STM + "|r| \\lt 1 " + WorkMgr.EDM + ", the series is convergent.");
                        return true;
                    }
                }
            }

            return null;
        }

        public bool? Converges(ref TermType.EvalData pEvalData, out ExComp result)
        {
            result = null;
            if (!IsInfiniteSeries)
                return true;

            if (!InnerTerm.Contains(IterVar))
            {
                pEvalData.WorkMgr.FromSides(this, null, "The above diverges");
                return false;
            }

            // Split into groups and take the summation of each group independently.
            List<ExComp[]> gps = InnerTerm.GetGroupsNoOps();
            ExComp overallResult = Number.Zero;
            for (int i = 0; i < gps.Count; ++i)
            {
                ExComp gpResult;
                bool? gpConverges = Converges(ref pEvalData, out gpResult, gps[0]);
                if (gpConverges == null || !gpConverges.Value)
                    return gpConverges;

                if (gpResult != null && overallResult != null)
                    overallResult = AddOp.StaticCombine(overallResult, gpResult);
                if (gpResult == null)
                    overallResult = null;
            }

            if (overallResult != null)
                result = overallResult;

            return true;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            // No negative numbers with summations.
            if ((IterCount is Number && (IterCount as Number) < 0.0) ||
                (IterStart is Number && (IterStart as Number) < 0.0))
                return this;

            bool toInfinity = IterCount is Number && IterCount.IsEqualTo(Number.PosInfinity);
            if (!InnerTerm.Contains(IterVar))
            {
                // Is this just a sum of numbers.
                if (toInfinity)
                    return Number.PosInfinity;

                ExComp sumTotal = Number.Zero;
                if (!Number.One.IsEqualTo(IterStart))
                    sumTotal = SubOp.StaticCombine(IterCount, SubOp.StaticCombine(IterStart, Number.One));
                else
                    sumTotal = IterCount;
                return MulOp.StaticCombine(sumTotal, InnerTerm);
            }

            if (toInfinity)
            {
                ExComp result = null;

                int stepCount = pEvalData.WorkMgr.WorkSteps.Count;
                bool? converges = Converges(ref pEvalData, out result);
                pEvalData.WorkMgr.PopSteps(stepCount);

                if (converges != null)
                {
                    if (result != null)
                        return result;
                    if (!converges.Value)
                        return Number.PosInfinity;
                }

                return this;
            }

            ExComp innerEx = InnerEx;
            if (innerEx.IsEqualTo(IterVar) && IterStart.IsEqualTo(Number.One))
            {
                return DivOp.StaticCombine(MulOp.StaticCombine(IterCount, AddOp.StaticCombine(IterCount, Number.One)), new Number(2.0));
            }

            if (IterCount is Number && (IterCount as Number).IsRealInteger() &&
                IterStart is Number && (IterStart as Number).IsRealInteger())
            {
                int count = (int)(IterCount as Number).RealComp;
                int start = (int)(IterStart as Number).RealComp;

                AlgebraTerm totalTerm = new AlgebraTerm(Number.Zero);

                if (count > MAX_SUM_COUNT)
                    return this;

                ExComp iterVal;

                for (int i = start; i <= count; ++i)
                {
                    iterVal = new Number(i);

                    AlgebraTerm innerTerm = InnerTerm.Clone().ToAlgTerm();

                    innerTerm = innerTerm.Substitute(IterVar, iterVal);
                    pEvalData.WorkMgr.FromFormatted("", "Evaluate the " + (i + 1).ToString() + (i + 1).GetCountingPrefix() + " term");
                    WorkStep lastStep = pEvalData.WorkMgr.GetLast();

                    lastStep.GoDown(ref pEvalData);
                    ExComp simpInnerEx = TermType.SimplifyTermType.BasicSimplify(innerTerm.Clone().ToAlgTerm().RemoveRedundancies(), ref pEvalData);
                    lastStep.GoUp(ref pEvalData);

                    lastStep.WorkHtml = WorkMgr.STM + innerTerm.FinalToDispStr() + "=" + WorkMgr.ToDisp(simpInnerEx) + WorkMgr.EDM;

                    totalTerm = AddOp.StaticCombine(totalTerm, simpInnerEx).ToAlgTerm();
                }

                return totalTerm.ForceCombineExponents();
            }

            return this;
        }

        public override string ToAsciiString()
        {
            return "\\sum_{" + IterVar + "=" + IterStart + "}^{" + IterCount + "}" +
                InnerEx.ToAsciiString();
        }

        public override string ToJavaScriptString(bool useRad)
        {
            return null;
        }

        public override string ToString()
        {
            if (MathSolver.USE_TEX_DEBUG)
                return ToTexString();
            return string.Format("Sum({0},{1},{2},{3})", InnerTerm.ToString(),
                IterCount.ToString(), IterStart.ToString(),
                IterVar.ToString());
        }

        public override string ToTexString()
        {
            return "\\Sigma_{" + IterVar + "=" + IterStart + "}^{" + IterCount + "}" +
                InnerEx.ToTexString();
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn)
        {
            return new SumFunction(InnerTerm.Substitute(subOut, subIn), 
                IterVar.IsEqualTo(subOut) ? (AlgebraComp)subIn : IterVar, 
                IterStart.ToAlgTerm().Substitute(subOut, subIn).RemoveRedundancies(),
                IterStart.ToAlgTerm().Substitute(subOut, subIn).RemoveRedundancies());
        }

        protected override AlgebraTerm CreateInstance(params ExComp[] args)
        {
            ExComp useArg1;
            if (args[1] is AlgebraTerm)
                useArg1 = (args[1] as AlgebraTerm).RemoveRedundancies();
            else
                useArg1 = args[1];

            return new SumFunction(args[0], (AlgebraComp)useArg1, args[2], args[3]);
        }
    }
}