﻿using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathSolverWebsite.MathSolverLibrary.Solving.Diff_Eqs
{
    class SeperableSolve : DiffSolve
    {
        private static AlgebraTerm FromFractionFix(ExComp num, ExComp den)
        {
            ExComp finalNum = Number.One;
            ExComp finalDen = Number.One;

            if (num is AlgebraTerm)
            {
                AlgebraTerm[] numNumDen = (num as AlgebraTerm).GetNumDenFrac();
                if (numNumDen != null)
                {
                    finalNum = MulOp.StaticCombine(finalNum, numNumDen[0]);
                    finalDen = MulOp.StaticCombine(finalDen, numNumDen[1]);
                }
                else
                    finalNum = num;
            }

            if (den is AlgebraTerm)
            {
                AlgebraTerm[] denNumDen = (den as AlgebraTerm).GetNumDenFrac();
                if (denNumDen != null)
                {
                    finalNum = MulOp.StaticCombine(finalNum, denNumDen[1]);
                    finalDen = MulOp.StaticCombine(finalDen, denNumDen[0]);
                }
                else
                    finalDen = den;
            }

            return AlgebraTerm.FromFraction(finalNum, finalDen);
        }

        public override ExComp[] Solve(AlgebraTerm left, AlgebraTerm right, AlgebraComp funcVar, AlgebraComp dVar,
            ref TermType.EvalData pEvalData)
        {
            // In the form N(x)dy/dx = M(x)

            // As the order is one convert to a variable representation of the derivatives.
            AlgebraComp derivVar = null;
            left = ConvertDerivsToAlgebraComps(left, funcVar, dVar, ref derivVar);
            right = ConvertDerivsToAlgebraComps(right, funcVar, dVar, ref derivVar);

            pEvalData.WorkMgr.FromSides(left, right, "This is a seperable differential equation.");

            // Move the dy/dx to the left.
            SolveMethod.VariablesToLeft(ref left, ref right, derivVar, ref pEvalData);

            // Move everything else to the right.
            SolveMethod.ConstantsToRight(ref left, ref right, derivVar, ref pEvalData);

            // Combine the fractions.
            SolveMethod.CombineFractions(ref left, ref right, ref pEvalData);

            AlgebraTerm[] numDen = null;

            if ((left.Contains(funcVar) && !left.Contains(dVar) && right.Contains(dVar) && !right.Contains(funcVar)) || 
                (left.Contains(dVar) && !left.Contains(funcVar) && right.Contains(funcVar) && !right.Contains(dVar)))
            {
                var groups = left.GetGroupsNoOps();
                var unrelatedGroups = from gp in groups
                                      select gp.GetUnrelatableTermsOfGroup(derivVar);

                // Combine all of the unrelated terms.
                AlgebraTerm leftDivideOut = new AlgebraTerm(unrelatedGroups.ToArray());

                if (leftDivideOut.GroupCount > 1)
                {
                    leftDivideOut = leftDivideOut.ApplyOrderOfOperations();
                    leftDivideOut = leftDivideOut.MakeWorkable().ToAlgTerm();
                }

                if (!leftDivideOut.IsZero() && !leftDivideOut.IsOne())
                {
                    numDen = new AlgebraTerm[] { right, leftDivideOut };
                }
            }
            else
            {
                SolveMethod.DivideByVariableCoeffs(ref left, ref right, derivVar, ref pEvalData);
            }

            if (numDen == null)
                numDen = right.GetNumDenFrac();

            if (numDen == null)
            {
                numDen = new AlgebraTerm[] { right, Number.One.ToAlgTerm() };
            }

            // The 'dx' on the left is implied.
            // The 'dy' on the right is implied.

            if (!numDen[1].Contains(dVar) && !numDen[0].Contains(funcVar))
            {
                // Just cross multiply.
                left = numDen[1];
                right = numDen[0];
            }
            else if (!numDen[1].Contains(funcVar) && !numDen[0].Contains(dVar))
            {
                left = FromFractionFix(Number.One, numDen[0]);
                right = FromFractionFix(Number.One, numDen[1]);
            }
            else if (!numDen[0].Contains(funcVar) && !numDen[1].Contains(funcVar))
            {
                left = Number.One.ToAlgTerm();
                right = FromFractionFix(numDen[0], numDen[1]);
            }
            else if (!numDen[0].Contains(dVar) && !numDen[1].Contains(dVar))
            {
                left = FromFractionFix(numDen[1], numDen[2]);
                right = Number.One.ToAlgTerm();
            }
            else
            {
                int gpCount = left.GroupCount;
                List<AlgebraGroup> yVarTo = right.GetGroupsVariableToNoOps(funcVar);
                List<AlgebraGroup> xVarTo = right.GetGroupsVariableToNoOps(dVar);

                if (yVarTo.Count == gpCount)
                {
                    AlgebraTerm divTerm = AlgebraGroup.GetConstantTo(yVarTo, funcVar);

                    left = AlgebraTerm.FromFraction(Number.One, DivOp.StaticCombine(right, divTerm));
                    if (left.Contains(dVar))
                        return null;
                    right = divTerm;
                }
                else if (xVarTo.Count == gpCount)
                {
                    AlgebraTerm divTerm = AlgebraGroup.GetConstantTo(xVarTo, dVar);

                    left = AlgebraTerm.FromFraction(Number.One, divTerm);
                    right = DivOp.StaticCombine(right, divTerm).ToAlgTerm();
                    if (right.Contains(funcVar))
                        return null;
                }
                else
                    return null;
            }

            string leftStr = left.FinalToDispStr();
            string rightStr = right.FinalToDispStr();

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "(" + leftStr + ")d" + funcVar.ToDispString() +
                "= (" + rightStr + ")d" + dVar.ToDispString() + WorkMgr.EDM, "Cross multiply.");

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\int (" + leftStr + ")d" + funcVar.ToDispString() +
                "= \\int (" + rightStr + ")d" + dVar.ToDispString() + WorkMgr.EDM, "Take the anti-derivative of both sides.");

            // Integrate both sides.
            pEvalData.WorkMgr.FromFormatted("", "Integrate.");
            WorkStep lastWorkStep = pEvalData.WorkMgr.GetLast();

            lastWorkStep.GoDown(ref pEvalData);
            ExComp leftIntegrated = Integral.TakeAntiDeriv(left, funcVar, ref pEvalData);
            lastWorkStep.GoUp(ref pEvalData);

            lastWorkStep.WorkHtml = WorkMgr.STM + "\\int (" + leftStr + ")d" + funcVar.ToDispString() + "=" + WorkMgr.ToDisp(leftIntegrated) + WorkMgr.EDM;

            pEvalData.WorkMgr.FromFormatted("", "Integrate.");
            lastWorkStep = pEvalData.WorkMgr.GetLast();

            lastWorkStep.GoDown(ref pEvalData);
            ExComp rightIntegrated = Integral.TakeAntiDeriv(right, dVar, ref pEvalData);
            lastWorkStep.GoUp(ref pEvalData);

            lastWorkStep.WorkHtml = WorkMgr.STM + "\\int (" + rightStr + ")d" + dVar.ToDispString() + "=" + WorkMgr.ToDisp(rightIntegrated) + WorkMgr.EDM;

            return new ExComp[] { leftIntegrated, rightIntegrated };
        }
    }
}