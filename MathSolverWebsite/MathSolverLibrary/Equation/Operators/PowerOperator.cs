using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg;
using System;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Operators
{
    internal class PowOp : AgOp
    {
        private const int MAX_BINOM_COMPLEXITY = 20;
        private const int MAX_COMPLEXITY = 1000;
        private const int MIN_BINOM_COMPLEXITY = 3;
        private const int MAX_COMBINE_COUNT = 10;

        /// <summary>
        /// Raises 'e' to the given power.
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public static ExComp Exp(ExComp power)
        {
            return new PowerFunction(Constant.GetE(), power);
        }

        public static ExComp RaiseNumToNum(ExComp ex1, ExComp ex2)
        {
            Number n1 = ex1 as Number;
            Number n2 = ex2 as Number;

            if (n1 == null || n2 == null)
                return PowOp.StaticWeakCombine(ex1, ex2);

            if (!n1.HasImaginaryComp() && !n2.HasImaginaryComp())
            {
                double dBase = n1.GetRealComp();
                double dPow = n2.GetRealComp();

                bool imag = false;

                // Given odd is false, even is not necessarily true as there can be decimals.
                bool isOdd = false;
                if (dBase < 0.0)
                {
                    double root = 1.0 / dPow;
                    if (DoubleHelper.IsInteger(root))
                    {
                        int rootInt = (int)root;
                        isOdd = rootInt % 2 != 0;

                        if (rootInt == 2)
                        {
                            // Even root. We got an imaginary number.
                            imag = true;
                            dBase *= -1.0;
                        }
                    }
                }

                bool negPow = false;
                if (dPow < 0.0)
                {
                    dPow *= -1.0;
                    negPow = true;
                }

                double result;
                if (isOdd)
                    result = Math.Pow(Math.Abs(dBase), dPow);
                else
                    result = Math.Pow(dBase, dPow);

                bool isResultInt = DoubleHelper.IsInteger(result);

                if (isResultInt && isOdd && dBase < 0.0)
                    result *= -1.0;
                else if (!isResultInt)
                    result = Math.Pow(dBase, dPow);

                ExComp resultEx = null;
                if (imag)
                {
                    if (isResultInt)
                    {
                        resultEx = new Number(0.0, result);
                    }
                    else
                    {
                        PowerFunction imagCoeff = new PowerFunction(new Number(dBase), ex2);
                        ExComp imagCoeffEx = imagCoeff.SimplifyRadical();

                        resultEx = MulOp.StaticCombine(new Number(0.0, 1.0), imagCoeffEx);
                    }
                }

                if (isResultInt && resultEx == null)
                    resultEx = new Number(result);

                if (resultEx != null && negPow)
                    resultEx = AlgebraTerm.FromFraction(Number.GetOne(), resultEx);

                if (resultEx != null)
                    return resultEx;
            }
            else if (!n2.HasImaginaryComp())
            {
                double dPow = n2.GetRealComp();
                if (DoubleHelper.IsInteger(dPow))
                {
                    int iPow = (int)dPow;

                    //IMPROVE:
                    // This could be improved by a lot.
                    Number finalNCloned = n1.CloneEx() as Number;
                    for (int i = 1; i < iPow; ++i)
                    {
                        finalNCloned = Number.OpMul(finalNCloned, (Number)n1.CloneEx());
                    }

                    return finalNCloned;
                }
            }

            return null;
        }

        public static ExComp RaiseToPower(ExComp term, Number power, ref TermType.EvalData pEvalData, bool forceCombine = false)
        {
            if (!power.IsRealInteger())
                return StaticWeakCombine(term, power).ToAlgTerm();

            int powerInt = (int)power.GetRealComp();

            if (term is AlgebraTerm)
            {
                List<ExComp[]> groups = (term as AlgebraTerm).GetGroups();
                int groupCount = groups.Count;

                if (groups.Count == 1 && !forceCombine)
                {
                    return StaticCombine(term, power);
                }
                else if (groups.Count == 2 && powerInt > MIN_BINOM_COMPLEXITY)
                {
                    if (powerInt > MAX_BINOM_COMPLEXITY)
                        return StaticWeakCombine(term, power).ToAlgTerm();

                    // Use the binomial theorem.
                    AlgebraComp iterVar = new AlgebraComp("$k");
                    ChooseFunction chooseFunc = new ChooseFunction(power, iterVar);
                    AlgebraTerm group0 = GroupHelper.ToAlgTerm(groups[0]);
                    AlgebraTerm group1 = GroupHelper.ToAlgTerm(groups[1]);
                    ExComp overallEx = MulOp.StaticWeakCombine(chooseFunc, PowOp.StaticWeakCombine(group0, SubOp.StaticWeakCombine(power, iterVar)));
                    overallEx = MulOp.StaticWeakCombine(overallEx, PowOp.StaticWeakCombine(group1, iterVar));

                    // Don't display the work steps associated with this.
                    int startingWorkSteps = pEvalData.GetWorkMgr().GetWorkSteps().Count;
                    SumFunction sumFunc = new SumFunction(overallEx, iterVar, Number.GetZero(), power);
                    pEvalData.GetWorkMgr().PopSteps(startingWorkSteps);

                    return sumFunc.Evaluate(false, ref pEvalData);
                }

                int complexityRating = (int)Math.Pow(groupCount, powerInt);

                if (complexityRating > MAX_COMPLEXITY)
                    return StaticWeakCombine(term, power).ToAlgTerm();
            }

            if (powerInt >= MAX_COMBINE_COUNT)
                return StaticCombine(term, power);

            ExComp acumTerm = term.CloneEx();
            for (int i = 1; i < powerInt; ++i)
            {
                acumTerm = MulOp.StaticCombine(acumTerm.CloneEx(), term.CloneEx());
            }

            if (acumTerm is AlgebraTerm)
            {
                AlgebraTerm agAcumTerm = acumTerm as AlgebraTerm;
                agAcumTerm = agAcumTerm.ApplyOrderOfOperations();
                acumTerm = agAcumTerm.MakeWorkable();
                acumTerm = PowerFunction.FixFraction(acumTerm);
            }

            return acumTerm;
        }

        public static ExComp StaticCombine(ExComp ex1, ExComp ex2)
        {
            if (ex1 is AlgebraTerm)
                ex1 = (ex1 as AlgebraTerm).RemoveRedundancies(false);
            if (ex2 is AlgebraTerm)
                ex2 = (ex2 as AlgebraTerm).RemoveRedundancies(false);

            if (ex1 is Functions.Calculus.CalcConstant)
                return ex1;
            else if (ex2 is Functions.Calculus.CalcConstant)
                return ex2;

            if (Number.IsUndef(ex1) || Number.IsUndef(ex2))
                return Number.GetUndefined();

            if (Number.GetOne().IsEqualTo(ex2))
                return ex1;
            if (Number.GetOne().IsEqualTo(ex1))
                return ex1;
            if (Number.GetZero().IsEqualTo(ex2))
                return Number.GetOne();

            if (ex1 is ExMatrix || ex2 is ExMatrix)
            {
                ExMatrix mat;
                ExComp other;
                if (ex1 is ExMatrix)
                {
                    mat = ex1 as ExMatrix;
                    other = ex2;
                }
                else
                {
                    mat = ex2 as ExMatrix;
                    other = ex1;
                }

                // With pow op combines it can either be done or it can't. There is no
                // weak combine in between.
                ExComp atmpt = MatrixHelper.PowOpCombine(mat, other);
                if (atmpt == null)
                    return Number.GetUndefined();
                return atmpt;
            }

            if (ex1 is Number && ex2 is Number)
            {
                ExComp result = RaiseNumToNum(ex1, ex2);
                if (result != null)
                    return result;
            }
            if (ex2 is Number && !(ex2 as Number).HasImaginaryComp() && Number.OpLT((ex2 as Number), 0.0))
            {
                Number nEx2 = ex2 as Number;
                nEx2 = Number.OpSub(nEx2);
                ExComp raised = StaticCombine(ex1, nEx2);
                return AlgebraTerm.FromFraction(Number.GetOne(), raised);
            }
            else if (ex1 is PowerFunction)
            {
                PowerFunction powFunc1 = ex1 as PowerFunction;
                ExComp ex1Power = powFunc1.GetPower();
                powFunc1.SetPower(MulOp.StaticCombine(ex1Power, ex2));

                ExComp resultant = powFunc1.RemoveRedundancies(false);

                return resultant;
            }
            else if (ex1 is AlgebraTerm)
            {
                Term.SimpleFraction simpFrac = new Term.SimpleFraction();
                if (simpFrac.HarshInit(ex1 as AlgebraTerm))
                {
                    ExComp num = StaticCombine(simpFrac.GetNumEx(), ex2);
                    ExComp den = StaticCombine(simpFrac.GetDenEx(), ex2);

                    if (!num.IsEqualTo(StaticWeakCombine(simpFrac.GetNumEx(), ex2)) ||
                        !den.IsEqualTo(StaticWeakCombine(simpFrac.GetDenEx(), ex2)))
                    {
                        return AlgebraTerm.FromFraction(num, den);
                    }
                }

                if (ex2 is Number)
                {
                    Number nPow = ex2 as Number;
                    AlgebraTerm ex1Term = ex1 as AlgebraTerm;
                    List<ExComp[]> gps = ex1Term.GetGroupsNoOps();
                    if (gps.Count == 1)
                    {
                        ExComp[] gp = gps[0];
                        Number coeff = GroupHelper.GetCoeff(gp);

                        if (coeff != null && Number.OpLT(coeff, 0.0) && nPow.IsEven())
                        {
                            GroupHelper.AssignCoeff(gp, Number.Abs(coeff));
                            ex1 = GroupHelper.ToAlgTerm(gp);
                        }
                    }
                }
            }
            else if (ex1 is Number && ex2 is AlgebraTerm)
            {
                Number n1 = ex1 as Number;
                AlgebraTerm term2 = ex2 as AlgebraTerm;

                AlgebraTerm[] numDen = term2.GetNumDenFrac();

                if (numDen != null)
                {
                    ExComp num = numDen[0].RemoveRedundancies(false);
                    ExComp den = numDen[1].RemoveRedundancies(false);

                    if (num is Number && den is Number)
                    {
                        Number numeratorNum = num as Number;
                        Number denominatorNum = den as Number;

                        Number recipDen = denominatorNum.GetReciprocal();

                        if (Number.OpNotEquals(numeratorNum, 1.0))
                        {
                            ex1 = StaticCombine(n1, numeratorNum);
                            ex2 = AlgebraTerm.FromFraction(Number.GetOne(), denominatorNum);
                        }

                        ExComp result = RaiseNumToNum(ex1, recipDen);
                        if (result != null)
                        {
                            if (result is PowerFunction)
                                return (result as PowerFunction).SimplifyRadical();
                            return result;
                        }
                    }
                }
            }

            PowerFunction powFunc = new PowerFunction(ex1, ex2);
            return powFunc.SimplifyRadical();
        }

        public static ExComp StaticWeakCombine(ExComp ex1, ExComp ex2)
        {
            if (ex2 is Number)
            {
                if (Number.OpEqual((ex2 as Number), 0.0))
                    return Number.GetOne();
                else if (Number.OpEqual((ex2 as Number), 1.0))
                    return ex1;
            }
            if ((ex1 is ExMatrix || ex1 is FunctionDefinition || ex1 is AlgebraComp) && ex2 is AlgebraComp && (ex2 as AlgebraComp).GetVar().GetVar() == "T")
            {
                // This is the transpose operation.
                return new Transpose(ex1);
            }
            if ((ex1 is ExMatrix || ex1 is FunctionDefinition || ex1 is AlgebraComp))
            {
                if (ex1 is AlgebraComp)
                {
                    AlgebraComp cmp = ex1 as AlgebraComp;
                    if (cmp.GetVar().GetVar().Length != 1 || !Char.IsUpper(cmp.GetVar().GetVar()[0]))
                        return new PowerFunction(ex1, ex2);
                }

                // This is the inverse operation.
                return new MatrixInverse(ex1);
            }
            return new PowerFunction(ex1, ex2);
        }

        public static ExComp TakeRoot(ExComp ex1, Number root, ref TermType.EvalData pEvalData, bool showWork = false)
        {
            if (Number.GetZero().IsEqualTo(ex1))
            {
                // To get multiplicities correct two zeros must be returned.
                return new AlgebraTermArray(Number.GetZero().ToAlgTerm(), Number.GetZero().ToAlgTerm());
            }

            AlgebraTerm pow = AlgebraTerm.FromFraction(Number.GetOne(), root);

            // Using DeMoivre's theorem.
            if (root.IsRealInteger() && ex1 is Number && !root.IsEqualTo(new Number(2.0)))
            {
                Number n1 = ex1 as Number;

                // z^(1/n) = r^(1/n)(cos((θ+2πk)/n) + i*sin((θ+2πk))/n)
                ExComp r, theta;
                n1.GetPolarData(out r, out theta, ref pEvalData);

                AlgebraTerm[] roots = new AlgebraTerm[(int)root.GetRealComp()];

                for (Number k = Number.GetZero(); Number.OpLT(k, root); k = Number.OpAdd(k, 1.0))
                {
                    ExComp p0 = StaticCombine(r, pow);

                    ExComp num = MulOp.StaticCombine(new Number(2.0), k);
                    num = MulOp.StaticCombine(num, Constant.GetPi());
                    num = AddOp.StaticCombine(num, theta);
                    ExComp frac = DivOp.StaticCombine(num, root);

                    CosFunction cos = new CosFunction(frac);
                    ExComp p1 = cos.Evaluate(false, ref pEvalData);

                    SinFunction sin = new SinFunction(frac);
                    ExComp p2 = MulOp.StaticCombine(Number.GetImagOne(), sin.Evaluate(false, ref pEvalData));
                    ExComp finalExResult;
                    ExComp add = AddOp.StaticCombine(p1, p2);

                    if (add is AlgebraTerm)
                        add = (add as AlgebraTerm).CompoundFractions();
                    if (Number.GetOne().IsEqualTo(p0))
                    {
                        finalExResult = add;
                    }
                    else if (Number.GetOne().IsEqualTo(add))
                    {
                        finalExResult = p0;
                    }
                    else
                    {
                        if (add is AlgebraTerm && (add as AlgebraTerm).GetGroupCount() != 1)
                            finalExResult = MulOp.StaticWeakCombine(p0, add);
                        else
                            finalExResult = MulOp.StaticCombine(p0, add);
                    }

                    if (showWork)
                    {
                        pEvalData.GetWorkMgr().FromFormatted(WorkMgr.STM + "root({0})({1})={2}" + WorkMgr.EDM, "Using De Moivre's theorem the root " + WorkMgr.STM + "{2}" + WorkMgr.EDM + " was found.", root, ex1, finalExResult);
                    }

                    roots[(int)k.GetRealComp()] = finalExResult.ToAlgTerm();
                }

                return new AlgebraTermArray(roots);
            }

            ExComp result = StaticCombine(ex1, pow);

            AlgebraTerm pos = result.CloneEx().ToAlgTerm();
            if (root.IsEven())
            {
                AlgebraTerm neg = MulOp.Negate(result).ToAlgTerm();

                AlgebraTermArray agTermArray = new AlgebraTermArray(pos, neg);
                return agTermArray;
            }
            else
                return pos;
        }

        public static AlgebraTermArray TakeSqrt(ExComp ex1, ref TermType.EvalData pEvalData)
        {
            return TakeRoot(ex1, new Number(2.0), ref pEvalData) as AlgebraTermArray;
        }

        public static ExComp WeakTakeSqrt(ExComp ex1)
        {
            return StaticWeakCombine(ex1, AlgebraTerm.FromFraction(new Number(1.0), new Number(2.0)));
        }

        public override ExComp CloneEx()
        {
            return new PowOp();
        }

        public override ExComp Combine(ExComp ex1, ExComp ex2)
        {
            return StaticCombine(ex1, ex2);
        }

        public override string ToString()
        {
            return "^";
        }

        public override ExComp WeakCombine(ExComp ex1, ExComp ex2)
        {
            return StaticWeakCombine(ex1, ex2);
        }
    }
}