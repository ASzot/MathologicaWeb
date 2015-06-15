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

            DiffSolve[] diffSolves = new DiffSolve[] { new SeperableSolve(), new HomogeneousSolve(), new IntegratingFactorSolve(), new ExactEqsSolve() };

            for (int i = 0; i < diffSolves.Length; ++i)
            {
                // Try separable differential equations.
                prevWorkStepCount = pEvalData.WorkMgr.WorkSteps.Count;
                atmpt = diffSolves[i].Solve((AlgebraTerm)ex0Term.Clone(), (AlgebraTerm)ex1Term.Clone(), solveForFunc, withRespect, ref pEvalData);
                if (atmpt != null)
                {
                    if (!(atmpt[0] is Integral || atmpt[1] is Integral))
                    {
                        // Add on a constant that will have the properties of a variable.
                        AlgebraComp varConstant = new AlgebraComp("$C");
                        atmpt[1] = Equation.Operators.AddOp.StaticCombine(atmpt[1], varConstant);

                        pEvalData.WorkMgr.FromSides(atmpt[0], atmpt[1], "Add the constant of integration.");
                    }

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

            Solution genSol = new Solution(leftRight[0], leftRight[1]);
            genSol.IsGeneral = true;

            if (leftRight[0] is Integral || leftRight[1] is Integral)
            {
                return SolveResult.Solved(leftRight[0], leftRight[1], ref pEvalData);
            }

            AlgebraSolver agSolver = new AlgebraSolver();

            int startStepCount = pEvalData.WorkMgr.WorkSteps.Count;

            pEvalData.WorkMgr.FromFormatted("", "Solve for " + WorkMgr.STM + solveForFunc.ToDispString() + WorkMgr.EDM);
            WorkStep lastStep = pEvalData.WorkMgr.GetLast();

            lastStep.GoDown(ref pEvalData);
            ExComp solved = agSolver.SolveEq(solveForFunc.Var, leftRight[0].Clone().ToAlgTerm(), leftRight[1].Clone().ToAlgTerm(), ref pEvalData);
            lastStep.GoUp(ref pEvalData);

            if (solved == null)
            {
                pEvalData.WorkMgr.PopSteps(startStepCount);
                return SolveResult.Solved(leftRight[0], leftRight[1], ref pEvalData);
            }

            lastStep.WorkHtml = WorkMgr.STM + solveForFunc.ToDispString() + " = " + WorkMgr.ToDisp(solved) + WorkMgr.EDM;

            SolveResult solveResult = SolveResult.Solved(solveForFunc, solved, ref pEvalData);
            solveResult.Solutions.Insert(0, genSol);

            return solveResult;
        }
    }
}
