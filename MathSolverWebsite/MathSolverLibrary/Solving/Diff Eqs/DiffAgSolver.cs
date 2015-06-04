using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using System;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Solving.Diff_Eqs
{
    class DiffAgSolver
    {
        public static bool ContainsDerivative(ExComp ex)
        {
            if (ex is Derivative)
                return true;
            else if (ex is AlgebraTerm)
            {
                AlgebraTerm term = ex as AlgebraTerm;
                foreach (ExComp subEx in term.SubComps)
                {
                    if (ContainsDerivative(subEx))
                        return true;
                }
            }

            return false;
        }

        public static bool ContainsDerivative(ExComp[] gp)
        {
            foreach (ExComp ex in gp)
            {
                if (ContainsDerivative(ex))
                    return true;
            }

            return false;
        }

        private static ExComp[] SolveDiffEq(AlgebraTerm ex0Term, AlgebraTerm ex1Term, AlgebraComp solveForFunc,
            AlgebraComp withRespect, int order, ref TermType.EvalData pEvalData)
        {
            ExComp[] atmpt = null;
            int prevWorkStepCount;

            DiffSolve[] diffSolves = new DiffSolve[] { new SeperableSolve(), new HomogeneousSolve() };

            for (int i = 0; i < diffSolves.Length; ++i)
            {
                // Try separable differential equations.
                prevWorkStepCount = pEvalData.WorkMgr.WorkSteps.Count;
                atmpt = diffSolves[i].Solve(ex0Term, ex1Term, solveForFunc, withRespect, ref pEvalData);
                if (atmpt != null)
                {
                    // Add on a constant that will have the properties of a variable.
                    AlgebraComp varConstant = new AlgebraComp("$C");
                    atmpt[1] = Equation.Operators.AddOp.StaticCombine(atmpt[1], varConstant);

                    pEvalData.WorkMgr.FromSides(atmpt[0], atmpt[1], "Add the constant of integration.");

                    return atmpt;
                }
                else
                    pEvalData.WorkMgr.PopSteps(prevWorkStepCount);
            }

            return null;
        }

        public static SolveResult Solve(ExComp ex0, ExComp ex1, AlgebraComp solveForFunc, AlgebraComp withRespect, int order, ref TermType.EvalData pEvalData)
        {
            if (order > 1)
                return SolveResult.Failure("Cannot solve differential equations with an order greater than one", ref pEvalData);

            ExComp[] leftRight = SolveDiffEq(ex0.ToAlgTerm(), ex1.ToAlgTerm(), solveForFunc, withRespect, order, ref pEvalData);
            if (leftRight == null)
                return SolveResult.Failure();

            AlgebraSolver agSolver = new AlgebraSolver();

            int startStepCount = pEvalData.WorkMgr.WorkSteps.Count;
            ExComp solved = agSolver.SolveEq(solveForFunc.Var, leftRight[0].Clone().ToAlgTerm(), leftRight[1].Clone().ToAlgTerm(), ref pEvalData);
            if (solved == null)
            {
                pEvalData.WorkMgr.PopSteps(startStepCount);
                return SolveResult.Solved(leftRight[0], leftRight[1], ref pEvalData);
            }

            return SolveResult.Solved(solveForFunc, solved, ref pEvalData);
        }
    }
}
