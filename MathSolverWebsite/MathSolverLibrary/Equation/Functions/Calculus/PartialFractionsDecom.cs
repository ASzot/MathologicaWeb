using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using MathSolverWebsite.MathSolverLibrary.Equation.Structural.Polynomial;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus
{
    class PartialFractionsDecom
    {
        public static AlgebraTerm[][] Decompose(AlgebraTerm num, AlgebraTerm den, PolynomialExt numPoly, AlgebraComp dVar, ref TermType.EvalData pEvalData)
        {
            //// Be sure that something like (x-2)(x^2-1) already in factored form works.
            //AlgebraTerm[] factors = den.GetFactors(ref pEvalData);

            //if (factors == null || factors.Length == 0)
            //    return null;

            //List<AlgebraTerm[]> decomNumDens = new List<AlgebraTerm[]>();

            //int startAlphaChar = (int)'A';
            //for (int i = 0; i < factors.Length; ++i)
            //{
            //    ExComp factor = factors[i].RemoveRedundancies();
            //    ExComp useEx;
            //    if (factor is PowerFunction)
            //        useEx = (factor as PowerFunction).Base;
            //    else
            //        useEx = factor;

            //    int maxPow = 1;
            //    if (factor is AlgebraTerm)
            //    {
            //        PolynomialExt polyFactor = new PolynomialExt();
            //        if (polyFactor.Init(factor as AlgebraTerm))
            //            maxPow = polyFactor.MaxPow;
            //    }

            //    for (int j = 0; j < maxPow; ++j)
            //    {
            //        AlgebraTerm decomNum = PolynomialGen.GenGenericOfDegree(maxPow - 1, dVar, startAlphaChar);
            //        startAlphaChar += maxPow;

            //        decomNumDens.Add(new AlgebraTerm[] 
            //        { 
            //            decomNum, 
            //            j == 1 ? useEx.ToAlgTerm() : Operators.PowOp.StaticWeakCombine(useEx, new Number(j + 1)).ToAlgTerm()
            //        });
            //    }
            //}

            //// Multiply the numerators by every single other denominator that isn't that numerator.
            //ExComp finalEx = null;
            //for (int i = 0; i < decomNumDens.Count; ++i)
            //{
            //    AlgebraTerm[] decomNumDen = decomNumDens[i];

            //    ExComp combined = decomNumDen[0];
            //    for (int j = 0; j < decomNumDens.Count; ++j)
            //    {
            //        if (j == i)
            //            continue;
            //        combined = Operators.MulOp.StaticCombine(combined, decomNumDens[j][1]);
            //    }

            //    if (finalEx == null)
            //        finalEx = combined;
            //    else
            //        finalEx = Operators.AddOp.StaticCombine(finalEx, combined);
            //}

            //AlgebraTerm finalTerm = finalEx.ToAlgTerm();

            //List<ExComp> decomCoeffs = new List<ExComp>();
            //for (int i = 0; i < 
            //{
            //    var decomVarGroups = finalTerm.GetGroupContainingTerm(decomVar);
            //    var decomVarTerms = from decomVarGroup in decomVarGroups
            //                        select decomVarGroup.GetUnrelatableTermsOfGroup(decomVar).ToAlgTerm();

            //    AlgebraTerm decomCoeff = new AlgebraTerm();
            //    foreach (AlgebraTerm aTerm in decomVarTerms)
            //    {
            //        decomCoeff = decomCoeff + aTerm;
            //    }

            //    decomCoeffs.Add(decomCoeff);
            //}

            //if (decomCoeffs.Count != numPoly.MaxPow)
            //    return null;

            //// Solve the system of equations for the decomposition coefficients.
            //List<EquationSet> equations = new List<EquationSet>();
            //for (int i = 0; i < decomCoeffs.Count; ++i)
            //{
            //    numPoly.Info.GetCoeffForPow(i);
            //}

            return null;
        }
    }
}