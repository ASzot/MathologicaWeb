namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions
{
    internal class FactorialFunction : AppliedFunction
    {
        public FactorialFunction(ExComp ex)
            : base(ex, FunctionType.Factorial, typeof(FactorialFunction))
        {
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            CallChildren(harshEval, ref pEvalData);

            ExComp innerEx = GetInnerEx();

            if (innerEx is Number && (innerEx as Number).IsRealInteger())
            {
                int num = (int)(innerEx as Number).GetRealComp();

                long factorialResult = 1;

                if (num < 0)
                {
                    return Number.GetUndefined();
                }

                if (num == 0)
                    return Number.GetOne();

                for (int i = 1; i <= num; ++i)
                {
                    factorialResult *= i;
                }

                return new Number(factorialResult);
            }

            return this;
        }

        public override string ToAsciiString()
        {
            return GetInnerEx().ToAsciiString() + "! ";
        }

        public override string ToJavaScriptString(bool useRad)
        {
            return null;
        }

        public override string ToString()
        {
            if (MathSolver.USE_TEX_DEBUG)
                return ToTexString();
            return GetInnerEx().ToString() + "! ";
        }

        public override string FinalToAsciiString()
        {
            return ToAsciiString();
        }

        public override string FinalToTexString()
        {
            return ToTexString();
        }

        public override string ToTexString()
        {
            return GetInnerEx().ToTexString() + "! ";
        }
    }
}