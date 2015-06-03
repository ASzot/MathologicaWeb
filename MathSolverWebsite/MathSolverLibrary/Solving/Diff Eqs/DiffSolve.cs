using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using System;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Solving.Diff_Eqs
{
    abstract class DiffSolve
    {
        protected static AlgebraTerm ConvertDerivsToAlgebraComps(AlgebraTerm term, AlgebraComp funcDeriv, AlgebraComp dVar, ref AlgebraComp replaceCmp)
        {
            if (replaceCmp == null)
                replaceCmp = new AlgebraComp("\\frac{" + funcDeriv.ToDispString() + "}{" + dVar.ToDispString() + "}");
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