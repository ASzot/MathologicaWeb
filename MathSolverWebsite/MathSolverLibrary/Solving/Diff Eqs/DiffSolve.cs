using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;

namespace MathSolverWebsite.MathSolverLibrary.Solving.Diff_Eqs
{
    internal abstract class DiffSolve
    {
        public abstract ExComp[] Solve(AlgebraTerm left, AlgebraTerm right, AlgebraComp funcVar, AlgebraComp dVar,
            ref TermType.EvalData pEvalData);

        protected static AlgebraTerm ConvertDerivsToAlgebraComps(AlgebraTerm term, AlgebraComp funcDeriv, AlgebraComp dVar, ref AlgebraComp replaceCmp)
        {
            if (replaceCmp == null)
                replaceCmp = new AlgebraComp("\\frac{d" + funcDeriv.ToDispString() + "}{d" + dVar.ToDispString() + "}");

            if (term is Derivative && (term as Derivative).DerivOf != null && (term as Derivative).DerivOf.IsEqualTo(funcDeriv))
                return replaceCmp.ToAlgTerm();

            for (int i = 0; i < term.TermCount; ++i)
            {
                if (term[i] is AlgebraTerm)
                    term[i] = ConvertDerivsToAlgebraComps(term[i] as AlgebraTerm, funcDeriv, dVar, ref replaceCmp);
                if (term[i] is Derivative && (term[i] as Derivative).DerivOf != null && (term[i] as Derivative).DerivOf.IsEqualTo(funcDeriv))
                {
                    term[i] = replaceCmp;
                }
            }

            return term;
        }
    }
}