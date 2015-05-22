using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus.Vector
{
    abstract class FieldTransformation : BasicAppliedFunc
    {
        public FieldTransformation(ExComp innerEx, string name, FunctionType ft, Type type)
            : base(innerEx, name, ft, type)
        {

        }

        protected ExComp GetCorrectedInnerEx(ref TermType.EvalData pEvalData)
        {
            ExComp innerEx = InnerEx;

            if (innerEx is AlgebraComp)
            {
                // There is a chance this is actually refering to a function.
                AlgebraComp funcIden = innerEx as AlgebraComp;
                KeyValuePair<FunctionDefinition, ExComp> def = pEvalData.FuncDefs.GetDefinition(funcIden);
                if (def.Key != null && def.Key.InputArgCount > 1)
                {
                    innerEx = def.Value;
                    if (innerEx is AlgebraTerm)
                        innerEx = (innerEx as AlgebraTerm).RemoveRedundancies();
                }
            }

            return innerEx;
        }
    }
}
