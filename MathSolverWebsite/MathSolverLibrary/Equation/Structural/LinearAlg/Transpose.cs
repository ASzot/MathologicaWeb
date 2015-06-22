using MathSolverWebsite.MathSolverLibrary.Equation.Functions;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg
{
    internal class Transpose : AppliedFunction
    {
        public Transpose(ExComp exMat)
            : base(exMat, FunctionType.Transpose, typeof(Transpose))
        {
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            CallChildren(harshEval, ref pEvalData);

            ExMatrix mat = InnerEx as ExMatrix;
            if (mat == null)
                return Number.Undefined;

            return mat.Transpose();
        }

        public override string FinalToAsciiString()
        {
            return InnerTerm.FinalToAsciiString() + "^{T}";
        }

        public override string FinalToTexString()
        {
            return InnerTerm.FinalToTexString() + "^{T}";
        }

        public override string ToAsciiString()
        {
            return InnerTerm.ToAsciiString() + "^{T}";
        }

        public override string ToTexString()
        {
            return ToTexString();
        }
    }
}