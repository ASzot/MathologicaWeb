using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using System;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Solving.Diff_Eqs
{
    class ExactEqsSolve : DiffSolve
    {
        public ExactEqsSolve()
        {

        }

        public override ExComp[] Solve(AlgebraTerm left, AlgebraTerm right, AlgebraComp funcVar, AlgebraComp dVar,
            ref TermType.EvalData pEvalData)
        {
            // In the form N(x,y)(dy/dx)+M(x,y)=0

            // As the order is one convert to a variable representation of the derivatives.
            AlgebraComp derivVar = null;
            left = ConvertDerivsToAlgebraComps(left, funcVar, dVar, ref derivVar);
            right = ConvertDerivsToAlgebraComps(right, funcVar, dVar, ref derivVar);

            // Move everything to the left.
            if (left.IsZero())
            {
                left = SubOp.StaticCombine(left, right).ToAlgTerm();
                right = Number.Zero.ToAlgTerm();
                pEvalData.WorkMgr.FromSides(left, right, "Move everything to the left hand side.");
            }

            // Find N(x,y)
            List<AlgebraGroup> varGps = left.GetGroupsVariableToNoOps(derivVar);
            AlgebraTerm funcN = AlgebraGroup.GetConstantTo(varGps, derivVar);

            // Find M(x,y)
            List<AlgebraGroup> constGps = left.GetGroupsConstantTo(derivVar);
            AlgebraTerm funcM = AlgebraGroup.ToTerm(constGps);

            // Does M_y = N_x ? 
            ExComp my = Derivative.TakeDeriv(funcM.Clone(), funcVar, ref pEvalData, true);
            ExComp nx = Derivative.TakeDeriv(funcN.Clone(), dVar, ref pEvalData, true);

            if (my is AlgebraTerm)
                my = (my as AlgebraTerm).RemoveRedundancies();
            if (nx is AlgebraTerm)
                nx = (nx as AlgebraTerm).RemoveRedundancies();

            if (!my.IsEqualTo(nx))
                return null;

            ExComp psi = Integral.TakeAntiDeriv(funcM, dVar, ref pEvalData);
            bool constX = false;
            if (psi is Integral)
            {
                // The integration failed.
                // Maybe integrating the other function will work.
                psi = Integral.TakeAntiDeriv(funcN, funcVar, ref pEvalData);
                constX = true;
            }

            if (psi is Integral)
                return null;

            // Now solve for the constant function.
            AlgebraComp solveForConstVar = constX ? dVar : funcVar;

            ExComp tmpPsi = Derivative.TakeDeriv(psi, solveForConstVar, ref pEvalData, true);

            // Find the difference.
            ExComp derivConstFunc = SubOp.StaticCombine(constX ? funcM : funcN, tmpPsi);
            // Negate because we subtracted.
            derivConstFunc = MulOp.Negate(derivConstFunc);

            ExComp constFunc = Integral.TakeAntiDeriv(derivConstFunc, solveForConstVar, ref pEvalData);
            if (constFunc is Integral)
                return null;

            psi = AddOp.StaticCombine(psi, constFunc);

            return new ExComp[] { psi, Number.Zero };
        }
    }
}