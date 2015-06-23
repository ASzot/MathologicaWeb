using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions
{
    internal class SumFunction : AppliedFunction_NArgs
    {
        private const int MAX_SUM_COUNT = 50;
        private const int MAX_WORK_STEP_COUNT = 5;

        public ExComp GetIterCount()
        {
            return _args[3];
        }

        public ExComp GetIterStart()
        {
            return _args[2];
        }

        public AlgebraComp GetIterVar()
        {
            return (AlgebraComp) _args[1];
        }

        public bool GetIsInfiniteSeries()
        {
            return GetIterCount().IsEqualTo(Number.GetPosInfinity());
        }

        public SumFunction(ExComp term, AlgebraComp iterVar, ExComp iterStart, ExComp iterCount)
            : base(FunctionType.Summation, typeof(SumFunction), (iterVar.GetVar().GetVar() == "i" && term is AlgebraTerm) ? (term as AlgebraTerm).ConvertImaginaryToVar() : term, iterVar, iterStart, iterCount)
        {
        }

        public override ExComp CloneEx()
        {
            return new SumFunction(GetInnerEx().CloneEx(), (AlgebraComp)GetIterVar().CloneEx(), GetIterStart().CloneEx(), GetIterCount().CloneEx());
        }

        private bool? Converges(ref TermType.EvalData pEvalData, out ExComp result, ExComp[] gp)
        {
            result = null;
            if (!gp.GroupContains(GetIterVar()))
            {
                return false;
            }

            // Take out the constants.
            ExComp[] varGp;
            ExComp[] constGp;
            gp.GetConstVarTo(out varGp, out constGp, GetIterVar());

            if (constGp.Length != 0)
            {
                pEvalData.GetWorkMgr().FromFormatted(WorkMgr.STM + constGp.ToAlgTerm().FinalToDispStr() + "*\\sum_{" + GetIterVar().ToDispString() + "=" +
                    GetIterStart().ToAlgTerm().FinalToDispStr() + "}^{" + GetIterCount().ToAlgTerm().ToDispString() + "}" + varGp.ToAlgTerm().FinalToDispStr() + WorkMgr.EDM,
                    "Take constants out.");
            }

            AlgebraTerm innerTerm = varGp.ToAlgTerm();
            string innerExStr = innerTerm.FinalToDispStr();
            string thisStrNoMathMark = "\\sum_{" + GetIterVar().ToDispString() + "=" + GetIterStart().ToAlgTerm().FinalToDispStr() + "}^{" +
                GetIterCount().ToAlgTerm().ToDispString() + "}" + innerExStr;
            string thisStr = WorkMgr.STM + thisStrNoMathMark + WorkMgr.EDM;

            // The basic divergence test.
            pEvalData.GetWorkMgr().FromFormatted(thisStr, "If " + WorkMgr.STM +
                "\\lim_{" + GetIterVar().ToDispString() + " \\to \\infty}" + gp.ToAlgTerm().FinalToDispStr() + "\\ne 0 " + WorkMgr.EDM + " then the series is divergent");

            ExComp divTest = Limit.TakeLim(innerTerm, GetIterVar(), Number.GetPosInfinity(), ref pEvalData);
            if (divTest is Limit)
                return null;
            if (divTest is AlgebraTerm)
                divTest = (divTest as AlgebraTerm).RemoveRedundancies();
            if (!divTest.IsEqualTo(Number.GetZero()))
            {
                pEvalData.GetWorkMgr().FromFormatted(thisStr, "The limit did not equal zero, the series is divergent.");
                return false;
            }

            // The p-series test.
            AlgebraTerm[] frac = innerTerm.GetNumDenFrac();
            if (frac != null && frac[0].RemoveRedundancies().IsEqualTo(Number.GetOne()))
            {
                ExComp den = frac[1].RemoveRedundancies();
                if (den is PowerFunction)
                {
                    PowerFunction powFunc = den as PowerFunction;
                    if (powFunc.GetBase().IsEqualTo(GetIterVar()) && powFunc.GetPower() is Number && Number.OpGT((powFunc.GetPower() as Number), 0.0) &&
                        GetIterStart() is Number && Number.OpGE((GetIterStart() as Number), 1.0))
                    {
                        Number nPow = powFunc.GetPower() as Number;
                        bool isConvergent = Number.OpGT(nPow, 1.0);
                        pEvalData.GetWorkMgr().FromFormatted(thisStr, "In the form " + WorkMgr.STM + "1/n^p" + WorkMgr.EDM + " if " +
                            WorkMgr.STM + "p \\gt 1" + WorkMgr.EDM + " then the series is convergent.");
                        if (isConvergent)
                            pEvalData.GetWorkMgr().FromFormatted(thisStr, WorkMgr.STM + "p > 1" + WorkMgr.EDM + " so the series converges");
                        else
                            pEvalData.GetWorkMgr().FromFormatted(thisStr, WorkMgr.STM + "p <= 1" + WorkMgr.EDM + " so the series diverges");
                        return isConvergent;
                    }
                }
            }

            // Geometric series test.
            if (varGp.Length == 2 || varGp.Length == 1)
            {
                ExComp ele0 = varGp[0];
                ExComp ele1 = varGp.Length > 1 ? varGp[1] : Number.GetOne();
                bool ele0Den = false;
                bool ele1Den = false;

                if (ele0 is PowerFunction && (ele0 as PowerFunction).GetPower().IsEqualTo(Number.GetNegOne()))
                {
                    ele0Den = true;
                    ele0 = (ele0 as PowerFunction).GetBase();
                }
                if (ele1 is PowerFunction && (ele1 as PowerFunction).GetPower().IsEqualTo(Number.GetNegOne()))
                {
                    ele1Den = true;
                    ele1 = (ele1 as PowerFunction).GetBase();
                }

                // Needs to be in the form a_n=a*r^n
                ExComp a = null;
                PowerFunction pf = null;
                bool pfDen = false;
                if (ele0 is PowerFunction && (ele0 as PowerFunction).GetPower().ToAlgTerm().Contains(GetIterVar()))
                {
                    pf = ele0 as PowerFunction;
                    a = ele1;
                    pfDen = ele0Den;
                }
                else if (ele1 is PowerFunction && (ele1 as PowerFunction).GetPower().ToAlgTerm().Contains(GetIterVar()))
                {
                    pf = ele1 as PowerFunction;
                    a = ele0;
                    pfDen = ele1Den;
                }

                if (a != null && pf != null && !a.ToAlgTerm().Contains(GetIterVar()) && pf.GetBase() is Number && !(pf.GetBase() as Number).HasImaginaryComp() &&
                    GetIterStart() is Number && Number.OpGE((GetIterStart() as Number), 0.0))
                {
                    Number nBase = pf.GetBase() as Number;
                    ExComp exBase = nBase;
                    if (pfDen)
                        exBase = Number.OpDiv(Number.GetOne(), nBase);

                    if (!(exBase is Number))
                        return null;

                    nBase = exBase as Number;
                    if (pfDen)
                        exBase = DivOp.StaticCombine(Number.GetOne(), pf.GetBase() as Number);

                    nBase = Number.Abs(nBase);
                    AlgebraTerm pfPow = pf.GetPower().ToAlgTerm();
                    List<ExComp> powers = pfPow.GetPowersOfVar(GetIterVar());
                    if (powers.Count == 1 && powers[0].IsEqualTo(Number.GetOne()))
                    {
                        pEvalData.GetWorkMgr().FromFormatted(thisStr, "In the geometric series form " + WorkMgr.STM + "ar^{n-1}" + WorkMgr.EDM + " if " +
                            WorkMgr.STM + "|r| \\lt 1 " + WorkMgr.EDM + " than the series is convergent");
                        if (Number.OpGE(nBase, 1.0))
                        {
                            pEvalData.GetWorkMgr().FromFormatted(thisStr, WorkMgr.STM + "|r| \\ge 1" + WorkMgr.EDM + ", the series is divergent.");
                            return false;
                        }

                        if (pfPow.RemoveRedundancies().IsEqualTo(SubOp.StaticCombine(GetIterVar(), GetIterStart())))
                        {
                            ExComp tmpDen = SubOp.StaticCombine(Number.GetOne(), exBase);
                            tmpDen = tmpDen.ToAlgTerm().CompoundFractions();

                            result = DivOp.StaticCombine(a, tmpDen);

                            string resultStr = result.ToAlgTerm().FinalToDispStr();
                            pEvalData.GetWorkMgr().FromFormatted(WorkMgr.STM + thisStrNoMathMark + "=" + resultStr + WorkMgr.EDM, "Use the formula " + WorkMgr.STM + "\\sum_{n=1}^{\\infty}ar^{n-1}=\\frac{a}{1-r}" +
                                WorkMgr.EDM);

                            if (constGp.Length != 0)
                            {
                                result = MulOp.StaticCombine(result, constGp.ToAlgTerm());
                                pEvalData.GetWorkMgr().FromFormatted(WorkMgr.STM + constGp.ToAlgTerm().FinalToDispStr() + "*\\sum_{" + GetIterVar().ToDispString() + "=" +
                                    GetIterStart().ToAlgTerm().FinalToDispStr() + "}^{" + GetIterCount().ToAlgTerm().ToDispString() + "}" + varGp.ToAlgTerm().FinalToDispStr() + "=" +
                                    WorkMgr.ToDisp(result) + WorkMgr.EDM,
                                    "Multiply the by the constants.");
                            }
                        }

                        pEvalData.GetWorkMgr().FromFormatted(thisStr, WorkMgr.STM + "|r| \\lt 1 " + WorkMgr.EDM + ", the series is convergent.");
                        return true;
                    }
                }
            }

            return null;
        }

        public bool? Converges(ref TermType.EvalData pEvalData, out ExComp result)
        {
            result = null;
            if (!GetIsInfiniteSeries())
                return true;

            if (!GetInnerTerm().Contains(GetIterVar()))
            {
                pEvalData.GetWorkMgr().FromSides(this, null, "The above diverges");
                return false;
            }

            // Split into groups and take the summation of each group independently.
            List<ExComp[]> gps = GetInnerTerm().GetGroupsNoOps();
            ExComp overallResult = Number.GetZero();
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
            CallChildren(harshEval, ref pEvalData);

            // No negative numbers with summations.
            if ((GetIterCount() is Number && Number.OpLT((GetIterCount() as Number), 0.0)) ||
                (GetIterStart() is Number && Number.OpLT((GetIterStart() as Number), 0.0)))
                return this;

            bool toInfinity = GetIterCount() is Number && GetIterCount().IsEqualTo(Number.GetPosInfinity());
            if (!GetInnerTerm().Contains(GetIterVar()))
            {
                // Is this just a sum of numbers.
                if (toInfinity)
                    return Number.GetPosInfinity();

                ExComp sumTotal = Number.GetZero();
                if (!Number.GetOne().IsEqualTo(GetIterStart()))
                    sumTotal = SubOp.StaticCombine(GetIterCount(), SubOp.StaticCombine(GetIterStart(), Number.GetOne()));
                else
                    sumTotal = GetIterCount();
                return MulOp.StaticCombine(sumTotal, GetInnerTerm());
            }

            if (toInfinity)
            {
                ExComp result = null;

                int stepCount = pEvalData.GetWorkMgr().GetWorkSteps().Count;
                bool? converges = Converges(ref pEvalData, out result);
                pEvalData.GetWorkMgr().PopSteps(stepCount);

                if (converges != null)
                {
                    if (result != null)
                        return result;
                    if (!converges.Value)
                        return Number.GetPosInfinity();
                }

                return this;
            }

            ExComp innerEx = GetInnerEx();
            if (innerEx.IsEqualTo(GetIterVar()) && GetIterStart().IsEqualTo(Number.GetOne()))
            {
                return DivOp.StaticCombine(MulOp.StaticCombine(GetIterCount(), AddOp.StaticCombine(GetIterCount(), Number.GetOne())), new Number(2.0));
            }

            if (GetIterCount() is Number && (GetIterCount() as Number).IsRealInteger() &&
                GetIterStart() is Number && (GetIterStart() as Number).IsRealInteger())
            {
                int count = (int)(GetIterCount() as Number).GetRealComp();
                int start = (int)(GetIterStart() as Number).GetRealComp();

                AlgebraTerm totalTerm = new AlgebraTerm(Number.GetZero());

                if (count > MAX_SUM_COUNT)
                    return this;

                ExComp iterVal;

                for (int i = start; i <= count; ++i)
                {
                    iterVal = new Number(i);

                    AlgebraTerm innerTerm = GetInnerTerm().CloneEx().ToAlgTerm();

                    innerTerm = innerTerm.Substitute(GetIterVar(), iterVal);

                    WorkStep lastStep = null;
                    if (count < MAX_WORK_STEP_COUNT)
                    {
                        pEvalData.GetWorkMgr().FromFormatted("", "Evaluate the " + (i + 1).ToString() + (i + 1).GetCountingPrefix() + " term");
                        lastStep = pEvalData.GetWorkMgr().GetLast();
                    }

                    if (lastStep != null)
                        lastStep.GoDown(ref pEvalData);

                    ExComp simpInnerEx = TermType.SimplifyTermType.BasicSimplify(innerTerm.CloneEx().ToAlgTerm().RemoveRedundancies(), ref pEvalData);

                    if (lastStep != null)
                        lastStep.GoUp(ref pEvalData);

                    if (lastStep != null)
                        lastStep.SetWorkHtml(WorkMgr.STM + innerTerm.FinalToDispStr() + "=" + WorkMgr.ToDisp(simpInnerEx) + WorkMgr.EDM);

                    totalTerm = AddOp.StaticCombine(totalTerm, simpInnerEx).ToAlgTerm();
                }

                return totalTerm.ForceCombineExponents();
            }

            return this;
        }

        public override string ToAsciiString()
        {
            return "\\sum_{" + GetIterVar() + "=" + GetIterStart() + "}^{" + GetIterCount() + "}" +
                GetInnerEx().ToAsciiString();
        }

        public override string ToJavaScriptString(bool useRad)
        {
            return null;
        }

        public override string ToString()
        {
            if (MathSolver.USE_TEX_DEBUG)
                return ToTexString();
            return string.Format("Sum({0},{1},{2},{3})", GetInnerTerm().ToString(),
                GetIterCount().ToString(), GetIterStart().ToString(),
                GetIterVar().ToString());
        }

        public override string ToTexString()
        {
            return "\\Sigma_{" + GetIterVar() + "=" + GetIterStart() + "}^{" + GetIterCount() + "}" +
                GetInnerEx().ToTexString();
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn)
        {
            return new SumFunction(GetInnerTerm().Substitute(subOut, subIn),
                GetIterVar().IsEqualTo(subOut) ? (AlgebraComp)subIn : GetIterVar(),
                GetIterStart().ToAlgTerm().Substitute(subOut, subIn).RemoveRedundancies(),
                GetIterCount().ToAlgTerm().Substitute(subOut, subIn).RemoveRedundancies());
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