using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg;

using System.Linq;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus
{
    internal class Derivative : AppliedFunction
    {
        private const int MAX_DERIV = 7;

        private AlgebraComp _derivOf = null;
        private bool _isDefined = true;
        private bool _isPartial = false;
        private ExComp _order = Number.One;
        private AlgebraComp _withRespectTo;
        private string ca_derivSymb = null;
        private AlgebraComp ca_impDeriv = null;

        /// <summary>
        /// Used for notation like f'(2) where a value is to be 
        /// evaluated after the derivative has been taken.
        /// No input value is null.
        /// </summary>
        private ExComp _inputVal = null;

        public Derivative(ExComp innerEx)
            : base(innerEx, FunctionType.Derivative, typeof(Derivative))
        {
        }

        public AlgebraComp DerivOf
        {
            get { return _derivOf; }
        }

        public bool IsOrderOne
        {
            get
            {
                return OrderInt == 1;
            }
        }

        public string NotationIden
        {
            get
            {
                return _isPartial ? "\\partial" : "d";
            }
        }

        public int OrderInt
        {
            get
            {
                if (!(_order is Number))
                    return -1;
                Number num = _order as Number;
                if (!num.IsRealInteger())
                    return -1;

                return (int)num.RealComp;
            }
        }

        public AlgebraComp WithRespectTo
        {
            get { return _withRespectTo; }
            set
            {
                _withRespectTo = value;
            }
        }

        public bool IsPartial
        {
            get { return _isPartial; }
        }

        public static Derivative ConstructDeriv(ExComp innerEx, AlgebraComp withRespect, AlgebraComp derivOf)
        {
            return ConstructDeriv(derivOf, innerEx, null, withRespect, Number.One);
        }

        public static Derivative ConstructDeriv(ExComp innerEx, AlgebraComp withRespect, AlgebraComp derivOf, ExComp order)
        {
            return ConstructDeriv(derivOf, innerEx, null, withRespect, order);
        }

        public static Derivative ConstructDeriv(AlgebraComp funcIden, ExComp inputVal, ExComp order)
        {
            return ConstructDeriv(funcIden, Number.Zero, inputVal, null, order);
        }

        public static Derivative ConstructDeriv(AlgebraComp derivOf, ExComp innerEx, ExComp inputVal, AlgebraComp withRespect, ExComp order)
        {
            Derivative deriv = new Derivative(innerEx);
            deriv._withRespectTo = withRespect;
            deriv._derivOf = derivOf;
            deriv._order = order;
            deriv._inputVal = inputVal;

            return deriv;
        }

        public static Derivative Parse(string function, string withRespectTo, ExComp order, bool isPartial,
            ref TermType.EvalData pEvalData, bool mustBeSingleVar = false)
        {
            if (withRespectTo != null && withRespectTo.Length == 0)
                return null;

            AlgebraComp respectToCmp = withRespectTo == null ? null : new AlgebraComp(withRespectTo);

            Derivative deriv;
            deriv = new Derivative(Number.Zero);
            deriv._isDefined = false;
            deriv._derivOf = new AlgebraComp(function);

            deriv.WithRespectTo = respectToCmp;
            deriv._order = order;
            deriv._isPartial = isPartial;

            return deriv;
        }

        public static Derivative Parse(string withRespectTo, ExComp inner, ExComp order, bool isPartial)
        {
            if (withRespectTo != null && withRespectTo.Length == 0)
                return null;

            Derivative deriv = new Derivative(inner);
            deriv.WithRespectTo = withRespectTo == null ? null : new AlgebraComp(withRespectTo);
            deriv._order = order;
            deriv._isPartial = isPartial;

            return deriv;
        }

        public static ExComp TakeDeriv(ExComp term, AlgebraComp withRespectTo, ref TermType.EvalData pEvalData, bool isPartial = false, bool isFuncDeriv = false)
        {
            if ((term is AlgebraComp || term is FunctionDefinition) && !term.IsEqualTo(withRespectTo) && isFuncDeriv)
                return ConstructImplicitDerivAgCmp(term, withRespectTo, isPartial);

            Derivative deriv = ConstructDeriv(term, withRespectTo, null);
            deriv._isPartial = isPartial;

            ExComp eval = deriv.Evaluate(false, ref pEvalData);
            return eval;
        }

        public ExComp GetDerivOfFunc(FunctionDefinition funcDef, ExComp def)
        {
            if (_derivOf == null || !funcDef.Iden.IsEqualTo(_derivOf))
                return null;

            if (!funcDef.HasValidInputArgs)
                return null;

            if (_withRespectTo == null)
            {
                if (funcDef.InputArgCount != 1)
                    return null;
            }
            else if (_isPartial)
            {
                bool contains = false;
                for (int i = 0; i < funcDef.InputArgCount; ++i)
                {
                    if (_withRespectTo.IsEqualTo(funcDef.InputArgs[i]))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                    return null;
            }
            else if (!_isPartial)
            {
                if (funcDef.InputArgCount != 1)
                    return null;

                if (!_withRespectTo.IsEqualTo(funcDef.InputArgs[0]))
                    return null;
            }

            return Derivative.ConstructDeriv(_inputVal == null ? null : _derivOf, def, _inputVal, _withRespectTo == null ? funcDef.InputArgs[0] : _withRespectTo, _order);
        }

        public override ExComp Clone()
        {
            Derivative deriv = new Derivative(InnerEx.Clone());
            deriv._withRespectTo = this._withRespectTo == null ? null : (AlgebraComp)this._withRespectTo.Clone();
            deriv._order = this._order.Clone();
            deriv._derivOf = this._derivOf == null ? null : (AlgebraComp)this._derivOf.Clone();
            deriv._isDefined = this._isDefined;
            deriv._isPartial = this._isPartial;
            deriv._inputVal = this._inputVal == null ? null : _inputVal.Clone();
            return deriv;
        }

        public AlgebraComp ConstructImplicitDerivAgCmp()
        {
            if (ca_impDeriv == null)
                ca_impDeriv = ConstructImplicitDerivAgCmp(_derivOf, _withRespectTo);
            return ca_impDeriv;
        }

        public ExComp GetOrder()
        {
            return _order;
        }

        public override bool Contains(AlgebraComp varFor)
        {
            return _order.ToAlgTerm().Contains(varFor) || base.Contains(varFor);
        }

        private static AlgebraComp ConstructImplicitDerivAgCmp(ExComp derivOf, ExComp withRespectTo, bool isPartial = false)
        {
            string iden = isPartial ? "\\partial" : "d";
            return new AlgebraComp("(" + iden + derivOf.ToDispString() + ")/(" + iden + withRespectTo.ToDispString() + ")");
        }

        public override ExComp CancelWith(ExComp innerEx, ref TermType.EvalData pEvalData)
        {
            if (!(_order is Number && (_order as Number).IsRealInteger()))
                return null;

            int order = (int)(_order as Number).RealComp;

            if (innerEx is Integral && _derivOf == null && order == 1)
            {
                Integral finalInt = innerEx as Integral;
                if (finalInt.DVar.IsEqualTo(_withRespectTo) && !finalInt.IsDefinite)
                {
                    pEvalData.WorkMgr.FromSides(this, null, "The derivative and the integral cancel.");
                    return finalInt.InnerTerm;
                }
                else if (finalInt.IsDefinite)
                {
                    bool upperContains = finalInt.UpperLimitTerm.ToAlgTerm().Contains(_withRespectTo);
                    bool lowerContains = finalInt.LowerLimitTerm.ToAlgTerm().Contains(_withRespectTo);
                    if (upperContains || lowerContains)
                    {
                        pEvalData.WorkMgr.FromSides(this, null, "Use the fundemental theorem of calculus.");
                        // Fundemental Theorem of calculus should be applied here.
                        Integral[] ints;
                        if (upperContains && lowerContains)
                        {
                            AlgebraComp tmpBoundryVar = new AlgebraComp("a");
                            Integral otherInt = Integral.ConstructIntegral(finalInt.InnerTerm, finalInt.DVar, finalInt.LowerLimit, tmpBoundryVar);
                            finalInt.LowerLimit = tmpBoundryVar;

                            pEvalData.WorkMgr.FromFormatted(ca_derivSymb + "[" + finalInt.FinalToDispStr() + "+" + otherInt.FinalToDispStr() + "]", "Split the integral");
                            ExComp tmp = otherInt.UpperLimit;
                            otherInt.UpperLimit = otherInt.LowerLimit;
                            otherInt.LowerLimit = tmp;
                            pEvalData.WorkMgr.FromFormatted(ca_derivSymb + "[" + finalInt.FinalToDispStr() + "-" + otherInt.FinalToDispStr() + "]", "Switch the integral bounds");

                            ints = new Integral[] { finalInt, otherInt };
                        }
                        else if (lowerContains)
                        {
                            Integral tmpInt = Integral.ConstructIntegral(MulOp.Negate(finalInt.InnerEx), finalInt.DVar, finalInt.UpperLimit, finalInt.LowerLimit);
                            pEvalData.WorkMgr.FromSides(tmpInt, null, "Switch the integral bounds.");
                            ints = new Integral[] { tmpInt };
                        }
                        else
                            ints = new Integral[] { finalInt };


                        ExComp[] modified = new ExComp[ints.Length];
                        Derivative[] chainRules = new Derivative[ints.Length];
                        for (int i = 0; i < ints.Length; ++i)
                        {
                            modified[i] = ints[i].InnerTerm.Substitute(ints[i].DVar, ints[i].UpperLimit);
                            chainRules[i] = ints[i].UpperLimit.IsEqualTo(_withRespectTo) ? null : Derivative.ConstructDeriv(ints[i].UpperLimit, _withRespectTo, null);
                        }

                        string dispStr = "";
                        bool useChainRule = false;
                        for (int i = 0; i < modified.Length; ++i)
                        {
                            if (chainRules[i] != null)
                            {
                                useChainRule = true;
                                dispStr += chainRules[i].FinalToDispStr();
                                dispStr += "(";
                            }

                            dispStr += WorkMgr.ToDisp(modified[i]);

                            if (chainRules[i] != null)
                            {
                                dispStr += ")";
                            }
                        }

                        pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + dispStr + WorkMgr.EDM, "Substitute in" + (useChainRule ? " and use the chain rule." : "."));

                        ExComp[] chainRuled = new ExComp[chainRules.Length];
                        dispStr = "";
                        for (int i = 0; i < chainRuled.Length; ++i)
                        {
                            chainRuled[i] = chainRules[i] == null ? null : chainRules[i].Evaluate(false, ref pEvalData);
                            if (chainRuled[i] != null)
                            {
                                dispStr += WorkMgr.ToDisp(chainRuled[i]);
                                dispStr += "(";
                            }

                            dispStr += WorkMgr.ToDisp(modified[i]);

                            if (chainRuled[i] != null)
                                dispStr += ")";
                        }

                        if (useChainRule)
                            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + dispStr + WorkMgr.EDM);

                        ExComp totalTerm = new AlgebraTerm();
                        for (int i = 0; i < chainRuled.Length; ++i)
                        {
                            totalTerm = AddOp.StaticCombine(totalTerm, chainRuled[i] == null ? modified[i] : (MulOp.StaticCombine(modified[i], chainRuled[i])));
                        }

                        if (useChainRule)
                            pEvalData.WorkMgr.FromSides(totalTerm, null);

                        return totalTerm;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Harsh evaluations have no importance here as derivatives are always symbolic.
        /// </summary>
        /// <param name="harshEval">Value doesn't matter.</param>
        /// <returns></returns>
        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            if (!_isDefined)
                return this;

            if (_withRespectTo == null)
                return this;

            if (!(_order is Number && (_order as Number).IsRealInteger()))
                return this;

            int order = (int)(_order as Number).RealComp;

            if (order > MAX_DERIV)
                return this;

            if (_isPartial)
                pEvalData.AttemptSetInputType(TermType.InputType.PartialDerivative);

            ca_derivSymb = "d/(d" + _withRespectTo.ToDispString() + ")";

            ExComp final = InnerEx;

            if (final is ExVector)
            {
                ExVector vec = final as ExVector;
                // Take the derivative of each component separately.
                ExVector derivVec = vec.CreateEmptyBody();

                // Work steps should go here.

                for (int i = 0; i < vec.Length; ++i)
                {
                    ExComp deriv = TakeDerivativeOf(vec.Get(i), ref pEvalData);
                    derivVec.Set(i, deriv);
                }

                // Sub in the point and evaluate.
                if (_inputVal != null)
                {
                    AlgebraTerm subbedTerm = derivVec.Substitute(_withRespectTo, _inputVal);
                    ExComp derivVecSimp = Simplifier.Simplify(subbedTerm, ref pEvalData);
                    return derivVecSimp;
                }

                return derivVec;
            }
            else if (final is ExMatrix)
            {
                // Don't know if this works.
                return Number.Undefined;
            }

            pEvalData.WorkMgr.FromFormatted("`{0}`", "Find the " + (_order).ToString() + MathHelper.GetCountingPrefix(order)
                + " derivative of the above.", InnerEx);
            for (int i = 0; i < order; ++i)
            {
                ExComp tmp = TakeDerivativeOf(final, ref pEvalData);
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]={1}`", "The final " + (i + 1).ToString() +
                    MathHelper.GetCountingPrefix(i + 1) + " derivative.", final, tmp);
                final = tmp;
            }

            if (_inputVal != null)
            {
                final = final.ToAlgTerm().Substitute(_withRespectTo, _inputVal);
                AlgebraTerm termWrapper = new AlgebraTerm(final);
                final = Simplifier.Simplify(termWrapper, ref pEvalData);
            }

            return final;
        }

        public override string FinalToAsciiKeepFormatting()
        {
            string orderStr = IsOrderOne ? "" : "^{" + _order.ToAsciiString() + "}";
            string followingStr = null;
            if (_order is Number && (_order as Number).IsRealInteger())
            {
                int order = (int)(_order as Number).RealComp;
                if (order < 3)
                {
                    followingStr = "";
                    // Use prime notation.
                    for (int i = 0; i < order; ++i)
                        followingStr += "'";
                }
            }

            if (_withRespectTo == null)
            {
                if (_inputVal == null)
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr);
                else
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAlgTerm().FinalToAsciiKeepFormatting() + ")";
            }

            if (!_isDefined)
            {
                if (_inputVal == null)
                    return "(\\frac{" + NotationIden + orderStr + _derivOf.ToAsciiString() + "}{" + NotationIden + _withRespectTo.ToAsciiString() + orderStr + "})";
                else
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAlgTerm().FinalToAsciiKeepFormatting() + ")";
            }
            return "(\\frac{" + NotationIden + orderStr + "}{" + NotationIden + _withRespectTo.ToAsciiString() + orderStr + "}[" +
                InnerEx.ToAlgTerm().FinalToAsciiKeepFormatting() + "])";
        }

        public override string FinalToAsciiString()
        {
            string orderStr = IsOrderOne ? "" : "^{" + _order.ToAsciiString() + "}";
            string followingStr = null;
            if (_order is Number && (_order as Number).IsRealInteger())
            {
                int order = (int)(_order as Number).RealComp;
                if (order < 3)
                {
                    followingStr = "";
                    // Use prime notation.
                    for (int i = 0; i < order; ++i)
                        followingStr += "'";
                }
            }

            if (_withRespectTo == null)
            {
                if (_inputVal == null)
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr);
                else
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAlgTerm().FinalToAsciiString() + ")";
            }

            if (!_isDefined || _inputVal != null)
            {
                if (_inputVal == null)
                    return "(\\frac{" + NotationIden + orderStr + _derivOf.ToAsciiString() + "}{" + NotationIden + _withRespectTo.ToAsciiString() + orderStr + "})";
                else
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAlgTerm().FinalToAsciiString() + ")";
            }
            return "(\\frac{" + NotationIden + orderStr + "}{" + NotationIden + _withRespectTo.ToAsciiString() + orderStr + "}[" +
                InnerEx.ToAlgTerm().FinalToAsciiString() + "])";
        }

        public override string FinalToTexKeepFormatting()
        {
            string orderStr = IsOrderOne ? "" : "^{" + _order.ToTexString() + "}";
            string followingStr = null;
            if (_order is Number && (_order as Number).IsRealInteger())
            {
                int order = (int)(_order as Number).RealComp;
                if (order < 3)
                {
                    followingStr = "";
                    // Use prime notation.
                    for (int i = 0; i < order; ++i)
                        followingStr += "'";
                }
            }

            if (_withRespectTo == null)
            {
                if (_inputVal == null)
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr);
                else
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAlgTerm().FinalToTexKeepFormatting() + ")";
            }

            if (!_isDefined || _inputVal != null)
            {
                if (_inputVal == null)
                    return "(\\frac{" + NotationIden + orderStr + _derivOf.ToTexString() + "}{" + NotationIden + _withRespectTo.ToTexString() + orderStr + "})";
                else
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAlgTerm().FinalToTexKeepFormatting() + ")";
            }
            return "(\\frac{" + NotationIden + orderStr + "}{" + NotationIden + _withRespectTo.ToTexString() + orderStr + "}[" +
                InnerEx.ToAlgTerm().FinalToTexKeepFormatting() + "])";
        }

        public override string FinalToTexString()
        {
            string orderStr = IsOrderOne ? "" : "^{" + _order.ToTexString() + "}";
            string followingStr = null;
            if (_order is Number && (_order as Number).IsRealInteger())
            {
                int order = (int)(_order as Number).RealComp;
                if (order < 3)
                {
                    followingStr = "";
                    // Use prime notation.
                    for (int i = 0; i < order; ++i)
                        followingStr += "'";
                }
            }

            if (_withRespectTo == null)
            {
                if (_inputVal == null)
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr);
                else
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAlgTerm().FinalToTexString() + ")";
            }

            if (!_isDefined || _inputVal != null)
            {
                if (_inputVal == null)
                    return "(\\frac{" + NotationIden + orderStr + _derivOf.ToTexString() + "}{" + NotationIden + _withRespectTo.ToTexString() + orderStr + "})";
                else
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAlgTerm().FinalToTexString() + ")";
            }
            return "(\\frac{" + NotationIden + orderStr + "}{" + NotationIden + _withRespectTo.ToTexString() + orderStr + "}[" +
                InnerEx.ToAlgTerm().FinalToTexString() + "])";
        }

        public override bool IsEqualTo(ExComp ex)
        {
            if (ex is Derivative)
            {
                Derivative deriv = ex as Derivative;

                return deriv.InnerEx.IsEqualTo(InnerEx) &&
                    ((deriv._withRespectTo != null && deriv._withRespectTo.IsEqualTo(this._withRespectTo)) ||
                    (deriv._withRespectTo == null && this._withRespectTo == null)) &&
                    ((deriv._derivOf != null && deriv._derivOf.IsEqualTo(this._derivOf)) ||
                    (deriv._derivOf == null && this._derivOf == null)) &&
                    (deriv._isPartial == this._isPartial) &&
                    deriv._order == _order;
            }

            return false;
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn)
        {
            Derivative deriv = new Derivative(InnerTerm.Substitute(subOut, subIn));
            deriv._derivOf = this._derivOf;
            deriv._isDefined = this._isDefined;
            deriv._isPartial = this._isPartial;
            deriv._inputVal = this._inputVal;
            if (this._order.IsEqualTo(subOut) && subIn is AlgebraComp || subIn is Number)
                deriv._order = subIn;
            else
                deriv._order = this._order;
            deriv._withRespectTo = this._withRespectTo;
            deriv.ca_derivSymb = this.ca_derivSymb;
            deriv.ca_impDeriv = this.ca_impDeriv;

            return deriv;
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn, ref bool success)
        {
            Derivative deriv = new Derivative(InnerTerm.Substitute(subOut, subIn, ref success));
            deriv._derivOf = this._derivOf;
            deriv._isDefined = this._isDefined;
            deriv._isPartial = this._isPartial;
            if (this._order.IsEqualTo(subOut) && subIn is AlgebraComp || subIn is Number)
            {
                deriv._order = subIn;
                success = true;
            }
            else
                deriv._order = this._order;
            deriv._withRespectTo = this._withRespectTo;
            deriv.ca_derivSymb = this.ca_derivSymb;
            deriv.ca_impDeriv = this.ca_impDeriv;

            return deriv;
        }

        public override string ToAsciiString()
        {
            string orderStr = IsOrderOne ? "" : "^{" + _order.ToAsciiString() + "}";
            string followingStr = null;
            if (_order is Number && (_order as Number).IsRealInteger())
            {
                int order = (int)(_order as Number).RealComp;
                if (order < 3)
                {
                    followingStr = "";
                    // Use prime notation.
                    for (int i = 0; i < order; ++i)
                        followingStr += "'";
                }
            }

            if (_withRespectTo == null)
            {
                if (_inputVal == null)
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr);
                else
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAsciiString() + ")";
            }

            if (!_isDefined || _inputVal != null)
            {
                if (_inputVal == null)
                    return "(\\frac{" + NotationIden + orderStr + _derivOf.ToAsciiString() + "}{" + NotationIden + _withRespectTo.ToAsciiString() + orderStr + "})";
                else
                    return _derivOf.ToAsciiString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToAsciiString() + ")";
            }
            return "(\\frac{" + NotationIden + orderStr + "}{" + NotationIden + _withRespectTo.ToAsciiString() + orderStr + "}[" +
                InnerEx.ToAsciiString() + "])";
        }

        public override string ToJavaScriptString(bool useRad)
        {
            return null;
        }

        public override string ToString()
        {
            return ToTexString();
        }

        public override string ToTexString()
        {
            string orderStr = IsOrderOne ? "" : "^{" + _order.ToTexString() + "}";
            string followingStr = null;
            if (_order is Number && (_order as Number).IsRealInteger())
            {
                int order = (int)(_order as Number).RealComp;
                if (order < 3)
                {
                    followingStr = "";
                    // Use prime notation.
                    for (int i = 0; i < order; ++i)
                        followingStr += "'";
                }
            }

            if (_withRespectTo == null)
            {
                if (_inputVal == null)
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr);
                else
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToTexString() + ")";
            }

            if (!_isDefined || _inputVal != null)
            {
                if (_inputVal == null)
                    return "(\\frac{" + NotationIden + orderStr + _derivOf.ToTexString() + "}{" + NotationIden + _withRespectTo.ToTexString() + orderStr + "})";
                else
                    return _derivOf.ToTexString() + (followingStr == null ? orderStr : followingStr) + "(" + _inputVal.ToTexString() + ")";
            }
            return "(\\frac{" + NotationIden + orderStr + "}{" + NotationIden + _withRespectTo.ToTexString() + orderStr + "}[" +
                InnerEx.ToTexString() + "])";
        }

        protected override AlgebraTerm CreateInstance(params ExComp[] args)
        {
            Derivative deriv = new Derivative(args[0]);
            deriv._withRespectTo = this._withRespectTo == null ? null : (AlgebraComp)this._withRespectTo.Clone();
            deriv._order = this._order.Clone();
            deriv._isDefined = this._isDefined;
            deriv._derivOf = this._derivOf == null ? null : (AlgebraComp)this._derivOf.Clone();
            deriv._isPartial = this._isPartial;
            deriv._inputVal = this._inputVal == null ? null : this._inputVal.Clone();
            return deriv;
        }

        private ExComp ApplyAbsDeriv(AbsValFunction abs, ref TermType.EvalData pEvalData)
        {
            ExComp innerEx = abs.InnerEx;
            bool useChainRule = ShouldApplyChainRule(innerEx);
            if (useChainRule)
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`",
                    "The above is a composition of functions `f(g(" + _withRespectTo.ToDispString() +
                    "))`. Use the chain rule which states `d/(dx)[f(g(x))]=f'(g(x))*g'(x)` First take the derivative of the outside function.", abs);
            }

            pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]=({0})/({1})`", "From the common derivative of the absolute value function.", abs.InnerTerm, abs);

            ExComp deriv = DivOp.StaticCombine(innerEx, abs);

            if (useChainRule)
            {
                pEvalData.AddInputType(TermType.InputAddType.DerivCR);
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`", "Next take the derivative of the inner function.", abs.InnerTerm);
                WorkStep last = pEvalData.WorkMgr.GetLast();

                last.GoDown(ref pEvalData);
                ExComp innerDeriv = TakeDerivativeOf(abs.InnerTerm, ref pEvalData);
                last.GoUp(ref pEvalData);

                pEvalData.WorkMgr.FromFormatted("`{0}`",
                    "According to the chain rule multiply the derivative of the inner function by the derivative of the outer function.",
                    MulOp.StaticWeakCombine(innerDeriv, deriv));

                // Apply the chain rule.
                return MulOp.StaticCombine(innerDeriv, deriv);
            }

            return deriv;
        }

        private ExComp ApplyInvTrigDeriv(InverseTrigFunction invTrigFunc, ref TermType.EvalData pEvalData)
        {
            bool useChainRule = ShouldApplyChainRule(invTrigFunc.InnerEx);
            if (useChainRule)
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`",
                    "The above is a composition of functions `f(g(" + _withRespectTo.ToDispString() +
                    "))`. Use the chain rule which states `d/(dx)[f(g(x))]=f'(g(x))*g'(x)` First take the derivative of the outside function.", invTrigFunc);
            }

            ExComp deriv = invTrigFunc.GetDerivativeOf();
            pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]={1}`",
                "Use the common definition that `d/(dx)[" + invTrigFunc.FuncName + "(x)]=" + invTrigFunc.GetDerivativeOfStr() + "`.", invTrigFunc, deriv);

            pEvalData.AttemptSetInputType(TermType.InputType.DerivInvTrig);

            if (useChainRule)
            {
                pEvalData.AddInputType(TermType.InputAddType.DerivCR);
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`", "Next take the derivative of the inner function.", invTrigFunc.InnerTerm);
                WorkStep last = pEvalData.WorkMgr.GetLast();

                last.GoDown(ref pEvalData);
                // Apply the chain rule.
                ExComp innerDeriv = TakeDerivativeOf(invTrigFunc.InnerTerm, ref pEvalData);
                last.GoUp(ref pEvalData);

                pEvalData.WorkMgr.FromFormatted("`{0}`",
                    "According to the chain rule multiply the derivative of the inner function by the derivative of the outer function.",
                    MulOp.StaticWeakCombine(innerDeriv, deriv));

                return MulOp.StaticCombine(innerDeriv, deriv);
            }

            return deriv;
        }

        private ExComp ApplyLogDeriv(LogFunction log, ref TermType.EvalData pEvalData)
        {
            if (log.Base.ToAlgTerm().Contains(_withRespectTo))
            {
                // Terms in the form log_x(y) cannot have the derivative be taken with respect to x.
                return ConstructDeriv(log, _withRespectTo, _derivOf);
            }

            bool useChainRule = ShouldApplyChainRule(log.InnerEx);
            if (useChainRule)
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`",
                    "The above is a composition of functions `f(g(" + _withRespectTo.ToDispString() +
                    "))`. Use the chain rule which states `d/(dx)[f(g(x))]=f'(g(x))*g'(x)` First take the derivative of the outside function.", log);
            }

            ExComp deriv;
            if (log.Base.IsEqualTo(Constant.E))
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]=1/({1})`",
                    "Using the definition that `d/(dx)[log_b(x)]=1/(ln(b)x)` which can be extended to natural logs to say `d/(dx)[ln(x)]=1/x`",
                    log, log.InnerTerm);
                deriv = DivOp.StaticCombine(Number.One, log.InnerTerm);
            }
            else
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]=1/(ln({2})({1}))`",
                    "Using the definition that `d/(dx)[log_b(x)]=1/(ln(b)x)`",
                    log, log.InnerTerm, log.Base);
                deriv = DivOp.StaticCombine(Number.One, MulOp.StaticWeakCombine(log.InnerTerm, LogFunction.Ln(log.Base)));
            }

            pEvalData.AttemptSetInputType(TermType.InputType.DerivLog);

            if (useChainRule)
            {
                pEvalData.AddInputType(TermType.InputAddType.DerivCR);
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`", "Next take the derivative of the inner function.", log.InnerTerm);
                WorkStep last = pEvalData.WorkMgr.GetLast();

                last.GoDown(ref pEvalData);
                ExComp innerDeriv = TakeDerivativeOf(log.InnerTerm, ref pEvalData);
                last.GoUp(ref pEvalData);

                pEvalData.WorkMgr.FromFormatted("`{0}`",
                    "According to the chain rule multiply the derivative of the inner function by the derivative of the outer function.",
                    MulOp.StaticWeakCombine(innerDeriv, deriv));

                // Apply the chain rule.
                return MulOp.StaticCombine(innerDeriv, deriv);
            }

            return deriv;
        }

        private ExComp ApplyPowBaseDeriv(PowerFunction powFunc, ref TermType.EvalData pEvalData)
        {
            ExComp final = MulOp.StaticCombine(powFunc.Power, LogFunction.Ln(powFunc.Base));
            pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]=({0})*d/(dx)[{1}]`", "This comes from the definition for the derivative of `d/(dx)[x^x]=x^x*d/(dx)[x*ln(x)]`.", powFunc, final);

            return MulOp.StaticCombine(powFunc, TakeDerivativeOf(final, ref pEvalData));
        }

        private ExComp ApplyPower(PowerFunction pfGpCmp, ref TermType.EvalData pEvalData)
        {
            bool powHas = ContainsVarOfInterest(pfGpCmp.Power);
            bool baseHas = ContainsVarOfInterest(pfGpCmp.Base);

            if (powHas && baseHas)
            {
                return ApplyPowBaseDeriv(pfGpCmp, ref pEvalData);
            }
            else if (powHas)
            {
                return ApplyPowerRulePower(pfGpCmp, ref pEvalData);
            }
            else if (baseHas)
            {
                return ApplyPowerRuleBase(pfGpCmp, ref pEvalData);
            }
            else
                return ConstructDeriv(pfGpCmp, _withRespectTo, _derivOf);
        }

        private ExComp ApplyPowerRuleBase(PowerFunction powFunc, ref TermType.EvalData pEvalData)
        {
            if (powFunc.Power.IsEqualTo(Number.NegOne))
            {
                // Don't include the constants which are still contained under the neg one power.
                if (powFunc.Base is AlgebraTerm)
                {
                    List<ExComp[]> gps = (powFunc.Base as AlgebraTerm).GetGroupsNoOps();
                    if (gps.Count == 1)
                    {
                        ExComp[] gp = gps[0];
                        ExComp[] varTo, constTo;
                        gp.GetConstVarTo(out varTo, out constTo, _withRespectTo);
                        if (varTo.Length != 0 && constTo.Length != 0)
                        {
                            AlgebraTerm agConst = AlgebraTerm.FromFraction(Number.One, constTo.ToAlgTerm());
                            AlgebraTerm agVarTo = varTo.ToAlgNoRedunTerm();
                            if (agVarTo is PowerFunction)
                                (agVarTo as PowerFunction).Power = MulOp.Negate((agVarTo as PowerFunction).Power);
                            else
                                agVarTo = new PowerFunction(agVarTo, Number.NegOne);

                            pEvalData.WorkMgr.FromFormatted("`" + agConst.ToDispString() + ca_derivSymb + "[" + agVarTo.FinalToDispStr() + "]`",
                                "Bring out all constants as they will have no effect on the derivative. This comes from the derivative property that `d/(dx)[kf(x)]=k*d/(dx)[f(x)]` the constants will be multiplied back in at the end.");

                            ExComp deriv = TakeDerivativeOf(agVarTo, ref pEvalData);
                            return MulOp.StaticCombine(agConst, deriv);
                        }
                    }
                }
            }

            ExComp term = powFunc.Power.Clone();

            ExComp power = SubOp.StaticCombine(powFunc.Power, Number.One);
            if (power is AlgebraTerm)
                power = (power as AlgebraTerm).CompoundFractions();

            PowerFunction derivPowFunc = new PowerFunction(powFunc.Base, power);
            if (derivPowFunc.Power.ToAlgTerm().IsOne())
                term = MulOp.StaticCombine(term, powFunc.Base);
            else
                term = MulOp.StaticCombine(term, derivPowFunc);

            bool useChainRule = ShouldApplyChainRule(powFunc.Base);
            if (useChainRule)
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`",
                    "The above is a composition of functions `f(g(" + _withRespectTo.ToDispString() +
                    "))`. Use the chain rule which states `d/(dx)[f(g(x))]=f'(g(x))*g'(x)` First take the derivative of the outside function.", powFunc);
            }

            pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]={1}`",
                "Through using the power rule which states `d/(dx)[x^n]=nx^(n-1)`.", powFunc, term);

            pEvalData.AttemptSetInputType(TermType.InputType.DerivPoly);

            if (useChainRule)
            {
                pEvalData.AddInputType(TermType.InputAddType.DerivCR);
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`", "Next take the derivative of the inner function.", powFunc.Base);
                WorkStep last = pEvalData.WorkMgr.GetLast();

                last.GoDown(ref pEvalData);
                // The chain rule has to be applied here.
                ExComp innerDeriv = TakeDerivativeOf(powFunc.Base, ref pEvalData);
                last.GoUp(ref pEvalData);

                pEvalData.WorkMgr.FromFormatted("`{0}`",
                    "According to the chain rule multiply the derivative of the inner function by the derivative of the outer function.",
                    MulOp.StaticWeakCombine(innerDeriv, term));

                return MulOp.StaticCombine(innerDeriv, term);
            }
            return term;
        }

        private ExComp ApplyPowerRulePower(PowerFunction powFunc, ref TermType.EvalData pEvalData)
        {
            bool useChainRule = ShouldApplyChainRule(powFunc.Power);

            if (useChainRule)
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`",
                    "The above is a composition of functions `f(g(" + _withRespectTo.ToDispString() +
                    "))`. Use the chain rule which states `d/(dx)[f(g(x))]=f'(g(x))*g'(x)` First take the derivative of the outside function.", powFunc);
            }

            ExComp deriv;
            if (powFunc.Base is Constant && (powFunc.Base as Constant).Var.Var == "e")
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]={0}`", "The derivative of `e` raised to anything is always the same.", powFunc);
                deriv = powFunc;
            }
            else
            {
                deriv = MulOp.StaticWeakCombine(LogFunction.Ln(powFunc.Base), powFunc);
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]={1}`", "Using the exponent rule which states `d/(dx)[a^x]=ln(a)a^x`", powFunc, deriv);
            }

            pEvalData.AttemptSetInputType(TermType.InputType.DerivExp);

            if (useChainRule)
            {
                pEvalData.AddInputType(TermType.InputAddType.DerivCR);
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`", "Next take the derivative of the inner function.", powFunc.Power);
                WorkStep last = pEvalData.WorkMgr.GetLast();

                last.GoDown(ref pEvalData);
                // Apply chain rule.
                ExComp innerDeriv = TakeDerivativeOf(powFunc.Power, ref pEvalData);
                last.GoUp(ref pEvalData);

                pEvalData.WorkMgr.FromFormatted("`{0}`",
                    "According to the chain rule multiply the derivative of the inner function by the derivative of the outer function.",
                    MulOp.StaticWeakCombine(innerDeriv, deriv));
                return MulOp.StaticCombine(deriv, innerDeriv);
            }

            return deriv;
        }

        private ExComp ApplyTrigDeriv(TrigFunction trigFunc, ref TermType.EvalData pEvalData)
        {
            bool useChainRule = ShouldApplyChainRule(trigFunc.InnerEx);
            if (useChainRule)
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`",
                    "The above is a composition of functions `f(g(" + _withRespectTo.ToDispString() +
                    "))`. Use the chain rule which states `d/(dx)[f(g(x))]=f'(g(x))*g'(x)` First take the derivative of the outside function.", trigFunc);
            }

            ExComp deriv = trigFunc.GetDerivativeOf();
            pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]={1}`",
                "Use the common definition that `d/(dx)[" + trigFunc.FuncName + "(x)]=" + trigFunc.GetDerivativeOfStr() + "`.", trigFunc, deriv);

            pEvalData.AttemptSetInputType(TermType.InputType.DerivTrig);

            if (useChainRule)
            {
                pEvalData.AddInputType(TermType.InputAddType.DerivCR);
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`", "Next take the derivative of the inner function.", trigFunc.InnerTerm);
                WorkStep lastWorkStep = pEvalData.WorkMgr.GetLast();

                lastWorkStep.GoDown(ref pEvalData);
                ExComp innerDeriv = TakeDerivativeOf(trigFunc.InnerTerm, ref pEvalData);
                lastWorkStep.GoUp(ref pEvalData);

                pEvalData.WorkMgr.FromFormatted("`{0}`",
                    "According to the chain rule multiply the derivative of the inner function by the derivative of the outer function.",
                    MulOp.StaticWeakCombine(innerDeriv, deriv));

                // Apply the chain rule.
                return MulOp.StaticCombine(innerDeriv, deriv);
            }

            return deriv;
        }

        private bool ContainsVarOfInterest(ExComp ex)
        {
            AlgebraTerm term = ex.ToAlgTerm();
            return term.Contains(_withRespectTo) || (_derivOf != null && term.Contains(_derivOf));
        }

        private bool ShouldApplyChainRule(ExComp ex)
        {
            return !ex.IsEqualTo(_withRespectTo);
        }

        private ExComp TakeDerivativeOf(ExComp ex, ref TermType.EvalData pEvalData)
        {
            if (ex is AlgebraTerm)
                ex = (ex as AlgebraTerm).RemoveRedundancies();

            if (ca_derivSymb == null)
            {
                ca_derivSymb = "d/(" + NotationIden + _withRespectTo.ToDispString() + ")";
            }

            pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`",
                _derivOf == null ? "Take the derivative of this expression with respect to `" + _withRespectTo.ToDispString() + "`." :
                "Find the derivative `" + ConstructImplicitDerivAgCmp().ToDispString() + "`", ex);

            if (!ContainsVarOfInterest(ex))
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]=0`", "The entire term is constant therefore the derivative equals `0`", ex);
                return Number.Zero;
            }

            if (ex is AlgebraComp)
            {
                if (_withRespectTo.IsEqualTo(ex))
                {
                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]=1`",
                        "Use the power rule.", ex);
                    return Number.One;
                }
                else if (_derivOf != null && _derivOf.IsEqualTo(ex))
                {
                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]=(d{0})/(d{1})`",
                        "As `{1}` is a function of `{0}` rather than the derivative being `(" +
                        NotationIden + "{1})/(" + NotationIden + "{1})=1` it is `(" + NotationIden +
                        "{0})/(" + NotationIden + "{1})`", _derivOf, _withRespectTo);
                    return ConstructImplicitDerivAgCmp();
                }
            }

            if (ex is PowerFunction)
            {
                return ApplyPower(ex as PowerFunction, ref pEvalData);
            }
            else if (ex is AbsValFunction)
            {
                return ApplyAbsDeriv(ex as AbsValFunction, ref pEvalData);
            }
            else if (ex is TrigFunction)
            {
                return ApplyTrigDeriv(ex as TrigFunction, ref pEvalData);
            }
            else if (ex is InverseTrigFunction)
            {
                return ApplyInvTrigDeriv(ex as InverseTrigFunction, ref pEvalData);
            }
            else if (ex is LogFunction)
            {
                return ApplyLogDeriv(ex as LogFunction, ref pEvalData);
            }
            else if (ex is AlgebraFunction)
            {
                // An unaccounted for function.
                return ConstructDeriv(ex, _withRespectTo, _derivOf);
            }
            else if (ex is AlgebraTerm)
            {
                // Split it up by the addition signs.
                AlgebraTerm term = ex as AlgebraTerm;
                AlgebraTerm[] numDen = term.GetNumDenFrac();
                if (numDen != null)
                {
                    ExComp numEx = numDen[0].RemoveRedundancies();
                    ExComp denEx = numDen[1].RemoveRedundancies();

                    if (Number.One.IsEqualTo(numEx) && denEx is PowerFunction)
                    {
                        PowerFunction pfDen = denEx as PowerFunction;
                        pfDen.Power = MulOp.Negate(pfDen.Power);

                        return ApplyPower(pfDen, ref pEvalData);
                    }
                }

                List<ExComp[]> gps = term.GetGroupsNoOps();
                AlgebraTerm final = new AlgebraTerm();

                if (gps.Count != 1 && pEvalData.WorkMgr.AllowWork)
                {
                    string indvDerivs = "";
                    for (int i = 0; i < gps.Count; ++i)
                    {
                        indvDerivs += ca_derivSymb + "[" + gps[i].FinalToMathAsciiString() + "]";
                        if (i != gps.Count - 1)
                            indvDerivs += "+";
                    }

                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[" + WorkMgr.ToDisp(ex) + "]=" + indvDerivs + "`",
                        "Split the derivative up using the property of derivatives that `d/(dx)[f(x)+g(x)]=f'(x)+g'(x)`");
                }

                for (int i = 0; i < gps.Count; ++i)
                {
                    ExComp deriv = TakeDerivativeOfGp(gps[i], ref pEvalData);
                    if (deriv.ToAlgTerm().IsZero())
                        continue;
                    final.Add(deriv);
                    if (i != gps.Count - 1)
                        final.Add(new AddOp());
                }

                return final;
            }

            // Whatever this is, the derivative of it can't be taken.
            return ConstructDeriv(ex, _withRespectTo, _derivOf);
        }

        private ExComp TakeDerivativeOfGp(ExComp[] gp, ref TermType.EvalData pEvalData)
        {
            if (_withRespectTo == null)
                return Derivative.ConstructDeriv(gp.ToAlgTerm(), _withRespectTo, _derivOf);

            ExComp[] varTo, constTo;
            if (_derivOf == null)
                gp.GetConstVarTo(out varTo, out constTo, _withRespectTo);
            else
                gp.GetConstVarTo(out varTo, out constTo, _withRespectTo, _derivOf);

            if (constTo.Length == 1 && constTo[0].IsEqualTo(Number.One))
            {
                ExComp[] empty = { };
                constTo = empty;
            }

            if (varTo.Length == 0)
            {
                pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[" + gp.FinalToMathAsciiString() + "]=0`", "The entire term is constant therefore the derivative equals `0`");
                return Number.Zero;
            }

            string varToStr = varTo.ToAlgTerm().ToAsciiString();

            ExComp derivTerm = null;

            if (constTo.Length != 0)
            {
                pEvalData.WorkMgr.FromFormatted("`" + constTo.ToAlgTerm().ToDispString() + ca_derivSymb + "[" + varToStr + "]`",
                    "Bring out all constants as they will have no effect on the derivative. This comes from the derivative property that `d/(dx)[kf(x)]=k*d/(dx)[f(x)]` the constants will be multiplied back in at the end.");
            }

            if (varTo.Length == 1)
            {
                ExComp gpCmp = varTo[0];

                derivTerm = TakeDerivativeOf(gpCmp, ref pEvalData);
            }
            else
            {
                ExComp[] num = varTo.GetNumerator();
                ExComp[] den = varTo.GetDenominator();

                if (den != null && den.Length > 0)
                {
                    ExComp numEx = num.ToAlgTerm();
                    ExComp denEx = den.ToAlgTerm();

                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[" + varToStr + "]`",
                        "As the above is a fraction use the quotient rule which states `d/(dx)[u/v]=(u'v-uv')/(v^2)`. In this case `u=" + numEx.ToDispString() + "`, `v=" + denEx.ToDispString() + "`");

                    // Use the quotient rule.
                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[" + varToStr + "]`",
                        "First find the derivative of the numerator.");
                    WorkStep last0 = pEvalData.WorkMgr.GetLast();

                    last0.GoDown(ref pEvalData);
                    ExComp numDeriv = TakeDerivativeOfGp(num, ref pEvalData);
                    last0.GoUp(ref pEvalData);

                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[" + varToStr + "]`",
                        "Find the derivative of the denominator.");
                    WorkStep last1 = pEvalData.WorkMgr.GetLast();

                    last1.GoDown(ref pEvalData);
                    ExComp denDeriv = TakeDerivativeOfGp(den, ref pEvalData);
                    last1.GoUp(ref pEvalData);

                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[" + varToStr + "]=(({0})({1})-({2})({3}))/(({1})^2)`",
                        "Plug the values back into the equation for the quotient rule `d/(dx)[u/v]=(u'v-uv')/(v^2)`. In the above case `u={2}`, `u'={0}`, `v={1}`, `v'={3}`", numDeriv, denEx, numEx, denDeriv);

                    ExComp tmpMul0 = MulOp.StaticCombine(numDeriv, denEx.Clone());
                    ExComp tmpMul1 = MulOp.StaticCombine(denDeriv.Clone(), numEx);

                    ExComp tmpNum = SubOp.StaticCombine(tmpMul0, tmpMul1);
                    ExComp tmpDen = PowOp.StaticCombine(denEx, new Number(2.0));

                    derivTerm = DivOp.StaticCombine(tmpNum, tmpDen);
                }
                else
                {
                    ExComp u = num[0];
                    ExComp v = num.ToList().GetRange(1, num.Length - 1).ToArray().ToAlgTerm();

                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[" + num.ToAsciiString() + "]`",
                        "Apply the product rule which states `d/(dx)[u*v]=u'v+uv'` in this case `u={0}`, `v={1}`", u, v);

                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`", "Calculate `u'` for the product rule.", u);
                    WorkStep last0 = pEvalData.WorkMgr.GetLast();

                    last0.GoDown(ref pEvalData);
                    ExComp uDeriv = TakeDerivativeOf(u, ref pEvalData);
                    last0.GoUp(ref pEvalData);

                    pEvalData.WorkMgr.FromFormatted("`" + ca_derivSymb + "[{0}]`", "Calculate `v'` for the product rule.", v);
                    WorkStep last1 = pEvalData.WorkMgr.GetLast();

                    last1.GoDown(ref pEvalData);
                    ExComp vDeriv = TakeDerivativeOf(v, ref pEvalData);
                    last1.GoUp(ref pEvalData);

                    derivTerm = AddOp.StaticCombine(MulOp.StaticCombine(uDeriv, v), MulOp.StaticCombine(vDeriv, u));
                }
            }

            if (derivTerm == null)
                return Derivative.ConstructDeriv(gp.ToAlgTerm(), _withRespectTo, _derivOf);

            if (constTo.Length == 0)
                return derivTerm;
            ExComp constToEx = constTo.ToAlgTerm();

            pEvalData.WorkMgr.FromFormatted("`{0}*" + ca_derivSymb + "[" + varTo.ToAsciiString() + "]={0}*{1}`", "Multiply back in the constants.", constToEx, derivTerm);

            return MulOp.StaticCombine(constToEx, derivTerm);
        }
    }
}