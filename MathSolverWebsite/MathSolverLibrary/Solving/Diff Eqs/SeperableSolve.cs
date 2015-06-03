using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using System;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Solving.Diff_Eqs
{
    class SeperableSolve : DiffSolve
    {


        public static ExComp[] SolveSeperable(AlgebraTerm left, AlgebraTerm right, AlgebraComp funcVar, AlgebraComp dVar,
            ref TermType.EvalData pEvalData)
        {
            // In the form N(x)dy/dx = M(x)

            // As the order is one convert to a variable representation of the derivatives.
            AlgebraComp derivVar = null;
            left = ConvertDerivsToAlgebraComps(left, funcVar, dVar, ref derivVar);
            right = ConvertDerivsToAlgebraComps(right, funcVar, dVar, ref derivVar);

            // Move the dy/dx to the left.
            SolveMethod.VariablesToLeft(ref left, ref right, derivVar, ref pEvalData);

            // Move everything else to the right.
            SolveMethod.ConstantsToRight(ref left, ref right, derivVar, ref pEvalData);

            // Combine the fractions.
            SolveMethod.CombineFractions(ref left, ref right, ref pEvalData);

            SolveMethod.DivideByVariableCoeffs(ref left, ref right, derivVar, ref pEvalData);

            AlgebraTerm[] numDen = right.GetNumDenFrac();

            if (numDen == null)
            {
                numDen = new AlgebraTerm[] { right, Number.One.ToAlgTerm() };
            }

            // The 'dx' on the left is implied.
            // The 'dy' on the right is implied.

            if (!numDen[0].Contains(dVar) && !numDen[1].Contains(funcVar))
            {
                // Just cross multiply.
                left = numDen[0];
                right = numDen[1];
            }
            else if (!numDen[0].Contains(funcVar) && !numDen[1].Contains(dVar))
            {
                left = AlgebraTerm.FromFraction(Number.One, left);
                right = AlgebraTerm.FromFraction(Number.One, right);
            }
            else
                return null;

            // Integrate both sides.
            ExComp leftIntegrated = Integral.TakeAntiDeriv(left, funcVar, ref pEvalData);
            ExComp rightIntegrated = Integral.TakeAntiDeriv(right, dVar, ref pEvalData);

            // Add in the constant.
            rightIntegrated = AddOp.StaticCombine(rightIntegrated, new CalcConstant());

            return new ExComp[] { leftIntegrated, rightIntegrated };
        }
    }
}