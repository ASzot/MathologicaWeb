using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using System.Linq;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Solving
{
    internal class FactorSolve : SolveMethod
    {
        private AlgebraTerm _overallTerm = null;
        private AlgebraSolver p_agSolver;

        public FactorSolve(AlgebraSolver pAgSolver)
        {
            p_agSolver = pAgSolver;
        }

        public FactorSolve(AlgebraSolver pAgSolver, AlgebraTerm overallTerm)
        {
            p_agSolver = pAgSolver;
            _overallTerm = overallTerm;
        }

        public override ExComp SolveEquation(AlgebraTerm left, AlgebraTerm right, AlgebraVar solveFor, ref TermType.EvalData pEvalData)
        {
            AlgebraComp solveForComp = solveFor.ToAlgebraComp();
            AlgebraTerm nonZeroTerm = left.IsZero() ? right : left;
            AlgebraTerm zero = new AlgebraTerm(Number.GetZero());

            if (_overallTerm != null)
            {
                // We have a simple factor solve.
                nonZeroTerm = _overallTerm.SimpleFactor();
            }
            else
            {
                nonZeroTerm = left.IsZero() ? right : left;
            }

            DivideByVariableCoeffs(ref nonZeroTerm, ref zero, solveForComp, ref pEvalData);

            List<ExComp[]> groups = nonZeroTerm.GetGroupsNoOps();

            p_agSolver.ClearLinearSolveRepeatCount();
            if (groups.Count != 1)
                return p_agSolver.Solve(solveFor, left, right, ref pEvalData);

            ExComp[] onlyGroup = nonZeroTerm.GetGroupsNoOps()[0];
            onlyGroup = onlyGroup.RemoveOneCoeffs();

            // The factors are the algebra terms of this groups.
            AlgebraTerm[] factors = (from onlyGroupComp in onlyGroup
                                     select onlyGroupComp.ToAlgTerm().RemoveRedundancies().ToAlgTerm()).ToArray();

            return SolveEquationFactors(solveFor, ref pEvalData, factors);
        }

        public ExComp SolveEquationFactors(AlgebraVar solveFor, ref TermType.EvalData pEvalData, params AlgebraTerm[] factors)
        {
            AlgebraTermArray factorTermArray = new AlgebraTermArray(factors);
            AlgebraTermArray solutions = new AlgebraTermArray();
            foreach (AlgebraTerm factor in factors)
            {
                int mulplicity = 1;
                AlgebraTerm leftSolve = factor;
                pEvalData.GetWorkMgr().FromSides(leftSolve, Number.GetZero(), "Solve when one of the factors equals zero.");
                if (factor is PowerFunction)
                {
                    PowerFunction pfFactor = factor as PowerFunction;
                    if (pfFactor.GetPower() is Number && (pfFactor.GetPower() as Number).IsRealInteger())
                    {
                        mulplicity = (int)(pfFactor.GetPower() as Number).GetRealComp();
                        pEvalData.GetWorkMgr().FromFormatted(WorkMgr.STM + "{1}={2}" + WorkMgr.EDM, "Since the factor has a power of " + WorkMgr.STM + "{0}" + WorkMgr.EDM + " it also has a mulplicity of " +
                            WorkMgr.STM + "{0}" + WorkMgr.EDM +
                            " Just ignore the power when solving as it determines the multiplicity not the solution.", mulplicity, leftSolve, Number.GetZero());
                        leftSolve = pfFactor.GetBase().ToAlgTerm();
                    }
                }

                p_agSolver.ClearLinearSolveRepeatCount();
                ExComp solved = p_agSolver.SolveEq(solveFor, leftSolve, Number.GetZero().ToAlgTerm(), ref pEvalData, true);
                for (int i = 0; i < mulplicity; ++i)
                {
                    solutions.Add(solved);
                }
            }

            return solutions;
        }
    }
}