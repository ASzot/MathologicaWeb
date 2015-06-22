namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions
{
    internal class PermutationFunction : AppliedFunction_NArgs
    {
        private const string IDEN = "P";

        public ExComp Bottom
        {
            get { return _args[1]; }
            set { _args[1] = value; }
        }

        public ExComp Top
        {
            get { return _args[0]; }
            set { _args[0] = value; }
        }

        public AlgebraTerm TopTerm
        {
            get { return Top.ToAlgTerm(); }
        }

        public AlgebraTerm BottomTerm
        {
            get { return Bottom.ToAlgTerm(); }
        }

        public PermutationFunction(ExComp top, ExComp bottom)
            : base(FunctionType.Permutation, typeof(PermutationFunction),
            top is AlgebraTerm ? (top as AlgebraTerm).RemoveRedundancies() : top,
            bottom is AlgebraTerm ? (bottom as AlgebraTerm).RemoveRedundancies() : bottom)
        {
        }

        public override AlgebraTerm ConvertImaginaryToVar()
        {
            ExComp bottom, top;
            if (Bottom is AlgebraTerm)
                bottom = (Bottom as AlgebraTerm).ConvertImaginaryToVar();
            else
                bottom = Bottom;
            if (Top is AlgebraTerm)
                top = (Top as AlgebraTerm).ConvertImaginaryToVar();
            else
                top = Top;

            return new ChooseFunction(top, bottom);
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            CallChildren(harshEval, ref pEvalData);

            ExComp n = Top;
            ExComp k = Bottom;

            if (n is Number && k is Number && (n as Number).IsRealInteger() && (k as Number).IsRealInteger())
            {
                FactorialFunction nFactorial = new FactorialFunction(n);

                FactorialFunction nMinusKFactorial = new FactorialFunction(Operators.SubOp.StaticCombine(n, k));

                ExComp nFactEval = nFactorial.Evaluate(harshEval, ref pEvalData);
                if (Number.IsUndef(nFactEval))
                    return Number.Undefined;

                ExComp nMinusKFactEval = nMinusKFactorial.Evaluate(harshEval, ref pEvalData);
                if (Number.IsUndef(nMinusKFactEval))
                    return Number.Undefined;

                return Operators.DivOp.StaticCombine(nFactEval, nMinusKFactEval);
            }

            return this;
        }

        public override string ToAsciiString()
        {
            return IDEN + "(" + Top.ToAsciiString() + ", " + Bottom.ToAsciiString() + ")";
        }

        public override string ToJavaScriptString(bool useRad)
        {
            return null;
        }

        public override string ToString()
        {
            return ToTexString();
        }

        public override string ToTexString()
        {
            return IDEN + "(" + Top.ToTexString() + ", " + Bottom.ToTexString() + ")";
        }

        public override string FinalToAsciiString()
        {
            return IDEN + "(" + TopTerm.FinalToAsciiString() + ", " + BottomTerm.FinalToAsciiString() + ")";
        }

        public override string FinalToTexString()
        {
            return IDEN + "( " + TopTerm.FinalToTexString() + ", " + BottomTerm.FinalToTexString() + ")";
        }
    }
}