using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using System;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Term
{
    internal class SimpleFraction : TermExtension
    {
        private AlgebraTerm _den;
        private AlgebraTerm _num;

        public AlgebraTerm GetDen()
        {
            return _den;
        }

        public ExComp GetDenEx()
        {
            return _den.RemoveRedundancies();
        }

        public AlgebraTerm GetNum()
        {
            return _num;
        }

        public ExComp GetNumEx()
        {
            return _num.RemoveRedundancies();
        }

        public ExComp GetReciprocal()
        {
            return Operators.DivOp.StaticCombine(GetDen(), GetNum());
        }

        /// <summary>
        /// Doesn't allow zero. Stricly single grouped terms with a numerator and denominator.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public bool HarshInit(AlgebraTerm term)
        {
            if (!term.ContainsOnlyFractions() || term.GetGroupCount() != 1)
                return false;
            return Init(term);
        }

        public override bool Init(AlgebraTerm term)
        {
            if (term == null)
                throw new ArgumentException();
            if (term.IsZero())
            {
                _num = Number.GetZero().ToAlgTerm();
                _den = Number.GetZero().ToAlgTerm();

                return true;
            }
            term = term.RemoveRedundancies().ToAlgTerm();

            if (term.GetTermCount() == 1)
            {
                if (term is PowerFunction && (term as PowerFunction).GetPower().IsEqualTo(Number.GetNegOne()))
                {
                    _num = Number.GetOne().ToAlgTerm();
                    _den = (term as PowerFunction).GetBase().ToAlgTerm();
                }
                else
                {
                    _num = term.ToAlgTerm();
                    _den = Number.GetOne().ToAlgTerm();
                }

                return true;
            }

            if (!term.ContainsOnlyFractions())
                return false;

            if (term.GetGroupCount() != 1)
                return false;

            AlgebraTerm[] numDen = term.GetNumDenFrac();
            if (numDen == null)
                return false;

            _num = numDen[0];
            _den = numDen[1];

            return true;
        }

        public bool IsDenOne()
        {
            return Number.GetOne().IsEqualTo(GetDenEx());
        }

        public bool IsSimpleUnitCircleAngle(out Number num, out Number den, bool handleNegs = true)
        {
            num = null;
            den = null;

            if (GetNumEx() is Number && Number.OpEqual((GetNumEx() as Number), 0.0))
            {
                num = Number.GetZero();
                den = Number.GetZero();
                return true;
            }

            if (!(GetDenEx() is Number))
                return false;

            if (!GetNum().Contains(Constant.ParseConstant(@"pi")))
                return false;

            System.Collections.Generic.List<ExComp[]> numGroups = GetNum().GetGroupsNoOps();
            if (numGroups.Count != 1)
                return false;
            ExComp[] numGroup = numGroups[0];

            if (numGroup.Length == 2)
            {
                ExComp first = numGroup[0];
                ExComp second = numGroup[1];

                ExComp piConstant = first is Constant ? first : second;
                ExComp otherEx = first is Constant ? second : first;

                if (!(otherEx is Number))
                    return false;

                num = otherEx as Number;
            }
            else if (numGroup.Length == 1)
                num = Number.GetOne();
            else
                return false;

            den = GetDenEx() as Number;

            if (!num.IsRealInteger() || !den.IsRealInteger())
                return false;

            Number doubleDen = Number.OpMul(den, 2.0);
            bool isNeg = false;
            if (Number.OpLT(num, 0.0))
            {
                num = Number.OpMul(num, -1.0);
                isNeg = true;
            }
            if (Number.OpLE(doubleDen, num))
            {
                num = Number.OpMod(num, doubleDen);
                if (Number.OpEqual(num, 0.0))
                    den = new Number(0.0);
            }

            if (isNeg && handleNegs)
            {
                Number numSub = Number.OpMul(den, 2.0);
                num = Number.OpSub(numSub, num);
            }

            return true;
        }

        public bool LooseInit(AlgebraTerm term)
        {
            if (term == null)
                return false;
            if (term.IsZero())
            {
                _num = Number.GetZero().ToAlgTerm();
                _den = Number.GetZero().ToAlgTerm();

                return true;
            }

            if (term.ContainsOnlyFractions())
            {
                if (term.GetGroupCount() == 1)
                {
                    AlgebraTerm[] numDen = term.GetNumDenFrac();
                    if (numDen != null)
                    {
                        _num = numDen[0];
                        _den = numDen[1];
                        return true;
                    }
                }
            }

            if (term.GetGroupCount() == 1)
            {
                _num = term.ToAlgTerm();
                _den = Number.GetOne().ToAlgTerm();

                return true;
            }

            return false;
        }
    }
}