﻿using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using System;
using System.Collections.Generic;


namespace MathSolverWebsite.MathSolverLibrary.Solving.Diff_Eqs
{
    class HomogeneousSolve : DiffSolve
    {
        public override ExComp[] Solve(AlgebraTerm left, AlgebraTerm right, AlgebraComp funcVar, AlgebraComp dVar,
            ref TermType.EvalData pEvalData)
        {
            // In the form dy/dx=f(y/x)

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
                return null;

            List<ExComp> xNumPowers = numDen[0].GetPowersOfVar(dVar);
            List<ExComp> yNumPowers = numDen[0].GetPowersOfVar(funcVar);
            List<ExComp> xDenPowers = numDen[1].GetPowersOfVar(dVar);
            List<ExComp> yDenPowers = numDen[1].GetPowersOfVar(funcVar);

            foreach (ExComp xDenPower in xDenPowers)
            {
                if (!xNumPowers.ContainsEx(xDenPower))
                    xNumPowers.Add(xDenPower);
            }

            foreach (ExComp yDenPower in yDenPowers)
            {
                if (!yNumPowers.ContainsEx(yDenPower))
                    yNumPowers.Add(yDenPower);
            }

            // Ensure all powers are real integers greater than or equal to 1.
            List<int> ixPows = new List<int>();
            List<int> iyPows = new List<int>();

            foreach (ExComp xPow in xNumPowers)
            {
                if (!(xPow is Number) || !(xPow as Number).IsRealInteger())
                    return null;

                int iPow = (int)(xPow as Number).RealComp;
                if (iPow < 1.0)
                    return null;

                ixPows.Add(iPow);
            }

            foreach (ExComp yPow in yNumPowers)
            {
                if (!(yPow is Number) || !(yPow as Number).IsRealInteger())
                    return null;

                int iPow = (int)(yPow as Number).RealComp;
                if (iPow < 1.0)
                    return null;

                iyPows.Add(iPow);
            }

            // Ensure the max pows are equal.
            int xPowMax = -1;
            int yPowMax = -1;

            foreach (int xPow in ixPows)
            {
                if (xPow > xPowMax)
                    xPowMax = xPow;
            }

            foreach (int yPow in iyPows)
            {
                if (yPow > yPowMax)
                    yPowMax = yPow;
            }

            if (xPowMax != yPowMax)
                return null;

            // Divide each group by the maximum power of x.
            ExComp xPf = xPowMax == 1 ? dVar : (ExComp)dVar.ToPow((double)xPowMax);

            numDen[0] = numDen[0].RemoveRedundancies().ToAlgTerm();
            numDen[1] = numDen[1].RemoveRedundancies().ToAlgTerm();

            numDen[0] = Limit.ComponentWiseDiv(numDen[0], xPf, dVar).ToAlgTerm();
            numDen[1] = Limit.ComponentWiseDiv(numDen[1], xPf, dVar).ToAlgTerm();

            pEvalData.WorkMgr.FromSides(left, AlgebraTerm.FromFraction(numDen[0], numDen[1]), "Divide the numerator and denominator by " + WorkMgr.STM + WorkMgr.ToDisp(xPf) + WorkMgr.EDM);

            // Make the substitution 'y/x -> v'
            AlgebraComp subVar = new AlgebraComp("$v");

            numDen[0] = MakeVSub(numDen[0], funcVar, dVar, subVar);
            if (numDen[0] == null || numDen[0].Contains(dVar) || numDen[0].Contains(funcVar))
                return null;

            numDen[1] = MakeVSub(numDen[1], funcVar, dVar, subVar);
            if (numDen[1] == null || numDen[1].Contains(dVar) || numDen[1].Contains(funcVar))
                return null;

            left = AddOp.StaticCombine(subVar, MulOp.StaticCombine(dVar, Derivative.ConstructDeriv(dVar, subVar))).ToAlgTerm();
            right = DivOp.StaticCombine(numDen[0], numDen[1]).ToAlgTerm();

            pEvalData.WorkMgr.FromSides(left, right, "Make the substitution " + WorkMgr.STM + "v=\\frac{d" + funcVar.ToDispString() + "}{d" + dVar.ToDispString() + "}" + WorkMgr.EDM);

            ExComp[] solved = (new SeperableSolve()).Solve(left, right, subVar, dVar, ref pEvalData);
            if (solved == null)
                return null;

            if (solved[0] is Integral || solved[1] is Integral)
                return solved;

            pEvalData.WorkMgr.FromSides(solved[0], solved[1]);

            // Substitute back in.
            for (int i = 0; i < solved.Length; ++i)
            {
                AlgebraTerm subbed = solved[i].ToAlgTerm().Substitute(subVar, AlgebraTerm.FromFraction(funcVar, dVar));
                subbed = subbed.ApplyOrderOfOperations();
                solved[i] = subbed.MakeWorkable();
            }

            pEvalData.WorkMgr.FromSides(solved[0], solved[1], "Substitute back in " + WorkMgr.STM + subVar.ToDispString() + "=\\frac{" + funcVar.ToDispString() + "}{" + dVar.ToDispString() + "}" + WorkMgr.EDM);

            return solved;
        }

        private static AlgebraTerm MakeVSub(AlgebraTerm term, AlgebraComp funcVar, AlgebraComp dVar, AlgebraComp subInVar)
        {
            //if (term is AppliedFunction)
            //{
            //    AppliedFunction af = term as AppliedFunction;
            //    AlgebraTerm innerTerm = MakeVSub(af.InnerTerm, funcVar, dVar, subInVar);

            //    af.SetSubComps(innerTerm.SubComps);
            //    return af;
            //}
            //else if (term is PowerFunction)
            //{
            //    PowerFunction pf = term as PowerFunction;
            //    AlgebraTerm baseEx = MakeVSub(pf.Base.ToAlgTerm(), funcVar, dVar, subInVar);
            //    AlgebraTerm powEx = MakeVSub(pf.Power.ToAlgTerm(), funcVar, dVar, subInVar);

            //    return new PowerFunction(baseEx, powEx);
            //}

            List<ExComp[]> gps = term.GetGroupsNoOps();
            if (gps.Count == 1 && gps[0].Length == 1 && gps[0][0] == term)
                return term;

            for (int i = 0; i < gps.Count; ++i)
            {
                ExComp[] gp = gps[i];
                for (int j = 0; j < gp.Length; ++j)
                {
                    if (gp[j] is AlgebraTerm)
                        gp[j] = MakeVSub(gp[j] as AlgebraTerm, funcVar, dVar, subInVar);
                }

                ExComp[] num = gp.GetNumerator();
                ExComp[] den = gp.GetDenominator();

                if (den.Length == 0)
                    continue;

                // Check for y^n in the numerator.
                int[] numPowAndIndex = SearchPowIndex(num, funcVar);
                if (numPowAndIndex == null)
                    continue;
                int[] denPowAndIndex = SearchPowIndex(den, dVar);
                if (denPowAndIndex == null)
                    continue;

                int powDiff = numPowAndIndex[0] - denPowAndIndex[0];
                if (powDiff < 0)
                    return null;
                powDiff = Math.Abs(powDiff);

                ExComp vSubIn = PowOp.StaticCombine(subInVar, new Number(Math.Min(numPowAndIndex[0], denPowAndIndex[0])));

                if (powDiff != 0)
                {
                    num[numPowAndIndex[1]] = PowOp.StaticCombine(funcVar, new Number(powDiff));
                    ExComp[] tmpNum = new ExComp[num.Length + 1];
                    for (int j = 0; j < tmpNum.Length; ++j)
                        tmpNum[j] = num[j];
                    tmpNum[tmpNum.Length - 1] = vSubIn;

                    num = tmpNum;
                }
                else
                    num[numPowAndIndex[1]] = vSubIn;

                den = den.RemoveEx(denPowAndIndex[1]);


                gps[i] = new ExComp[num.Length + (den.Length == 0 ? 0 : 1)];
                for (int j = 0; j < num.Length; ++j)
                {
                    gps[i][j] = num[j];
                }
                
                if (den.Length != 0)
                    gps[i][gps[i].Length - 1] = new PowerFunction(den.ToAlgTerm(), Number.NegOne);
            }

            return new AlgebraTerm(gps.ToArray());
        }

        private static int[] SearchPowIndex(ExComp[] gp, AlgebraComp searchVar)
        {
            int pow = -1;
            int index = -1;
            for (int j = 0; j < gp.Length; ++j)
            {
                if (gp[j] is AlgebraComp && gp[j].IsEqualTo(searchVar))
                {
                    index = j;
                    pow = 1;
                }
                else if (gp[j] is PowerFunction)
                {
                    ExComp exBase = (gp[j] as PowerFunction).Base;
                    if (exBase is AlgebraTerm)
                        exBase = (exBase as AlgebraTerm).RemoveRedundancies();

                    if (!exBase.IsEqualTo(searchVar))
                        continue;

                    ExComp power = (gp[j] as PowerFunction).Power;
                    if (!(power is Number) || !(power as Number).IsRealInteger())
                        continue;

                    pow = (int)(power as Number).RealComp;
                    if (pow < 1.0)
                        continue;

                    index = j;
                }
            }

            if (index == -1)
                return null;

            return new int[] { pow, index };
        }
    }
}