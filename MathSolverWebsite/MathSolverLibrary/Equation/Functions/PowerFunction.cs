﻿using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using MathSolverWebsite.MathSolverLibrary.Equation.Term;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions
{
    internal class PowerFunction : AlgebraFunction
    {
        private ExComp _power;
        private bool b_keepRedunFlag = false;

        public ExComp GetBase()
        {
            AlgebraTerm term = new AlgebraTerm();
            term.AssignTo(this);
            if (b_keepRedunFlag)
                return term;
            ExComp finalEx = term.RemoveRedundancies();

            return finalEx;
        }

        public void SetPower(ExComp value)
        {
            _power = value;
        }

        public ExComp GetPower()
        {
            return _power;
        }

        public PowerFunction(ExComp baseTerm, ExComp power)
        {
            if (baseTerm is AlgebraTerm && !(baseTerm is AlgebraFunction))
                _subComps.AddRange((baseTerm as AlgebraTerm).GetSubComps());
            else
                _subComps.Add(baseTerm);
            _power = power;
        }

        public static ExComp OpMul(PowerFunction pf1, PowerFunction pf2)
        {
            ExComp base1 = pf1.GetBase();
            ExComp base2 = pf2.GetBase();
            // We only combine non radicals.
            if (base1.IsEqualTo(base2))
            {
                ExComp combinedPow = AddOp.StaticCombine(pf1.GetPower(), pf2.GetPower());
                if (combinedPow is AlgebraTerm)
                {
                    combinedPow = (combinedPow as AlgebraTerm).CompoundFractions();
                    combinedPow = (combinedPow as AlgebraTerm).ReduceFracs();
                }
                AlgebraTerm combinedPowTerm = combinedPow.ToAlgTerm();
                if (combinedPowTerm.IsZero())
                {
                    AlgebraTerm term = new AlgebraTerm();
                    term.Add(new Number(1.0));
                    return term;
                }
                if (combinedPowTerm.IsOne())
                    return base1;
                PowerFunction resultant = new PowerFunction(base1, combinedPow);
                return resultant;
            }
            else if (pf1.GetPower().IsEqualTo(pf2.GetPower()))
            {
                ExComp combinedBase = MulOp.StaticCombine(base1, base2);
                return new PowerFunction(combinedBase, pf1.GetPower());
            }
            else
            {
                AlgebraTerm term = new AlgebraTerm();
                term.Add(pf1, new MulOp(), pf2);

                return term;
            }
        }

        public static ExComp OpMul(PowerFunction pf, AlgebraComp comp)
        {
            if (!pf.GetBase().IsEqualTo(comp))
            {
                if (Number.GetOne().IsEqualTo(pf.GetBase()))
                    return comp;

                AlgebraTerm term = new AlgebraTerm();
                term.Add(pf, new MulOp(), comp);

                return term;
            }
            PowerFunction compFunc = new PowerFunction(comp, new Number(1.0));
            ExComp resultant = PowerFunction.OpMul(pf, compFunc);
            return resultant;
        }

        public static ExComp OpMul(PowerFunction pf, AlgebraTerm term)
        {
            if (!pf.GetBase().IsEqualTo(term))
            {
                bool multiplyOut = true;

                List<ExComp[]> groups = term.GetGroupsNoOps();

                int modGroupCount = 0;

                for (int i = 0; i < groups.Count; ++i)
                {
                    ExComp[] group = groups[i];
                    bool equalTerm = false;
                    for (int j = 0; j < group.Length; ++j)
                    {
                        ExComp groupComp = group[j];
                        if (groupComp.IsEqualTo(pf))
                        {
                            equalTerm = true;
                            break;
                        }
                        else if (groupComp.IsEqualTo(pf.GetBase()) || (groupComp is PowerFunction && (groupComp as PowerFunction).GetBase().IsEqualTo(pf.GetBase())))
                        {
                            equalTerm = true;
                            break;
                        }
                        else if (groupComp is PowerFunction && (groupComp as PowerFunction).GetPower().IsEqualTo(pf.GetPower()) &&
                            (groupComp as PowerFunction).GetBase() is Number && pf.GetBase() is Number)
                        {
                            if (modGroupCount != i)
                                continue;

                            PowerFunction pfGc = groupComp as PowerFunction;
                            ExComp resultBase = MulOp.StaticCombine(pfGc.GetBase(), pf.GetBase());
                            PowerFunction pfAdd = new PowerFunction(resultBase, pf.GetPower());
                            group[j] = pfAdd;

                            groups[i] = group;

                            modGroupCount++;

                            break;
                        }
                    }

                    if (equalTerm)
                        break;

                    // This used to be an || but I changed it. I really don't know what the consequences of doing this are.
                    if (!equalTerm && modGroupCount != i + 1)
                    {
                        //multiplyOut = false;
                        break;
                    }
                }

                if (modGroupCount != 0)
                {
                    return (new AlgebraTerm(groups.ToArray())).MakeWorkable();
                }

                if (multiplyOut && (groups.Count != 1 || groups[0].Length != 1))
                {
                    for (int i = 0; i < groups.Count; ++i)
                    {
                        groups[i] = AlgebraTerm.MultiplyGroup(groups[i], pf);
                    }

                    AlgebraTerm multiplied = new AlgebraTerm(groups.ToArray());
                    ExComp final = multiplied;//.MakeWorkable();

                    return final;
                }

                return MulOp.StaticWeakCombine(pf, term);
            }
            PowerFunction termFunc = new PowerFunction(term, new Number(1.0));
            ExComp resultant = PowerFunction.OpMul(pf, termFunc);
            return resultant;
        }

        public static ExComp OpDiv(PowerFunction pf1, PowerFunction pf2)
        {
            ExComp base1 = pf1.GetBase();
            ExComp base2 = pf2.GetBase();
            if (base1.IsEqualTo(base2))
            {
                AlgebraTerm power = new AlgebraTerm();
                power.Add(pf1.GetPower(), new SubOp(), pf2.GetPower());
                ExComp workablePow = power.MakeWorkable();
                if ((workablePow is AlgebraTerm && (workablePow as AlgebraTerm).IsZero()) ||
                    (workablePow is Number && Number.OpEqual((workablePow as Number), 0.0)))
                {
                    AlgebraTerm term = new AlgebraTerm();
                    term.Add(new Number(1.0));
                    return term;
                }

                if (power is AlgebraTerm)
                    power = (power as AlgebraTerm).CompoundFractions();

                PowerFunction resultant = new PowerFunction(base1, power);
                return resultant;
            }
            else
            {
                AlgebraTerm term = new AlgebraTerm();
                term.Add(pf1, new DivOp(), pf2);

                return term;
            }
        }

        public static ExComp OpAdd(PowerFunction pf1, PowerFunction pf2)
        {
            AlgebraTerm term = new AlgebraTerm();
            term.Add(pf1, new AddOp(), pf2);
            return term;
        }

        public override void CallFunction(FunctionDefinition funcDef, ExComp def, ref TermType.EvalData pEvalData, bool callSubTerms = true)
        {
            AlgebraTerm baseTerm = GetBase().ToAlgTerm();
            AlgebraTerm powTerm = GetPower().ToAlgTerm();

            baseTerm.CallFunction(funcDef, def, ref pEvalData, callSubTerms);
            powTerm.CallFunction(funcDef, def, ref pEvalData, callSubTerms);

            _power = powTerm;
            SetSubComps(baseTerm.GetSubComps());
        }

        public override bool CallFunctions(ref TermType.EvalData pEvalData)
        {
            AlgebraTerm baseTerm = GetBase().ToAlgTerm();
            AlgebraTerm powTerm = GetPower().ToAlgTerm();

            if (!baseTerm.CallFunctions(ref pEvalData) || !powTerm.CallFunctions(ref pEvalData))
                return false;

            _power = powTerm;
            _subComps = new List<ExComp>();
            _subComps.Add(baseTerm);

            return true;
        }

        public static ExComp FixFraction(ExComp power)
        {
            if (power is AlgebraTerm)
                power = (power as AlgebraTerm).RemoveRedundancies();
            if (power is PowerFunction && (power as PowerFunction).GetPower().IsEqualTo(Number.GetNegOne()))
            {
                power = MulOp.StaticWeakCombine(Number.GetOne(), power);
            }

            return power;
        }

        public override AlgebraTerm ApplyOrderOfOperations()
        {
            AlgebraTerm innerTerm = GetBase().ToAlgTerm();
            innerTerm = innerTerm.ApplyOrderOfOperations();

            if (_power is AlgebraTerm)
                _power = (_power as AlgebraTerm).ApplyOrderOfOperations();

            return new PowerFunction(innerTerm, _power);
        }

        public override void AssignTo(AlgebraTerm algebraTerm)
        {
            if (algebraTerm is PowerFunction)
            {
                PowerFunction powerFunc = algebraTerm as PowerFunction;
                base.AssignTo(powerFunc);
                _power = new AlgebraTerm();
                _power = powerFunc.GetPower();
            }
            else
                throw new ArgumentException();
        }

        public override ExComp CloneEx()
        {
            return new PowerFunction(GetBase().CloneEx(), _power.CloneEx());
        }

        public override AlgebraTerm CompoundFractions()
        {
            ExComp innerCompounded = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).CompoundFractions() : GetBase();
            ExComp powerCompounded = _power is AlgebraTerm ? (_power as AlgebraTerm).CompoundFractions() : _power;

            return new PowerFunction(innerCompounded, powerCompounded);
        }

        public override AlgebraTerm CompoundFractions(out bool valid)
        {
            bool innerValid = false;
            ExComp innerCompounded = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).CompoundFractions(out innerValid) : GetBase();

            bool powerValid = false;
            ExComp powerCompounded = _power is AlgebraTerm ? (_power as AlgebraTerm).CompoundFractions(out powerValid) : _power;

            valid = innerValid || powerValid;

            return new PowerFunction(innerCompounded, powerCompounded);
        }

        public override bool Contains(AlgebraComp varFor)
        {
            if (GetBase() is AlgebraTerm && (GetBase() as AlgebraTerm).Contains(varFor))
                return true;
            else if (GetBase() is AlgebraComp && (GetBase() as AlgebraComp).IsEqualTo(varFor))
                return true;

            if (GetPower() is AlgebraTerm && (GetPower() as AlgebraTerm).Contains(varFor))
                return true;
            else if (GetPower() is AlgebraComp && (GetPower() as AlgebraComp).IsEqualTo(varFor))
                return true;

            return false;
        }

        public override AlgebraTerm ConvertImaginaryToVar()
        {
            ExComp baseEx = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).ConvertImaginaryToVar() : GetBase();
            ExComp powEx = GetPower() is AlgebraTerm ? (GetPower() as AlgebraTerm).ConvertImaginaryToVar() : GetPower();

            return new PowerFunction(baseEx, powEx);
        }

        public override ExComp CancelWith(ExComp innerEx, ref TermType.EvalData evalData)
        {
            AlgebraTerm power = _power.ToAlgTerm().CompoundLogs();
            power = power.ForceLogCoeffToPow();
            ExComp powerEx = power.RemoveRedundancies();

            LogFunction log = powerEx as LogFunction;
            if (log == null)
                return null;
            if (log.GetBase().IsEqualTo(innerEx))
                return log.GetInnerEx();

            return null;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            ExComp baseEx = GetBase();
            if (Number.IsUndef(baseEx) || Number.IsUndef(_power))
                return Number.GetUndefined();

            if (Number.GetZero().IsEqualTo(baseEx) && !Number.GetNegOne().IsEqualTo(_power))
                return Number.GetZero();

            if (baseEx is AlgebraTerm)
            {
                AlgebraTerm surroundingBase = new AlgebraTerm(baseEx);
                surroundingBase.EvaluateFunctions(harshEval, ref pEvalData);
                baseEx = surroundingBase;
            }

            if (_power is AlgebraTerm)
            {
                AlgebraTerm surroundingPow = new AlgebraTerm(_power);
                surroundingPow.EvaluateFunctions(harshEval, ref pEvalData);
                _power = surroundingPow;
            }

            if (harshEval)
            {
                if (baseEx is Number && _power is Number)
                {
                    Number nBase = baseEx as Number;
                    Number nPow = _power as Number;

                    Number raised = Number.RaiseToPower(nBase, nPow);
                    if (raised != null)
                        return raised;
                }
            }

            //return PowOp.StaticCombine(baseEx, _power);
            return new PowerFunction(baseEx, _power);
        }

        public override string FinalToAsciiString()
        {
            string powerAsciiStr = _power.ToAsciiString();
            if (powerAsciiStr == ("-1"))
                powerAsciiStr = powerAsciiStr.Remove(0, 2);

            ExComp baseNoRedun = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).RemoveRedundancies() : GetBase();
            if (baseNoRedun is TrigFunction || baseNoRedun is InverseTrigFunction)
            {
                BasicAppliedFunc funcBase = baseNoRedun as BasicAppliedFunc;

                return funcBase.GetFuncName() + "^{" + powerAsciiStr + "}(" + funcBase.GetInnerTerm().ToAsciiString() + ")";
            }

            string baseAsciiStr = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).FinalToDispStr() : GetBase().ToAsciiString();

            if (powerAsciiStr == "")
                return baseAsciiStr;

            if (powerAsciiStr == "0.5")
                return @"sqrt(" + baseAsciiStr + ")";
            else if (_power is AlgebraTerm)
            {
                AlgebraTerm powTerm = _power as AlgebraTerm;

                AlgebraTerm[] numDen = powTerm.GetNumDenFrac();
                if (numDen != null)
                {
                    ExComp num = numDen[0].RemoveRedundancies();
                    ExComp den = numDen[1].RemoveRedundancies();

                    if (Number.GetOne().IsEqualTo(num))
                    {
                        if ((new Number(2.0)).IsEqualTo(den))
                            return "sqrt(" + baseAsciiStr + ")";
                        else
                            return "root(" + den.ToAsciiString() + ")(" + baseAsciiStr + ")";
                    }
                }
            }

            if (!(GetBase() is Constant || GetBase() is AlgebraComp || GetBase() is BasicAppliedFunc))
                baseAsciiStr = baseAsciiStr.SurroundWithParas();

            return baseAsciiStr + "^(" + powerAsciiStr + ")";
        }

        public override string FinalToTexString()
        {
            string powerTexStr = _power.ToTexString();
            if (powerTexStr == ("-1"))
                powerTexStr = powerTexStr.Remove(0, 2);

            ExComp baseNoRedun = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).RemoveRedundancies() : GetBase();
            if (baseNoRedun is TrigFunction || baseNoRedun is InverseTrigFunction)
            {
                BasicAppliedFunc funcBase = baseNoRedun as BasicAppliedFunc;

                return funcBase.GetFuncName() + "^{" + powerTexStr + "}(" + funcBase.GetInnerTerm().ToTexString() + ")";
            }

            string baseTexStr = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).FinalToDispStr() : GetBase().ToTexString();

            if (powerTexStr == "")
                return baseTexStr;

            if (powerTexStr == "0.5")
                return @"sqrt(" + baseTexStr + ")";
            else if (_power is AlgebraTerm)
            {
                AlgebraTerm powTerm = _power as AlgebraTerm;

                AlgebraTerm[] numDen = powTerm.GetNumDenFrac();
                if (numDen != null)
                {
                    ExComp num = numDen[0].RemoveRedundancies();
                    ExComp den = numDen[1].RemoveRedundancies();

                    if (Number.GetOne().IsEqualTo(num))
                    {
                        if ((new Number(2.0)).IsEqualTo(den))
                            return "sqrt(" + baseTexStr + ")";
                        else
                            return "root(" + den.ToTexString() + ")(" + baseTexStr + ")";
                    }
                }
            }
            else
            {
                if (!Regex.IsMatch(baseTexStr, @"^\d$") && baseTexStr.Length > 1 && !(baseTexStr.StartsWith("(") && baseTexStr.EndsWith(")")))
                    baseTexStr = baseTexStr.SurroundWithParas();
            }

            return baseTexStr + "^{" + powerTexStr + "}";
        }

        public ExComp FlipFrac()
        {
            AlgebraTerm powTerm = new AlgebraTerm(Number.GetNegOne(), new MulOp(), _power);
            _power = powTerm.MakeWorkable();

            if (_power is AlgebraTerm)
                _power = (_power as AlgebraTerm).RemoveRedundancies();

            if (_power is Number && Number.OpEqual((_power as Number), 1.0))
                return GetBase();

            return this;
        }

        /// <summary>
        /// Multiplies exponents to exponents. A common use is for combining the denominator -1 with the denominator exponent so (x^3)^-1 will turn into x^-3.
        /// </summary>
        /// <returns></returns>
        public override AlgebraTerm ForceCombineExponents()
        {
            ExComp baseEx = GetBase();
            if (GetBase() is AlgebraTerm)
                baseEx = (GetBase() as AlgebraTerm).ForceCombineExponents();

            if (GetBase() is PowerFunction)
            {
                ExComp pow = (GetBase() as PowerFunction).GetPower();
                ExComp mulPow = MulOp.StaticCombine(pow, _power);

                return new PowerFunction((GetBase() as PowerFunction).GetBase(), mulPow);
            }

            return new PowerFunction(baseEx, _power);
        }

        public override List<FunctionType> GetAppliedFunctionsNoPow(AlgebraComp varFor)
        {
            AlgebraTerm surrounded = new AlgebraTerm(this);
            return surrounded.GetAppliedFunctionsNoPow(varFor);
        }

        public override double GetCompareVal()
        {
            Number powerNum = null;
            if (_power is AlgebraTerm)
            {
                AlgebraTerm powerTerm = _power as AlgebraTerm;
                if (powerTerm.GetTermCount() == 1 && powerTerm.GetSubComps()[0] is Number)
                    powerNum = powerTerm.GetSubComps()[0] as Number;
                AlgebraTerm[] numDen = powerTerm.GetNumDenFrac();
                if (numDen != null)
                {
                    ExComp num = numDen[0].RemoveRedundancies();
                    ExComp den = numDen[1].RemoveRedundancies();
                    if (Number.GetOne().IsEqualTo(num) && den is Number && (den as Number).IsRealInteger())
                        return (den as Number).GetReciprocal().GetRealComp();
                }
            }
            else if (_power is Number)
            {
                powerNum = _power as Number;
            }

            if (powerNum == null)
                return 1.0;
            return powerNum.GetRealComp();
        }

        public override List<Restriction> GetDomain(AlgebraVar varFor, AlgebraSolver agSolver, ref TermType.EvalData pEvalData)
        {
            int root;
            if (HasIntRoot(out root))
            {
                if (root % 2 != 0 || GetBase() is Number)
                    return null;

                pEvalData.GetWorkMgr().FromFormatted(WorkMgr.STM + this.FinalToDispStr() + WorkMgr.EDM, "The above radical cannot be anything less than 0. The domain will be restricted by " + WorkMgr.STM + "{0}" +
                    Restriction.ComparisonOpToStr(Parsing.LexemeType.GreaterEqual) + "0" + WorkMgr.EDM, GetBase());

                SolveResult result = agSolver.SolveRegInequality(GetBase().ToAlgTerm(), Number.GetZero().ToAlgTerm(), Parsing.LexemeType.GreaterEqual, varFor, ref pEvalData);
                if (!result.Success)
                    return null;

                return result.Restrictions;
            }

            return new List<Restriction>();
        }

        public override List<ExComp[]> GetGroups()
        {
            List<ExComp[]> groups = new List<ExComp[]>();

            AlgebraTerm baseTerm = new AlgebraTerm();
            foreach (ExComp comp in _subComps)
            {
                baseTerm.Add(comp);
            }

            ExComp baseExComp = baseTerm.RemoveRedundancies();

            PowerFunction powFunc = new PowerFunction(baseExComp, _power);
            ExComp[] group = { powFunc };

            groups.Add(group);

            return groups;
        }

        public override List<ExComp> GetPowersOfVar(AlgebraComp varFor)
        {
            List<ExComp> powers = new List<ExComp>();
            if (GetBase() is AlgebraTerm && (GetBase() as AlgebraTerm).Contains(varFor))
                powers.Add(_power);
            else if (GetBase() is AlgebraComp && (GetBase() as AlgebraComp).IsEqualTo(varFor))
                powers.Add(_power);
            else
                powers = base.GetPowersOfVar(varFor);

            return powers.Distinct().ToList();
        }

        public override AlgebraTerm HarshEvaluation()
        {
            ExComp evalPow = _power;
            if (_power is AlgebraTerm)
                evalPow = (_power as AlgebraTerm).HarshEvaluation();

            ExComp evalBase;
            if (!(evalPow is Number) && GetBase() is Constant)
                evalBase = GetBase();
            else
                evalBase = GetBase().ToAlgTerm().HarshEvaluation();

            PowerFunction powFunc = new PowerFunction(evalBase, evalPow);
            return powFunc;
        }

        public bool HasIntPow()
        {
            if (_power is AlgebraTerm)
                _power = (_power as AlgebraTerm).RemoveRedundancies();
            if (_power is Number && (_power as Number).IsRealInteger())
            {
                return true;
            }

            return false;
        }

        public bool HasIntPow(out int pow)
        {
            pow = int.MaxValue;
            if (_power is AlgebraTerm)
                _power = (_power as AlgebraTerm).RemoveRedundancies();
            if (_power is Number && (_power as Number).IsRealInteger())
            {
                pow = (int)(_power as Number).GetRealComp();
                return true;
            }

            return false;
        }

        public bool HasIntRoot(out int root)
        {
            root = int.MaxValue;
            if (_power is AlgebraTerm)
                _power = (_power as AlgebraTerm).RemoveRedundancies();
            if (_power is Number)
            {
                Number powRecip = (_power as Number).GetReciprocal();
                if (!powRecip.IsRealInteger())
                    return false;
                root = (int)powRecip.GetRealComp();
            }
            else if (_power is AlgebraTerm)
            {
                AlgebraTerm powTerm = _power as AlgebraTerm;
                Term.SimpleFraction frac = new Term.SimpleFraction();
                if (!frac.Init(powTerm))
                    return false;

                ExComp recip = frac.GetReciprocal();
                if (recip is Number && (recip as Number).IsRealInteger())
                {
                    root = (int)(recip as Number).GetRealComp();
                }
            }
            else
                return false;

            return true;
        }

        public override bool HasLogFunctions()
        {
            bool baseHas = false;
            if (GetBase() is AlgebraTerm)
                baseHas = (GetBase() as AlgebraTerm).HasLogFunctions();

            if (baseHas)
                return true;

            bool powHas = false;
            if (_power is AlgebraTerm)
                powHas = (_power as AlgebraTerm).HasLogFunctions();

            return powHas;
        }

        public override bool HasTrigFunctions()
        {
            return _power.ToAlgTerm().HasTrigFunctions() || GetBase().ToAlgTerm().HasTrigFunctions();
        }

        public override bool HasVariablePowers(AlgebraComp varFor)
        {
            if (base.HasVariablePowers(varFor))
                return true;

            if (_power is AlgebraTerm)
            {
                AlgebraTerm powerTerm = _power as AlgebraTerm;
                if (powerTerm.Contains(varFor))
                    return true;
            }
            else if (_power is AlgebraComp && _power.IsEqualTo(varFor))
                return true;

            return false;
        }

        public bool IsDenominator()
        {
            if (_power is AlgebraTerm)
                _power = (_power as AlgebraTerm).RemoveRedundancies();
            if (_power is Number)
            {
                Number powNum = _power as Number;
                return Number.OpEqual(powNum, -1.0);
            }

            return false;
        }

        public override bool IsEqualTo(ExComp ex)
        {
            if (ex is PowerFunction)
            {
                PowerFunction powerFunc = ex as PowerFunction;

                if (GetBase().IsEqualTo(powerFunc.GetBase()))
                {
                    // There are different ways to represent powers.
                    // 0.5 or 1/2.
                    if (powerFunc.GetPower() is AlgebraTerm && _power is Number)
                    {
                        AlgebraTerm powerFuncPowTerm = powerFunc.GetPower() as AlgebraTerm;
                        AlgebraTerm[] numDen = powerFuncPowTerm.GetNumDenFrac();
                        if (numDen != null)
                        {
                            ExComp num = numDen[0].RemoveRedundancies();
                            ExComp den = numDen[1].RemoveRedundancies();

                            if (num is Number && den is Number)
                            {
                                ExComp divNum = Number.OpDiv((num as Number), (den as Number));
                                if (divNum.IsEqualTo(_power))
                                    return true;
                            }
                        }
                    }
                    else if (_power is AlgebraTerm && powerFunc.GetPower() is Number)
                    {
                        AlgebraTerm powerTerm = _power as AlgebraTerm;
                        AlgebraTerm[] numDen = powerTerm.GetNumDenFrac();
                        if (numDen != null)
                        {
                            ExComp num = numDen[0].RemoveRedundancies();
                            ExComp den = numDen[1].RemoveRedundancies();

                            if (num is Number && den is Number)
                            {
                                ExComp divNum = Number.OpDiv((num as Number), (den as Number));
                                if (divNum.IsEqualTo(powerFunc.GetPower()))
                                    return true;
                            }
                        }
                    }
                    else
                        return _power.IsEqualTo(powerFunc.GetPower());
                }
            }

            return false;
        }

        public bool IsRadical()
        {
            if (_power is Number && !(_power as Number).IsRealInteger())
                return true;

            if (_power is AlgebraTerm && (_power as AlgebraTerm).ContainsOnlyFractions())
                return true;

            return false;
        }

        public override bool IsUndefined()
        {
            if (GetBase() is Number && (GetBase() as Number).IsUndefined())
                return true;
            if (_power is Number && (_power as Number).IsUndefined())
                return true;
            if (GetBase() is AlgebraTerm && (GetBase() as AlgebraTerm).IsUndefined())
                return true;
            if (_power is AlgebraTerm && (_power as AlgebraTerm).IsUndefined())
                return true;
            return false;
        }

        public override ExComp MakeWorkable()
        {
            // Here redundancies are important to keeping the order of operations.
            b_keepRedunFlag = true;
            ExComp baseTerm = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).MakeWorkable() : GetBase();
            b_keepRedunFlag = false;
            ExComp powerTerm = _power;
            if (_power is AlgebraTerm)
            {
                powerTerm = (_power as AlgebraTerm).MakeWorkable();
            }

            powerTerm = FixFraction(powerTerm);

            return new PowerFunction(baseTerm, powerTerm);
        }

        public override AlgebraTerm Order()
        {
            ExComp baseTerm = GetBase();
            ExComp powerTerm = _power;

            if (GetBase() is AlgebraTerm)
                baseTerm = (GetBase() as AlgebraTerm).Order();
            if (GetPower() is AlgebraTerm)
                powerTerm = (_power as AlgebraTerm).Order();

            PowerFunction powFunc = new PowerFunction(baseTerm, powerTerm);
            return powFunc;
        }

        public override List<ExComp[]> PopGroups()
        {
            List<ExComp[]> groups = GetGroups();

            _subComps.Clear();

            return groups;
        }

        public override AlgebraTerm PushGroups(List<ExComp[]> groups)
        {
            if (groups.Count == 1 && groups[0].Count() == 1 && groups[0][0] is PowerFunction)
            {
                PowerFunction powerFunc = groups[0][0] as PowerFunction;
                return powerFunc;
            }
            else
            {
                AlgebraTerm term = new AlgebraTerm();
                foreach (ExComp[] group in groups)
                    term.AddGroup(group);

                return term;
            }
        }

        public override AlgebraTerm RemoveOneCoeffs()
        {
            ExComp baseEx = GetBase();
            if (baseEx is AlgebraTerm)
                baseEx = (baseEx as AlgebraTerm).RemoveOneCoeffs();

            return new PowerFunction(baseEx, _power);
        }

        public override ExComp RemoveRedundancies(bool postWorkable = false)
        {
            ExComp baseTerm = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).RemoveRedundancies(postWorkable) : GetBase();
            ExComp powerTerm = GetPower() is AlgebraTerm ? (GetPower() as AlgebraTerm).RemoveRedundancies(postWorkable) : GetPower();

            if (powerTerm is Number)
            {
                Number powerNum = powerTerm as Number;
                if (Number.OpEqual(powerNum, 0.0))
                    return new Number(1.0);
                else if (Number.OpEqual(powerNum, 1.0))
                    return baseTerm;
            }

            return new PowerFunction(baseTerm, powerTerm);
        }

        public override AlgebraTerm RemoveZeros()
        {
            AlgebraTerm baseTerm = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).RemoveZeros() : GetBase().ToAlgTerm();

            AlgebraTerm oneTerm = new AlgebraTerm();
            oneTerm.Add(new Number(1.0));

            ExComp power = null;
            if (_power is AlgebraTerm)
            {
                AlgebraTerm powerTerm = _power as AlgebraTerm;
                powerTerm = powerTerm.RemoveZeros();
                if (powerTerm.GetTermCount() == 0)
                    return oneTerm;

                power = powerTerm;
            }
            else if (_power is Number)
            {
                Number number = _power as Number;
                if (Number.OpEqual(number, 0.0))
                    return oneTerm;
                if (Number.OpEqual(number, 1.0))
                    return baseTerm;

                power = number;
            }
            else
                power = _power;

            PowerFunction powerFunc = new PowerFunction(baseTerm, power);
            return powerFunc;
        }

        public ExComp SimplifyRadical()
        {
            int root;
            if (!HasIntRoot(out root))
                return this;

            // See if any of the radical terms can be moved out.
            AlgebraTerm baseTerm = GetBase().ToAlgTerm();
            baseTerm = baseTerm.SimpleFactor();
            if (baseTerm.GetGroupCount() != 1)
                return this;
            List<ExComp> mulTerms = new List<ExComp>();
            for (int i = 0; i < baseTerm.GetTermCount(); ++i)
            {
                ExComp subComp = baseTerm[i];

                if (subComp is Number && (subComp as Number).IsRealInteger())
                {
                    Number nSubComp = subComp as Number;
                    int n = (int)nSubComp.GetRealComp();

                    if (root % 2 == 0)
                    {
                        // This is an even root. Take out any negative terms in the radical.
                        if (n < 0)
                        {
                            mulTerms.Add(new Number(0.0, 1.0));
                            baseTerm[i] = Number.OpSub(nSubComp);
                            n = -n;
                        }
                    }

                    int[] divisors;
                    if (root % 2 != 0)
                        divisors = Math.Abs(n).GetDivisors(true);
                    else
                        divisors = n.GetDivisors(true);

                    for (int j = divisors.Length - 1; j >= 0; --j)
                    {
                        int divisor = divisors[j];
                        int rootResult;
                        if (divisor.IsPerfectRoot(root, out rootResult))
                        {
                            n = n / divisor;
                            if (n < 0 && root % 2 != 0)
                            {
                                rootResult = -rootResult;
                                n = -n;
                            }
                            baseTerm[i] = new Number(n);
                            Number outside = new Number(rootResult);
                            mulTerms.Add(outside);
                            break;
                        }
                    }
                }
                else if (subComp is PowerFunction)
                {
                    PowerFunction pfSubComp = subComp as PowerFunction;
                    ExComp powBase = pfSubComp.GetBase();

                    int comparePow;
                    if (pfSubComp.HasIntPow(out comparePow))
                    {
                        if (comparePow == 1 || comparePow == 0)
                            continue;
                        int outside = comparePow / root;
                        int inside = comparePow % root;

                        if (outside != 0)
                        {
                            ExComp outsideEx = outside == 1 ? powBase : PowOp.StaticCombine(powBase, new Number(outside));
                            ExComp insideEx = inside == 1 ? powBase : PowOp.StaticCombine(powBase, new Number(inside));

                            baseTerm[i] = insideEx;
                            mulTerms.Add(outsideEx);
                        }
                    }
                }
            }

            if (mulTerms.Count == 0)
                return this;

            AlgebraTerm outsideTerm = mulTerms.ToArray().ToAlgTerm();
            if (baseTerm.IsOne())
                return outsideTerm;

            ExComp baseEx = baseTerm.MakeWorkable();
            if (baseEx is AlgebraTerm)
                baseEx = (baseEx as AlgebraTerm).RemoveRedundancies();
            if (Number.GetOne().IsEqualTo(baseEx))
                return outsideTerm.MakeWorkable();

            ExComp finalPowFunc = PowOp.StaticWeakCombine(baseEx, _power);
            ExComp finalTerm = MulOp.StaticWeakCombine(outsideTerm.MakeWorkable(), finalPowFunc);

            return finalTerm;
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn)
        {
            ExComp baseEx = GetBase();

            if (subOut is PowerFunction)
            {
                PowerFunction pfSubOut = subOut as PowerFunction;
                if (pfSubOut.GetBase().IsEqualTo(baseEx))
                {
                    ExComp gcf = DivOp.GetCommonFactor(_power, pfSubOut._power);

                    if (gcf != null && !Number.GetZero().IsEqualTo(gcf) && !Number.GetOne().IsEqualTo(gcf))
                    {
                        ExComp divPow = DivOp.StaticCombine(_power, gcf);
                        return PowOp.StaticCombine(subIn, divPow).ToAlgTerm();
                    }
                }
            }

            if (baseEx.IsEqualTo(subOut))
            {
                baseEx = subIn;
            }
            else if (baseEx is AlgebraTerm)
            {
                baseEx = (baseEx as AlgebraTerm).Substitute(subOut, subIn);
            }

            ExComp powerEx = _power;
            if (powerEx.IsEqualTo(subOut))
                powerEx = subIn;
            else if (powerEx is AlgebraTerm)
            {
                powerEx = (powerEx as AlgebraTerm).Substitute(subOut, subIn);
            }

            return new PowerFunction(baseEx, powerEx);
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn, ref bool success)
        {
            ExComp baseEx = GetBase();

            if (subOut is PowerFunction)
            {
                PowerFunction pfSubOut = subOut as PowerFunction;
                if (pfSubOut.GetBase().IsEqualTo(baseEx))
                {
                    ExComp gcf = DivOp.GetCommonFactor(_power, pfSubOut._power);

                    if (gcf != null && !Number.GetZero().IsEqualTo(gcf) && !Number.GetOne().IsEqualTo(gcf))
                    {
                        ExComp divPow = DivOp.StaticCombine(_power, gcf);
                        success = true;
                        return PowOp.StaticCombine(subIn, divPow).ToAlgTerm();
                    }
                }
            }

            if (baseEx.IsEqualTo(subOut))
            {
                success = true;
                baseEx = subIn;
            }
            else if (baseEx is AlgebraTerm)
            {
                baseEx = (baseEx as AlgebraTerm).Substitute(subOut, subIn, ref success);
            }

            ExComp powerEx = _power;
            if (powerEx.IsEqualTo(subOut))
            {
                success = true;
                powerEx = subIn;
            }
            else if (powerEx is AlgebraTerm)
            {
                powerEx = (powerEx as AlgebraTerm).Substitute(subOut, subIn, ref success);
            }

            return new PowerFunction(baseEx, powerEx);
        }

        public override bool TermsRelatable(ExComp comp)
        {
            if (comp is AlgebraTerm)
                comp = (comp as AlgebraTerm).RemoveRedundancies();

            // This is to prevent stuff like 2 and sqrt(2) from combining.
            // While technically these could combine to keep things looking nice
            // they are not combined in this program.
            if (comp is Number)
                return false;

            if (comp is PowerFunction)
            {
                PowerFunction powFunc = comp as PowerFunction;
                if (GetBase().IsEqualTo(powFunc.GetBase()))
                    return true;
                return false;
            }

            return GetBase().IsEqualTo(comp);
        }

        public override string ToAsciiString()
        {
            string powerAsciiStr = _power.ToAsciiString();
            if (powerAsciiStr.StartsWith("-1") && !powerAsciiStr.StartsWith("-1*") && !powerAsciiStr.StartsWith("-1."))
                powerAsciiStr = powerAsciiStr.Remove(0, 2);

            ExComp baseNoRedun = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).RemoveRedundancies() : GetBase();
            if (baseNoRedun is TrigFunction || baseNoRedun is InverseTrigFunction)
            {
                BasicAppliedFunc funcBase = baseNoRedun as BasicAppliedFunc;

                return funcBase.GetFuncName() + "^{" + powerAsciiStr + "}(" + funcBase.GetInnerTerm().ToAsciiString() + ")";
            }

            string baseAsciiStr = GetBase().ToAsciiString();

            if (powerAsciiStr == "")
                return baseAsciiStr;

            bool surrounded = false;

            if (powerAsciiStr == "0.5")
            {
                string finalBaseAsciiStr = WorkMgr.ToDisp(GetBase());
                return @"sqrt(" + finalBaseAsciiStr + ")";
            }
            else if (_power is AlgebraTerm)
            {
                string finalBaseAsciiStr = WorkMgr.ToDisp(GetBase());
                AlgebraTerm powTerm = _power as AlgebraTerm;

                AlgebraTerm[] numDen = powTerm.GetNumDenFrac();
                if (numDen != null)
                {
                    ExComp num = numDen[0].RemoveRedundancies();
                    ExComp den = numDen[1].RemoveRedundancies();

                    if (Number.GetOne().IsEqualTo(num))
                    {
                        if ((new Number(2.0)).IsEqualTo(den))
                            return "sqrt(" + finalBaseAsciiStr + ")";
                        else
                            return "root(" + den.ToAsciiString() + ")(" + finalBaseAsciiStr + ")";
                    }
                }
            }
            else
            {
                if ((!Regex.IsMatch(baseAsciiStr, @"^\d$") && baseAsciiStr.Length > 1 && !(baseAsciiStr.StartsWith("(") && baseAsciiStr.EndsWith(")"))))
                {
                    surrounded = true;
                    baseAsciiStr = baseAsciiStr.SurroundWithParas();
                }
            }

            if (!surrounded && GetBase() is Number)
            {
                baseAsciiStr = baseAsciiStr.SurroundWithParas();
            }

            return baseAsciiStr + "^(" + powerAsciiStr + ")";
        }

        public override string ToJavaScriptString(bool useRad)
        {
            string baseStr = base.ToJavaScriptString(useRad);
            if (baseStr == null)
                return null;
            string powerStr = _power.ToJavaScriptString(useRad);
            if (powerStr == null)
                return null;

            return "Math.pow(" + baseStr + "," + powerStr + ")";
        }

        public override string ToString()
        {
            if (MathSolver.USE_TEX_DEBUG)
                return ToTexString();
            return "PF(" + base.ToString() + ", " + _power.ToString() + ")";
        }

        public override string ToTexString()
        {
            string powerTexStr = _power.ToTexString();
            if (powerTexStr.StartsWith("-1") && !powerTexStr.StartsWith("-1*") && !powerTexStr.StartsWith("-1."))
                powerTexStr = powerTexStr.Remove(0, 2);

            ExComp baseNoRedun = GetBase() is AlgebraTerm ? (GetBase() as AlgebraTerm).RemoveRedundancies() : GetBase();
            if (baseNoRedun is TrigFunction || baseNoRedun is InverseTrigFunction)
            {
                BasicAppliedFunc funcBase = baseNoRedun as BasicAppliedFunc;

                return funcBase.GetFuncName() + "^{" + powerTexStr + "}(" + funcBase.GetInnerTerm().ToTexString() + ")";
            }

            string baseTexStr = GetBase().ToTexString();

            if (powerTexStr == "")
                return baseTexStr;

            if (powerTexStr == "0.5" || powerTexStr == @"\frac{1}{2}")
                return @"\sqrt{" + baseTexStr + "}";
            else if (powerTexStr.StartsWith(@"\frac"))
            {
                MatchCollection matches = Regex.Matches(powerTexStr, MathSolverLibrary.Parsing.LexicalParser.REAL_NUM_PATTERN);
                if (matches.Count != 2)
                    throw new ArgumentException();
                string numStr = matches[0].Value;
                string denStr = matches[1].Value;

                string radicalRootStr;
                if (numStr == "1")
                {
                    radicalRootStr = denStr;
                }
                else
                {
                    radicalRootStr = @"\frac{" + denStr + "}{" + numStr + "}";
                }

                return @"\sqrt[" + radicalRootStr + "]{" + baseTexStr + "}";
            }
            else
            {
                if (!Regex.IsMatch(baseTexStr, @"^\d$") && baseTexStr.Length > 1 && !(baseTexStr.StartsWith("(") && baseTexStr.EndsWith(")")))
                    baseTexStr = baseTexStr.SurroundWithParas();
            }

            return baseTexStr + "^{" + powerTexStr + "}";
        }
    }
}