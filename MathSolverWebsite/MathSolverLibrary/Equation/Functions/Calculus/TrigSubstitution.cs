using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using MathSolverWebsite.MathSolverLibrary.TermType;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus
{
    class TrigSubTech
    {
        public TrigSubTech()
        {
        }

        public ExComp TrigSubstitution(ExComp[] group, AlgebraComp dVar, ref EvalData pEvalData)
        {
            AlgebraComp subVar;
            AlgebraTerm subbedResult;
            ExComp subOut;

            ExComp subIn = TrigSubstitutionGetSub(group, dVar, out subVar, out subOut, out subbedResult, ref pEvalData);
            if (subIn == null || subbedResult == null || subVar == null || subOut == null)
                return null;

            if (!subOut.IsEqualTo(dVar))
            {
                AlgebraSolver agSolver = new AlgebraSolver();
                subIn = agSolver.Solve(dVar.Var, subOut.ToAlgTerm(), subIn.ToAlgTerm(), ref pEvalData);
            }

            subbedResult = subbedResult.Substitute(dVar, subIn.Clone());
            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\int (" + WorkMgr.ToDisp(subbedResult) + ") d" + dVar.ToDispString() + WorkMgr.EDM, "Substitute " + WorkMgr.STM + dVar.ToDispString() +
                " = " + WorkMgr.ToDisp(subIn) + WorkMgr.EDM);

            pEvalData.WorkMgr.FromFormatted("");
            WorkStep lastStep = pEvalData.WorkMgr.GetLast();

            lastStep.GoDown(ref pEvalData);
            ExComp differential = Derivative.TakeDeriv(subIn.Clone(), subVar, ref pEvalData);
            lastStep.GoUp(ref pEvalData);

            lastStep.WorkHtml = WorkMgr.STM + "d" + dVar.ToDispString() + " = "
                + WorkMgr.ToDisp(differential) + "d" + subVar.ToDispString() + WorkMgr.EDM;

            ExComp trigSubbed = MulOp.StaticCombine(subbedResult, differential);
            trigSubbed = Simplifier.Simplify(trigSubbed.ToAlgTerm(), ref pEvalData);
            if (trigSubbed is AlgebraTerm)
                trigSubbed = (trigSubbed as AlgebraTerm).RemoveRedundancies();

            string trigSubbedStr = WorkMgr.ToDisp(trigSubbed);

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\int (" + trigSubbedStr + ") d" + subVar.ToDispString() + WorkMgr.EDM,
                "Substitute in " + WorkMgr.STM + "d" + dVar.ToDispString() + WorkMgr.EDM);

            pEvalData.WorkMgr.FromFormatted("", "Evaluate the integral.");
            lastStep = pEvalData.WorkMgr.GetLast();

            lastStep.GoDown(ref pEvalData);
            ExComp antiDerivResult = Integral.TakeAntiDeriv(trigSubbed, subVar, ref pEvalData);
            if (antiDerivResult is Integral)
                return null;
            lastStep.GoUp(ref pEvalData);

            lastStep.WorkHtml = WorkMgr.STM + "\\int (" + trigSubbedStr + ") d" + subVar.ToDispString() + " = " + WorkMgr.ToDisp(antiDerivResult) + WorkMgr.EDM;

            // Sub back in the appropriate variables.
            ExComp subbedBackIn = TrigSubBackIn(antiDerivResult, subVar, subIn, dVar, ref pEvalData);
            if (subbedBackIn == null)
                return null;

            return subbedBackIn;
        }

        private static ExComp TrigSubBackIn(ExComp ex, AlgebraComp subVar, ExComp subVal, AlgebraComp dVar, ref TermType.EvalData pEvalData)
        {
            int workStepStart = pEvalData.WorkMgr.WorkSteps.Count;
            pEvalData.WorkMgr.FromSides(dVar, subVal, "Solve for " + WorkMgr.STM + subVar.ToDispString() + WorkMgr.EDM);

            AlgebraTerm left = subVal.ToAlgTerm();
            AlgebraTerm right = dVar.ToAlgTerm();
            Solving.SolveMethod.ConstantsToRight(ref left, ref right, subVar, ref pEvalData);
            Solving.SolveMethod.DivideByVariableCoeffs(ref left, ref right, subVar, ref pEvalData);

            List<WorkStep> stepRange = pEvalData.WorkMgr.WorkSteps.GetRange(workStepStart, pEvalData.WorkMgr.WorkSteps.Count - workStepStart);
            pEvalData.WorkMgr.WorkSteps.RemoveRange(workStepStart, pEvalData.WorkMgr.WorkSteps.Count - workStepStart);

            // It should now just be the isolated trig function.
            ExComp leftEx = left.RemoveRedundancies();
            if (!(leftEx is TrigFunction))
                return null;
            TrigFunction trigFunc = leftEx as TrigFunction;

            ExComp hyp = null;
            ExComp opp = null;
            ExComp adj = null;

            AlgebraTerm[] numDen = right.GetNumDenFrac();
            if (numDen == null)
                numDen = new AlgebraTerm[] { right, Number.One.ToAlgTerm() };

            if (trigFunc is SinFunction)
            {
                opp = numDen[0];
                hyp = numDen[1];
            }
            else if (trigFunc is CosFunction)
            {
                adj = numDen[0];
                hyp = numDen[1];
            }
            else if (trigFunc is TanFunction)
            {
                opp = numDen[0];
                adj = numDen[1];
            }
            else if (trigFunc is CscFunction)
            {
                hyp = numDen[0];
                opp = numDen[1];
            }
            else if (trigFunc is SecFunction)
            {
                hyp = numDen[0];
                adj = numDen[1];
            }
            else if (trigFunc is CotFunction)
            {
                adj = numDen[0];
                opp = numDen[1];
            }

            // Find the last side.
            if (hyp == null)
            {
                hyp = PowOp.StaticCombine(
                    AddOp.StaticCombine(PowOp.StaticCombine(adj, new Number(2.0)), PowOp.StaticCombine(opp, new Number(2.0))),
                    AlgebraTerm.FromFraction(Number.One, new Number(2.0)));
            }
            else if (opp == null)
            {
                opp = PowOp.StaticCombine(
                    SubOp.StaticCombine(PowOp.StaticCombine(hyp, new Number(2.0)), PowOp.StaticCombine(adj, new Number(2.0))),
                    AlgebraTerm.FromFraction(Number.One, new Number(2.0)));
            }
            else if (adj == null)
            {
                adj = PowOp.StaticCombine(
                    SubOp.StaticCombine(PowOp.StaticCombine(hyp, new Number(2.0)), PowOp.StaticCombine(opp, new Number(2.0))),
                    AlgebraTerm.FromFraction(Number.One, new Number(2.0)));
            }

            List<string> defDispStrs = new List<string>();

            ExComp trigSubIn = RecursiveTrigSubIn(ex.ToAlgTerm(), hyp, opp, adj, subVar, ref defDispStrs, ref pEvalData);
            if (trigSubIn == null)
                return null;

            string summed = WorkMgr.STM;
            for (int i = 0; i < defDispStrs.Count; ++i)
            {
                summed += defDispStrs[i];
                if (i != defDispStrs.Count - 1)
                    summed += ",";
            }

            summed += WorkMgr.EDM;

            pEvalData.WorkMgr.FromSides(trigSubIn, null, "Make the substitutions " + summed);

            pEvalData.WorkMgr.WorkSteps.AddRange(stepRange);

            InverseTrigFunction itf = trigFunc.GetInverseOf();
            itf.SetSubComps(right.SubComps);

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + itf.FuncName + "(" + WorkMgr.ToDisp(trigFunc) + ") = " + WorkMgr.ToDisp(itf) + WorkMgr.EDM,
                "Take the inverse " + trigFunc.FuncName + " of both sides.");

            ExComp dVarSubIn = itf.Evaluate(false, ref pEvalData);

            pEvalData.WorkMgr.FromSides(subVar, dVarSubIn);

            // Now do the easy subs.
            trigSubIn = trigSubIn.ToAlgTerm().Substitute(subVar, dVarSubIn);

            if (trigSubIn.ToAlgTerm().Contains(subVar))
                return null;

            pEvalData.WorkMgr.FromSides(trigSubIn, null, "Substitute in " + WorkMgr.STM + subVar.ToDispString() + " = " + WorkMgr.ToDisp(dVarSubIn) + WorkMgr.EDM);

            return trigSubIn;
        }

        private static ExComp GetAppropriateSub(TrigFunction trigFunc, ExComp hyp, ExComp opp, ExComp adj)
        {
            if (trigFunc is SinFunction)
                return DivOp.StaticCombine(opp, hyp);
            else if (trigFunc is CosFunction)
                return DivOp.StaticCombine(adj, hyp);
            else if (trigFunc is TanFunction)
                return DivOp.StaticCombine(opp, adj);
            else if (trigFunc is CscFunction)
                return DivOp.StaticCombine(hyp, opp);
            else if (trigFunc is SecFunction)
                return DivOp.StaticCombine(hyp, adj);
            else if (trigFunc is CotFunction)
                return DivOp.StaticCombine(adj, opp);

            return null;
        }

        private static ExComp RecursiveTrigSubIn(AlgebraTerm term, ExComp hyp, ExComp opp, ExComp adj, AlgebraComp subInVar, ref List<string> defDispStrs, 
            ref TermType.EvalData pEvalData)
        {
            for (int i = 0; i < term.TermCount; ++i)
            {
                if (term[i] is TrigFunction && (term[i] as TrigFunction).InnerEx.IsEqualTo(subInVar))
                {
                    TrigFunction tfTerm = term[i] as TrigFunction;
                    ExComp trigSubIn = GetAppropriateSub(tfTerm, hyp, opp, adj);
                    if (trigSubIn == null)
                        return null;

                    bool contains = false;
                    foreach (string defDispStr in defDispStrs)
                    {
                        if (defDispStr.StartsWith(tfTerm.FuncName))
                        {
                            contains = true;
                            break;
                        }
                    }

                    if (!contains)
                        defDispStrs.Add(tfTerm.FuncName + "(" + subInVar.ToDispString() + ")=" + WorkMgr.ToDisp(trigSubIn));

                    term[i] = trigSubIn;
                }
                else if (term[i] is AlgebraTerm)
                {
                    term[i] = RecursiveTrigSubIn(term[i] as AlgebraTerm, hyp, opp, adj, subInVar, ref defDispStrs, ref pEvalData);
                }
            }

            return term;
        }

        private static ExComp SumTerm(List<ExComp[]> subGps, int subIndex, ExComp[] subInGp)
        {
            ExComp summedTerm = null;
            for (int j = 0; j < subGps.Count; ++j)
            {
                ExComp addTerm = (j != subIndex ? subGps[j].ToAlgTerm() : subInGp.ToAlgTerm());
                if (summedTerm == null)
                    summedTerm = addTerm;
                else
                    summedTerm = AddOp.StaticCombine(summedTerm, addTerm);
            }

            return summedTerm;
        }

        private static ExComp TrigSubstitutionGetSub(ExComp[] group, AlgebraComp dVar, out AlgebraComp subVar, out ExComp subOut, out AlgebraTerm subbedResult, ref EvalData pEvalData)
        {
            return TrigSubstitutionGetSub(group, dVar, out subVar, out subOut, out subbedResult, ref pEvalData, null, null, -1, -1);
        }

        private static void SubstituteIn(ref ExComp[] dispGp, List<ExComp[]> dispSubGps, ExComp[] group, int index, int subIndex)
        {
            if (dispGp != null && dispSubGps != null && index != -1 && subIndex != -1)
            {
                ExComp termSummed = SumTerm(dispSubGps, subIndex, group);
                if (dispGp[index] is AlgebraTerm)
                    (dispGp[index] as AlgebraTerm).SetSubComps(termSummed.ToAlgTerm().SubComps);
                else
                    dispGp[index] = termSummed;
            }
            else
                dispGp = group;
        }

        private static ExComp TrigSubstitutionGetSub(ExComp[] group, AlgebraComp dVar, out AlgebraComp subVar, out ExComp subOut, out AlgebraTerm subbedResult, ref EvalData pEvalData, 
            ExComp[] dispGp, List<ExComp[]> dispSubGps, int index, int subIndex)
        {
            string subVarStr = "$theta";
            subVar = new AlgebraComp(subVarStr);
            subbedResult = null;
            subOut = null;

            for (int i = 0; i < group.Length; ++i)
            {
                if (group[i] is AlgebraTerm)
                {
                    // Apply on all sub levels.
                    AlgebraTerm subTerm = group[i] as AlgebraTerm;

                    List<ExComp[]> subGps = subTerm.GetGroupsNoOps();
                    if (subGps.Count == 1 && subGps[0].Length == 1 && subGps[0][0] == subTerm)
                    {
                        // This is something that returns itself as a group.
                        subGps = (new AlgebraTerm(subTerm.SubComps.ToArray())).GetGroupsNoOps();
                    }
                    ExComp subSubbedResult = null;
                    int subSubbedIndex = -1;
                    for (int j = 0; j < subGps.Count; ++j)
                    {
                        subSubbedResult = TrigSubstitutionGetSub(subGps[j], dVar, out subVar, out subOut, out subbedResult, ref pEvalData, group.CloneGroup(), 
                            subGps.CloneGpList(), i, j);
                        if (subSubbedResult != null)
                        {
                            subSubbedIndex = j;
                            break;
                        }
                    }

                    if (subSubbedIndex != -1)
                    {
                        ExComp summedTerm = null;
                        for (int j = 0; j < subGps.Count; ++j)
                        {
                            ExComp addTerm = (j == subSubbedIndex ? subGps[j].ToAlgTerm() : subbedResult);
                            if (summedTerm == null)
                                summedTerm = addTerm;
                            else
                                summedTerm = AddOp.StaticCombine(summedTerm, addTerm);
                        }

                        (group[i] as AlgebraTerm).SetSubComps(summedTerm.ToAlgTerm().SubComps);

                        subbedResult = group.ToAlgTerm();
                        return subSubbedResult;
                    }
                }
                // Is this a sqrt function?
                if (!(group[i] is PowerFunction))
                {
                    continue;
                }

                PowerFunction powFunc = group[i] as PowerFunction;
                if (!(powFunc.Power is AlgebraTerm))
                    continue;

                AlgebraTerm powTerm = powFunc.Power as AlgebraTerm;
                AlgebraTerm[] numDen = powTerm.GetNumDenFrac();
                if (numDen == null)
                    continue;
                if (!numDen[1].RemoveRedundancies().IsEqualTo(new Number(2.0)))
                    continue;

                if (!Number.One.IsEqualTo(numDen[0].RemoveRedundancies()))
                {
                    PowerFunction nestedPf = new PowerFunction(new PowerFunction(powFunc.Base, AlgebraTerm.FromFraction(Number.One, new Number(2.0))), numDen[0]);

                    // Go back over this element.
                    group[i] = nestedPf;
                    i--;
                    continue;
                }

                AlgebraTerm baseTerm = (group[i] as PowerFunction).Base.ToAlgTerm();

                // Is it x^2?
                List<ExComp> basePows = baseTerm.GetPowersOfVar(dVar);
                if (basePows.Count != 1)
                    continue;
                ExComp singlePow = basePows[0];
                if (!(singlePow is Number) && (singlePow as Number) == 2.0)
                    continue;

                subOut = dVar;

                // Find the b value.
                List<AlgebraGroup> varGroups = baseTerm.GetGroupsVariableToNoOps(dVar);
                if (varGroups.Count != 1)
                    continue;

                AlgebraTerm bSqTerm = AlgebraGroup.GetConstantTo(varGroups, dVar);
                ExComp bSq = bSqTerm.RemoveRedundancies();

                // Find the a value.
                List<AlgebraGroup> constGroups = baseTerm.GetGroupsConstantTo(dVar);
                if (constGroups.Count != 1)
                    continue;

                AlgebraTerm aSqTerm = AlgebraGroup.ToTerm(constGroups);
                ExComp aSq = aSqTerm.RemoveRedundancies();

                Number aCoeff = null;
                if (bSq is Number)
                    aCoeff = aSq as Number;
                else if (aSq is AlgebraTerm)
                {
                    ExComp[] aValGp = varGroups[0].Group.GetUnrelatableTermsOfGroup(dVar);
                    aCoeff = aValGp.GetCoeff();
                }

                if (aCoeff == null)
                    return null;

                Number bCoeff = null;
                if (bSq is Number)
                    bCoeff = bSq as Number;
                else if (bSq is AlgebraTerm)
                {
                    bCoeff = constGroups[0].Group.GetCoeff();
                }

                if (bCoeff == null)
                    return null;

                if (aCoeff.HasImaginaryComp() || bCoeff.HasImaginaryComp())
                    return null;

                bool aNeg = aCoeff < 0.0;
                bool bNeg = bCoeff < 0.0;

                if (aNeg)
                    aSq = (new AbsValFunction(aSq)).Evaluate(false, ref pEvalData);

                if (aSq is AbsValFunction)
                    return null;

                if (bNeg)
                    bSq = (new AbsValFunction(bSq)).Evaluate(false, ref pEvalData);

                if (bSq is AbsValFunction)
                    return null;

                ExComp usedTrigFunc = null;
                if (!aNeg && bNeg)
                    usedTrigFunc = new SinFunction(new AlgebraComp(subVarStr));
                else if (aNeg && !bNeg)
                    usedTrigFunc = new SecFunction(new AlgebraComp(subVarStr));
                else if (!aNeg && !bNeg)
                    usedTrigFunc = new TanFunction(new AlgebraComp(subVarStr));

                if (usedTrigFunc == null)
                    return null;

                ExComp aVal = PowOp.StaticCombine(aSq, AlgebraTerm.FromFraction(Number.One, new Number(2.0)));
                ExComp bVal = PowOp.StaticCombine(bSq, AlgebraTerm.FromFraction(Number.One, new Number(2.0)));

                if (aVal is AlgebraTerm)
                    aVal = (aVal as AlgebraTerm).RemoveRedundancies();
                if (bVal is AlgebraTerm)
                    bVal = (bVal as AlgebraTerm).RemoveRedundancies();

                ExComp subIn = MulOp.StaticCombine(DivOp.StaticCombine(aVal, bVal), usedTrigFunc);

                // Replace the value in the group itself.
                ExComp baseTermSubbed = AddOp.StaticCombine(new AlgebraTerm(bSq, new MulOp(), PowOp.StaticWeakCombine(subIn, new Number(2.0))), aSq);
                ExComp pfPow = (group[i] as PowerFunction).Power;

                group[i] = new PowerFunction(baseTermSubbed, pfPow);
                SubstituteIn(ref dispGp, dispSubGps, group, index, subIndex);

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\int (" + WorkMgr.ToDisp(dispGp.ToAlgTerm()) + ") d" + dVar.ToDispString() + WorkMgr.EDM, "Substitute " + WorkMgr.STM + subVar.ToDispString() +
                    "=" + WorkMgr.ToDisp(subIn) + WorkMgr.EDM);

                AlgebraTerm dispIdenStep = new AlgebraTerm();
                AlgebraTerm useTerm = dispIdenStep;
                if (!aSqTerm.IsOne())
                {
                    useTerm = new AlgebraTerm();
                    dispIdenStep.Add(aSqTerm, new MulOp(), useTerm);
                }

                ExComp simpTo = null;

                if (usedTrigFunc is SinFunction)
                {
                    useTerm.Add(Number.One, new SubOp(), PowOp.StaticWeakCombine(usedTrigFunc, new Number(2.0)));
                    simpTo = new CosFunction((usedTrigFunc as AppliedFunction).InnerTerm);
                }
                else if (usedTrigFunc is SecFunction)
                {
                    useTerm.Add(PowOp.StaticWeakCombine(usedTrigFunc, new Number(2.0)), new SubOp(), Number.One);
                    simpTo = new TanFunction((usedTrigFunc as AppliedFunction).InnerTerm);
                }
                else if (usedTrigFunc is TanFunction)
                {
                    useTerm.Add(PowOp.StaticWeakCombine(usedTrigFunc, new Number(2.0)), new AddOp(), Number.One);
                    simpTo = new SecFunction((usedTrigFunc as AppliedFunction).InnerTerm);
                }
                else
                    return null;

                group[i] = new PowerFunction(dispIdenStep, pfPow);
                SubstituteIn(ref dispGp, dispSubGps, group, index, subIndex);

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\int (" + WorkMgr.ToDisp(dispGp.ToAlgTerm()) + ")d" + dVar.ToDispString() + WorkMgr.EDM,
                    "Simplify.");

                dispIdenStep = new AlgebraTerm();
                if (aVal is Number && (aVal as Number) != 1.0)
                {
                    dispIdenStep.Add(aVal, new MulOp());
                }

                dispIdenStep.Add(new PowerFunction(PowOp.StaticWeakCombine(simpTo, new Number(2.0)), pfPow));

                group[i] = dispIdenStep;
                SubstituteIn(ref dispGp, dispSubGps, group, index, subIndex);

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\int (" + WorkMgr.ToDisp(dispGp.ToAlgTerm()) + ") d" + dVar.ToDispString() + WorkMgr.EDM, "Use the trig identity " +
                    WorkMgr.STM + WorkMgr.ToDisp(PowOp.StaticWeakCombine(simpTo, new Number(2.0))) + " = " + useTerm.FinalToDispStr() + WorkMgr.EDM);

                group[i] = MulOp.StaticCombine(aVal, simpTo);
                SubstituteIn(ref dispGp, dispSubGps, group, index, subIndex);

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\int (" + WorkMgr.ToDisp(dispGp.ToAlgTerm()) + ") d" + dVar.ToDispString() + WorkMgr.EDM);

                subbedResult = group.ToAlgTerm();

                return subIn;
            }

            return null;
        }
    }
}