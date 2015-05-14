using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathSolverWebsite.MathSolverLibrary.Information_Helpers;
using MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus.Vector
{
    class SurfaceIntegral : Integral
    {
        /// <summary>
        /// In the case _withRespect1 is null this is the surface differential.
        /// </summary>
        private AlgebraComp _withRespect0;
        private AlgebraComp _withRespect1;

        public SurfaceIntegral(ExComp innerEx)
            : base(innerEx)
        {
            
        }

        public static SurfaceIntegral ConstructSurfaceIntegral(ExComp innerEx, AlgebraComp surfaceDifferential)
        {
            return ConstructSurfaceIntegral(innerEx, surfaceDifferential, null);
        }

        public static SurfaceIntegral ConstructSurfaceIntegral(ExComp innerEx, AlgebraComp withRespect0, AlgebraComp withRespect1)
        {
            SurfaceIntegral surfaceIntegral = new SurfaceIntegral(innerEx);
            surfaceIntegral._withRespect0 = withRespect0;
            surfaceIntegral._withRespect1 = withRespect1;

            return surfaceIntegral;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            int startingWorkSteps = pEvalData.WorkMgr.WorkSteps.Count;
            if (_withRespect1 == null)
            {
                // Only the surface differential identifier was specified.
                List<FunctionDefinition> vectorFuncs = pEvalData.FuncDefs.GetAllVecEquations(2);
                FunctionDefinition vectorFunc = FuncDefHelper.GetMostCurrentDef(vectorFuncs, _withRespect0);

                if (vectorFunc == null)
                    return this;

                ExComp definition = pEvalData.FuncDefs.GetDefinition(vectorFunc).Value;
                ExVector vectorDef = definition as ExVector;
                if (vectorDef == null || vectorDef.Length > 3)
                    return this;

                AlgebraComp withRespect0 = vectorFunc.InputArgs[0];
                AlgebraComp withRespect1 = vectorFunc.InputArgs[1];

                AndRestriction respect0Rest = pEvalData.GetVariableRestriction(withRespect0);
                AndRestriction respect1Rest = pEvalData.GetVariableRestriction(withRespect1);

                if (respect0Rest == null || respect1Rest == null)
                {
                    return this;
                }

                string vectorFuncIden = "\\vec{" + vectorFunc.Iden.ToDispString() + "}";
                string withRespect0Str = withRespect0.ToDispString();
                string withRespect1Str = withRespect1.ToDispString();
                string integralStr = "\\iint_{" + _withRespect0.ToDispString() + "}";
                string innerStr = InnerTerm.FinalToDispStr();
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + integralStr + innerStr +
                    "|\\frac{\\partial " + vectorFuncIden + "}{\\partial " + withRespect0Str + "}" + CrossProductOp.IDEN +
                    "\\frac{\\partial " + vectorFuncIden + "}{\\partial " + withRespect1Str + "}|d" + withRespect0Str + "d" + withRespect1Str + WorkMgr.EDM);

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "|\\frac{\\partial " + vectorFuncIden + "}{\\partial " + withRespect0Str + "}" + CrossProductOp.IDEN +
                    "\\frac{\\partial " + vectorFuncIden + "}{\\partial " + withRespect1Str + "}|" + WorkMgr.EDM, "Calculate.");

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\frac{\\partial " + vectorFuncIden + "}{\\partial " + withRespect0Str + "}" + WorkMgr.EDM, "Calculate.");
                ExComp surfacePartial0 = Derivative.TakeDeriv(definition, withRespect0, ref pEvalData, true);

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "\\frac{\\partial " + vectorFuncIden + "}{\\partial " + withRespect1Str + "}" + WorkMgr.EDM, "Calculate.");
                ExComp surfacePartial1 = Derivative.TakeDeriv(definition, withRespect1, ref pEvalData, true);

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + WorkMgr.ToDisp(surfacePartial0) + CrossProductOp.IDEN + WorkMgr.ToDisp(surfacePartial1) + WorkMgr.EDM, "Calculate the cross product");
                ExComp crossed = CrossProductOp.StaticCombine(surfacePartial0, surfacePartial1);
                pEvalData.WorkMgr.FromSides(crossed, null, "The calculated cross product.");

                if (!(crossed is ExVector))
                {
                    pEvalData.WorkMgr.WorkSteps.RemoveRange(startingWorkSteps, pEvalData.WorkMgr.WorkSteps.Count - startingWorkSteps);
                    return this;
                }

                ExVector crossedVec = crossed as ExVector;
                ExComp crossedVecLength = crossedVec.GetVecLength();

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "|" + crossedVec.FinalToDispStr() + "|=" + WorkMgr.ToDisp(crossedVecLength) + WorkMgr.EDM,
                    "Calculate the length of the vector.");

                string surfaceDiffIdenStr = "d" + _withRespect0.ToDispString();
                string surfaceDiffStr = WorkMgr.ToDisp(crossedVecLength);
                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + surfaceDiffIdenStr + "=" + surfaceDiffStr + WorkMgr.EDM, "The calculated surface differential");

                // Sub the parameterization of the surface into the function.
                AlgebraTerm innerTerm = InnerTerm;

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + integralStr + innerStr + surfaceDiffIdenStr + WorkMgr.EDM, 
                    "Change to terms of " + WorkMgr.STM + withRespect0Str + WorkMgr.EDM + " and " + WorkMgr.STM + withRespect1Str + WorkMgr.EDM);

                // X, Y, and Z have to be assumed for this.
                for (int i = 0; i < vectorDef.Length; ++i)
                {
                    string dimenIden = FunctionDefinition.GetDimenStr(i);
                    innerTerm = innerTerm.Substitute(new AlgebraComp(dimenIden), vectorDef.Get(i));
                }

                innerStr = innerTerm.FinalToDispStr();

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + integralStr + "(" + innerStr + ")(" + surfaceDiffStr + ")d" + withRespect0Str + "d" + withRespect1Str + WorkMgr.EDM);
                Integral firstIntegral = Integral.ConstructIntegral(innerTerm, withRespect0, respect0Rest.GetLower(), respect0Rest.GetUpper());
                Integral secondIntegral = Integral.ConstructIntegral(firstIntegral, withRespect1, respect1Rest.GetLower(), respect1Rest.GetUpper());

                return secondIntegral.Evaluate(false, ref pEvalData);
            }
            else
            {
                AndRestriction withRespect0Restriction = pEvalData.GetVariableRestriction(_withRespect0);
                AndRestriction withRespect1Restriction = pEvalData.GetVariableRestriction(_withRespect1);

                Integral firstIntegral = Integral.ConstructIntegral(InnerEx, _withRespect0, withRespect0Restriction.GetLower(), withRespect0Restriction.GetUpper());
                Integral secondIntegral = Integral.ConstructIntegral(firstIntegral, _withRespect1, withRespect1Restriction.GetLower(), withRespect1Restriction.GetUpper());

                return secondIntegral.Evaluate(false, ref pEvalData);
            }
        }
    }
}