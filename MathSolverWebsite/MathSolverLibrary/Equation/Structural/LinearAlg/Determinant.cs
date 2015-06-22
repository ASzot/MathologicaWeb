using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg
{
    internal class Determinant : AppliedFunction
    {
        private const int MAX_DET_DIMEN = 4;

        public Determinant(ExComp innerMat)
            : base(innerMat, FunctionType.Deteriment, typeof(Determinant))
        {
        }

        public static ExComp TakeDeteriment(ExMatrix mat)
        {
            if (mat.Rows == 2)
            {
                ExComp a = mat.Get(0, 0);
                ExComp b = mat.Get(0, 1);
                ExComp c = mat.Get(1, 0);
                ExComp d = mat.Get(1, 1);

                return SubOp.StaticCombine(MulOp.StaticCombine(a, d), MulOp.StaticCombine(b, c));
            }

            ExComp total = Number.Zero;
            for (int i = 0; i < mat.Cols; ++i)
            {
                ExComp factor = mat.Get(0, i);
                ExComp cofactor = mat.GetCofactor(0, i);
                ExComp comp = MulOp.StaticCombine(factor, cofactor);

                total = AddOp.StaticCombine(total, comp);
            }

            return total;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            CallChildren(harshEval, ref pEvalData);

            ExComp innerEx = InnerEx;

            ExMatrix mat = innerEx as ExMatrix;
            if (mat == null || !mat.IsSquare)
            {
                pEvalData.AddMsg("Only the deteriment of square matrices can be taken");
                return Number.Undefined;
            }

            if (mat.Rows > MAX_DET_DIMEN)
                return this;

            return TakeDeteriment(mat);
        }

        public override string FinalToAsciiString()
        {
            return "\\text{det}" + InnerTerm.FinalToAsciiString();
        }

        public override string FinalToTexString()
        {
            return "\\text{det}" + InnerTerm.FinalToTexString();
        }

        public override string ToTexString()
        {
            return "\\text{det}" + InnerEx.ToTexString();
        }

        public override string ToAsciiString()
        {
            return "\\text{det}" + InnerEx.ToAsciiString();
        }

        public override string ToString()
        {
            return ToTexString();
        }
    }
}