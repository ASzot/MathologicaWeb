using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using System;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Solving.Diff_Eqs
{
    class IntegratingFactorSolve : DiffSolve
    {
        public IntegratingFactorSolve()
        {

        }

        public override Equation.ExComp[] Solve(Equation.AlgebraTerm left, Equation.AlgebraTerm right, Equation.AlgebraComp funcVar, Equation.AlgebraComp dVar, ref TermType.EvalData pEvalData)
        {
            // In the form dy/dx + N(x)y = M(x)

            // As the order is one convert to a variable representation of the derivatives.
            AlgebraComp derivVar = null;
            left = SeperableSolve.ConvertDerivsToAlgebraComps(left, funcVar, dVar, ref derivVar);
            right = SeperableSolve.ConvertDerivsToAlgebraComps(right, funcVar, dVar, ref derivVar);

            // Move the dy/dx to the left.
            SolveMethod.VariablesToLeft(ref left, ref right, derivVar, ref pEvalData);

            // Move N(x)y to the left.
            SolveMethod.VariablesToLeft(ref left, ref right, funcVar, ref pEvalData);

            // Move M(x) to the right.
            SolveMethod.ConstantsToRight(ref left, ref right, new AlgebraComp[] { funcVar, derivVar }, ref pEvalData);

            SolveMethod.DivideByVariableCoeffs(ref left, ref right, derivVar, ref pEvalData);

            List<AlgebraGroup> ags = left.GetGroupsVariableTo(derivVar);
            AlgebraTerm nx = AlgebraGroup.ToTerm(ags);

            if (nx.Contains(funcVar))
                return null;

            ExComp ix = Integral.TakeAntiDeriv(nx, dVar, ref pEvalData);

            ix = new PowerFunction(Constant.E, ix);
            ix = new AlgebraTerm(ix);
            (ix as AlgebraTerm).EvaluateFunctions(false, ref pEvalData);

            left = MulOp.StaticCombine(ix, funcVar).ToAlgTerm();
            right = MulOp.StaticCombine(ix, right).ToAlgTerm();

            right = Integral.TakeAntiDeriv(right, dVar, ref pEvalData).ToAlgTerm();

            return new ExComp[] { left, right };
        }
    }
}