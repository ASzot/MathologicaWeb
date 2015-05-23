using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus.Vector
{
    class CurlFunc : FieldTransformation
    {
        public CurlFunc(ExComp innerEx)
            : base(innerEx, "\\text{curl}", FunctionType.Curl, typeof(CurlFunc))
        {

        }

        public static bool IsSuitableField(ExComp innerEx)
        {
            if (innerEx is ExVector)
            {
                ExVector exVec = innerEx as ExVector;
                return exVec.Length > 1 && exVec.Length < 4;
            }
            else if (innerEx is FunctionDefinition)
            {
                FunctionDefinition funcDef = innerEx as FunctionDefinition;

                return funcDef.InputArgCount > 1 && funcDef.InputArgCount < 4;
            }
            else if (innerEx is AlgebraComp)
            {
                // The user is reasonably referring to a function not an individual variable.
                return true;
            }
            else
                return false;
        }

        public override ExComp CancelWith(ExComp innerEx, ref TermType.EvalData pEvalData)
        {
            if (innerEx is GradientFunc)
            {
                pEvalData.WorkMgr.FromFormatted(this.FinalToDispStr() + "=0", "From the identity curl(\\nablaF)=0");
                return Number.Zero;
            }

            return null;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            if (!IsSuitableField(InnerEx))
                return Number.Undefined;

            ExComp p, q, r;
            ExComp innerEx = GetCorrectedInnerEx(ref pEvalData);
            
            AlgebraComp x = null;
            AlgebraComp y = null;
            AlgebraComp z = null;

            bool isFuncDeriv;

            if (innerEx is AlgebraComp)
            {
                innerEx = new FunctionDefinition(innerEx as AlgebraComp, 
                    new AlgebraComp[] 
                    { 
                        new AlgebraComp("x"), 
                        new AlgebraComp("y"), 
                        new AlgebraComp("z") 
                    }, null);
            }

            if (innerEx is ExVector)
            {
                ExVector innerVec = innerEx as ExVector;
                p = innerVec.X;
                q = innerVec.Y;
                r = innerVec.Z;
                isFuncDeriv = false;
            }
            else if (innerEx is FunctionDefinition)
            {
                FunctionDefinition funcDef = innerEx as FunctionDefinition;
                p = new AlgebraComp("P");
                q = new AlgebraComp("Q");
                r = funcDef.InputArgCount == 3 ? new AlgebraComp("R") : null;

                x = funcDef.InputArgs[0];
                y = funcDef.InputArgs[1];
                z = funcDef.InputArgCount == 3 ? funcDef.InputArgs[2] : null;

                isFuncDeriv = true;
            }
            else
                return this;

            if (x == null)
            {
                x = new AlgebraComp("x");
                y = new AlgebraComp("y");
                z = new AlgebraComp("z");
            }

            if (isFuncDeriv)
            {
                pEvalData.WorkMgr.FromFormatted("", "Assuming " + WorkMgr.STM + innerEx.ToDispString() + WorkMgr.EDM + " is defined as the vector field " +
                    "P(x,y,z)" + ExVector.I + " + Q(x,y,z)" + ExVector.J + " + R(x,y,z)" + ExVector.K);
            }

            string formulaStr = "";
            string descStr = "";

            if (z != null)
            {
                formulaStr += "(\\frac{\\partial R}{\\partial y}  - \\frac{\\partial Q}{\\partial z})" + ExVector.I +
                    "+(\\frac{\\partial P}{\\partial z} - \\frac{\\partial R}{\\partial x})" + ExVector.J;
            }

            formulaStr += "+(\\frac{\\partial Q}{\\partial x} - \\frac{\\partial P}{\\partial y})" + ExVector.K;
            if (!isFuncDeriv && innerEx is ExVector)
            {
                descStr += "Where ";
                string funcParamsStr = z == null ? "(x,y)" : "(x,y,z)";
                ExVector innerVec = innerEx as ExVector;
                descStr += WorkMgr.STM + "P" + funcParamsStr + " = " + WorkMgr.ToDisp(innerVec.X) + ",Q" + funcParamsStr + "=" +
                    WorkMgr.ToDisp(innerVec.Y);
                if (z != null)
                    descStr += "," + "R" + funcParamsStr + "=" + WorkMgr.ToDisp(innerVec.Z);
                descStr += WorkMgr.EDM;
            }

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + this.FinalToDispStr() + "=" + formulaStr + WorkMgr.EDM, descStr);

            WorkStep lastStep;
            string stepStr;

            ExComp r_y;
            if (z != null)
            {
                stepStr = "\\frac{\\partial R}{\\partial " + y.ToDispString() + "}";
                pEvalData.WorkMgr.FromFormatted("", "Find " + WorkMgr.STM + stepStr + WorkMgr.EDM);
                lastStep = pEvalData.WorkMgr.GetLast();

                lastStep.GoDown(ref pEvalData);
                r_y = Derivative.TakeDeriv(r, y, ref pEvalData, true, isFuncDeriv);
                lastStep.GoUp(ref pEvalData);

                lastStep.WorkHtml = WorkMgr.STM + stepStr + "=" + WorkMgr.ToDisp(r_y) + WorkMgr.EDM;
            }
            else
                r_y = Number.Zero;

            ExComp q_z;
            if (z != null)
            {
                stepStr = "\\frac{\\partial Q}{\\partial " + z.ToDispString() + "}";
                pEvalData.WorkMgr.FromFormatted("", "Find " + WorkMgr.STM + stepStr + WorkMgr.EDM);
                lastStep = pEvalData.WorkMgr.GetLast();

                lastStep.GoDown(ref pEvalData);
                q_z = Derivative.TakeDeriv(q, z, ref pEvalData, true, isFuncDeriv);
                lastStep.GoUp(ref pEvalData);

                lastStep.WorkHtml = WorkMgr.STM + stepStr + "=" + WorkMgr.ToDisp(q_z) + WorkMgr.EDM;
            }
            else
                q_z = Number.Zero;

            ExComp p_z;
            if (z != null)
            {
                stepStr = "\\frac{\\partial P}{\\partial " + z.ToDispString() + "}";
                pEvalData.WorkMgr.FromFormatted("", "Find " + WorkMgr.STM + stepStr + WorkMgr.EDM);
                lastStep = pEvalData.WorkMgr.GetLast();

                lastStep.GoDown(ref pEvalData);
                p_z = Derivative.TakeDeriv(p, z, ref pEvalData, true, isFuncDeriv);
                lastStep.GoUp(ref pEvalData);

                lastStep.WorkHtml = WorkMgr.STM + stepStr + "=" + WorkMgr.ToDisp(p_z) + WorkMgr.EDM;
            }
            else
                p_z = Number.Zero;

            ExComp r_x;
            if (z != null)
            {
                stepStr = "\\frac{\\partial R}{\\partial " + x.ToDispString() + "}";
                pEvalData.WorkMgr.FromFormatted("", "Find " + WorkMgr.STM + stepStr + WorkMgr.EDM);
                lastStep = pEvalData.WorkMgr.GetLast();

                lastStep.GoDown(ref pEvalData);
                r_x = Derivative.TakeDeriv(r, x, ref pEvalData, true, isFuncDeriv);
                lastStep.GoUp(ref pEvalData);

                lastStep.WorkHtml = WorkMgr.STM + stepStr + "=" + WorkMgr.ToDisp(r_x) + WorkMgr.EDM;
            }
            else
                r_x = Number.Zero;


            stepStr = "\\frac{\\partial Q}{\\partial " + x.ToDispString() + "}";
            pEvalData.WorkMgr.FromFormatted("", "Find " + WorkMgr.STM + stepStr + WorkMgr.EDM);
            lastStep = pEvalData.WorkMgr.GetLast();

            lastStep.GoDown(ref pEvalData);
            ExComp q_x = Derivative.TakeDeriv(q, x, ref pEvalData, true, isFuncDeriv);
            lastStep.GoUp(ref pEvalData);

            lastStep.WorkHtml = WorkMgr.STM + stepStr + "=" + WorkMgr.ToDisp(q_x) + WorkMgr.EDM;


            stepStr = "\\frac{\\partial P}{\\partial " + y.ToDispString() + "}";
            pEvalData.WorkMgr.FromFormatted("", "Find " + WorkMgr.STM + stepStr + WorkMgr.EDM);
            lastStep = pEvalData.WorkMgr.GetLast();

            lastStep.GoDown(ref pEvalData);
            ExComp p_y = Derivative.TakeDeriv(p, y, ref pEvalData, true, isFuncDeriv);
            lastStep.GoUp(ref pEvalData);

            lastStep.WorkHtml = WorkMgr.STM + stepStr + "=" + WorkMgr.ToDisp(p_y) + WorkMgr.EDM;

            ExVector vec = new ExVector(
                SubOp.StaticCombine(r_y, q_z),
                SubOp.StaticCombine(p_z, r_x),
                SubOp.StaticCombine(q_x, p_y));

            pEvalData.WorkMgr.FromSides(this, vec);

            return vec;
        }
    }
}
