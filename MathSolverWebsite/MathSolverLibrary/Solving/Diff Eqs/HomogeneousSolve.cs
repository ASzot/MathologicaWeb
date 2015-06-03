using MathSolverWebsite.MathSolverLibrary.Equation;
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
        public static ExComp[] Solve(AlgebraTerm left, AlgebraTerm right, AlgebraComp funcVar, AlgebraComp dVar,
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

            List<ExComp> xPowers = right.GetPowersOfVar(dVar);
            List<ExComp> yPowers = right.GetPowersOfVar(funcVar);

            // Ensure all powers are real integers greater than or equal to 1.
            List<int> ixPows = new List<int>();
            List<int> iyPows = new List<int>();

            foreach (ExComp xPow in xPowers)
            {
                if (!(xPow is Number) || !(xPow as Number).IsRealInteger())
                    return null;

                int iPow = (int)(xPow as Number).RealComp;
                if (iPow < 1.0)
                    return null;

                ixPows.Add(iPow);
            }

            foreach (ExComp yPow in yPowers)
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
            PowerFunction xPf = dVar.ToPow((double)xPowMax);

            numDen[0] = Limit.ComponentWiseDiv(numDen[0], xPf, dVar).ToAlgTerm();
            numDen[1] = Limit.ComponentWiseDiv(numDen[1], xPf, dVar).ToAlgTerm();

            // Make the substitution 'y/x -> v'
            AlgebraComp subVar = new AlgebraComp("$v");

        }

        private static AlgebraTerm MakeVSub(AlgebraTerm term, AlgebraComp funcVar, AlgebraComp dVar, AlgebraComp subInVar)
        {
            List<ExComp[]> gps = term.GetGroupsNoOps();
            for (int i = 0; i < gps.Count; ++i)
            {
                ExComp[] gp = gps[i];

                ExComp[] num = gp.GetNumerator();
                ExComp[] den = gp.GetDenominator();

                // Check for y^n in the numerator.
                int[] numPowAndIndex = SearchPowIndex(num, funcVar);
                if (numPowAndIndex == null)
                    return null;
                int[] denPowAndIndex = SearchPowIndex(den, dVar);
                if (denPowAndIndex == null)
                    return null;

                int powDiff = numPowAndIndex[0] - denPowAndIndex[0];
                if (powDiff < 0)
                    return null;
                powDiff = Math.Abs(powDiff);

                ExComp vSubIn = PowOp.StaticCombine(subInVar, new Number(Math.Max(numPowAndIndex[0], denPowAndIndex[0])));

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
                gps[i] = 
            }
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