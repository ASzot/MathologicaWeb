using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions
{
    internal class AbsValFunction : AppliedFunction
    {
        public AbsValFunction(ExComp ex)
            : base(ex, FunctionType.AbsoluteValue, typeof(AbsValFunction))
        {
        }

        public static ExComp MakePositive(ExComp ex)
        {
            if (ex is Number)
                return Number.Abs(ex as Number);
            else if (ex is AlgebraTerm)
            {
                AlgebraTerm term = ex as AlgebraTerm;
                for (int i = 0; i < term.TermCount; ++i)
                {
                    if (term[i] is Number)
                    {
                        term[i] = Number.Abs(term[i] as Number);
                    }
                    if (term[i] is AlgebraTerm)
                    {
                        term[i] = MakePositive(term[i] as AlgebraTerm);
                    }
                }

                return term;
            }

            return ex;
        }

        public static ExComp[] MakePositive(ExComp[] group)
        {
            for (int i = 0; i < group.Length; ++i)
            {
                if (group[i] is Number && (group[i] as Number) < 0.0)
                {
                    group[i] = (group[i] as Number) * -1.0;
                }
            }

            return group;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            ExComp innerEx = InnerEx;

            if (Number.IsUndef(innerEx))
                return Number.Undefined;
            if (innerEx is Number)
            {
                Number absInner = Number.Abs(innerEx as Number);
                return absInner;
            }
            else if (innerEx is Equation.Structural.LinearAlg.ExVector)
            {
                // Vector magnitude.
                return (innerEx as Equation.Structural.LinearAlg.ExVector).GetVecLength();
            }
            else if (innerEx is AlgebraTerm && !(innerEx is AlgebraFunction))
            {
                AlgebraTerm innerTerm = innerEx as AlgebraTerm;
                List<ExComp[]> groups = innerTerm.GetGroupsNoOps();
                if (groups.Count == 1)
                {
                    ExComp[] gp = groups[0];
                    Number coeff = gp.GetCoeff();
                    coeff = Number.Abs(coeff);
                    gp.AssignCoeff(coeff);
                    return new AbsValFunction(gp.ToAlgTerm());
                }
            }

            return this;
        }

        public override string FinalToAsciiString()
        {
            return "|" + InnerTerm.FinalToAsciiString() + "|";
        }

        public override string ToAsciiString()
        {
            return "|" + InnerTerm.FinalToAsciiString() + "|";
        }

        public override string ToJavaScriptString(bool useRad)
        {
            string baseStr = base.ToJavaScriptString(useRad);
            if (baseStr == null)
                return null;
            return "Math.abs(" + baseStr + ")";
        }

        public override string ToString()
        {
            return ToTexString();
            return "AbVl(" + base.ToString() + ")";
        }

        public override string ToTexString()
        {
            return "|" + InnerTerm.ToTexString() + "|";
        }
    }
}