using MathSolverWebsite.MathSolverLibrary.Equation.Functions;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg
{
    internal class MatrixInverse : AppliedFunction
    {
        public MatrixInverse(ExComp exMat)
            : base(exMat, FunctionType.MatInverse, typeof(MatrixInverse))
        {
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            CallChildren(harshEval, ref pEvalData);

            ExMatrix mat = InnerEx as ExMatrix;
            if (mat == null)
                return Number.Undefined;

            return mat.GetInverse();
        }

        public override string FinalToAsciiString()
        {
            return InnerTerm.FinalToAsciiString() + "^{-1}";
        }

        public override string FinalToTexString()
        {
            return InnerTerm.FinalToTexString() + "^{-1}";
        }

        public override string ToAsciiString()
        {
            return InnerTerm.ToAsciiString() + "^{-1}";
        }

        public override string ToTexString()
        {
            return ToTexString();
        }
    }
}