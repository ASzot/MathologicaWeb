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

        public bool? Converges(ref TermType.EvalData pEvalData, out ExComp result)
        {
            result = null;
            if (!IsInfiniteSeries)
                return true;

            if (!InnerTerm.Contains(IterVar))
                return false;

            // The basic divergence test.
            ExComp divTest = Limit.TakeLim(InnerEx, IterVar, Number.PosInfinity, ref pEvalData);
            if (divTest.IsEqualTo(Number.Zero))
                return false;

            ExComp innerEx = InnerEx;

            // The p-series test.
            if (innerEx is PowerFunction)
            {
                PowerFunction powFunc = innerEx as PowerFunction;
                if (powFunc.Base.IsEqualTo(IterVar) && powFunc.Power is Number && (powFunc.Power as Number) < 0.0)
                {
                    Number nPow = powFunc.Power as Number;
                    return nPow <= -1.0;
                }
            }

            List<ExComp[]> gps = InnerTerm.GetGroupsNoOps();

            // Geometric series test.
            if (gps.Count == 1 && gps[0].Length == 2)
            {
                ExComp ele0 = gps[0][0];
                ExComp ele1 = gps[0][1];

                // Needs to be in the form a_n=a*r^n
                ExComp a = null;
                PowerFunction pf = null;
                if (ele0 is PowerFunction && (ele0 as PowerFunction).Power.ToAlgTerm().Contains(IterVar))
                {
                    pf = ele0 as PowerFunction;
                    a = ele1;
                }
                else if (ele1 is PowerFunction && (ele1 as PowerFunction).Power.ToAlgTerm().Contains(IterVar))
                {
                    pf = ele1 as PowerFunction;
                    a = ele0;
                }

                if (a != null && pf != null && !a.ToAlgTerm().Contains(IterVar) && pf.Base is Number && !(pf.Base as Number).HasImaginaryComp())
                {
                    Number nBase = pf.Base as Number;
                    nBase = Number.Abs(nBase);
                    AlgebraTerm pfPow = pf.Power.ToAlgTerm();
                    List<ExComp> powers = pfPow.GetPowersOfVar(IterVar);
                    if (powers.Count == 1 && powers[0].IsEqualTo(Number.One))
                    {
                        if (nBase > 1.0)
                            return false;
                        if (pfPow.IsEqualTo(new AlgebraTerm(IterVar, new SubOp(), IterStart)))
                            result = DivOp.StaticCombine(a, SubOp.StaticCombine(Number.One, nBase));
                        return true;
                    }
                }
            }

            return null;
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
                return this;

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

                    ExComp simpInnerEx = TermType.SimplifyTermType.BasicSimplify(innerTerm.RemoveRedundancies(), ref pEvalData);

                    totalTerm = AddOp.StaticCombine(totalTerm, simpInnerEx).ToAlgTerm();
                }

                return totalTerm.ForceCombineExponents();
            }

            return this;
        }

        public override string ToAsciiString()
        {
            return "\\Sigma_{" + IterVar + "=" + IterStart + "}^{" + IterCount + "}" +
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