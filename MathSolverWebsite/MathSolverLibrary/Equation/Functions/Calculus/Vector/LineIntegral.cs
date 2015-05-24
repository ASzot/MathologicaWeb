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
    class LineIntegral : Integral
    {
        private AlgebraComp _lineIden;

        public AlgebraComp LineIden
        {
            get { return _lineIden; }
        }

        public LineIntegral(ExComp innerEx)
            : base(innerEx)
        {

        }

        public override ExComp Clone()
        {
            return ConstructLineIntegral(InnerTerm.Clone(), _lineIden == null ? null : (AlgebraComp)_lineIden.Clone(), _dVar == null ? null : (AlgebraComp)_dVar.Clone());
        }

        public override bool IsEqualTo(ExComp ex)
        {
            if (!(ex is LineIntegral))
                return false;

            LineIntegral other = ex as LineIntegral;

            if ((other._dVar == null || this._dVar == null) && !(other._dVar == null && this._dVar == null))
                return false;

            if ((other._lineIden == null || this._lineIden == null) && !(other._lineIden == null && this._lineIden == null))
                return false;

            if (this._dVar != null && !this._dVar.IsEqualTo(other._dVar))
                return false;

            if (this._lineIden != null && !this._lineIden.IsEqualTo(other._lineIden))
                return false;

            return this.InnerEx.IsEqualTo(other.InnerEx);
        }

        protected override AlgebraTerm CreateInstance(params ExComp[] args)
        {
            return ConstructLineIntegral(args[0], _lineIden, _dVar);
        }

        public override string FinalToTexString()
        {
            if (InnerEx is ExVector)
                return "\\oint_{" + _lineIden.ToTexString() + "}" + InnerTerm.FinalToTexString() + "*d" + _dVar.ToTexString();
            return "\\oint_{" + _lineIden.ToTexString() + "}(" + InnerTerm.FinalToTexString() + ")d" + _dVar.ToTexString();
        }

        public override string FinalToAsciiString()
        {
            if (InnerEx is ExVector)
                return "\\oint_{" + _lineIden.ToAsciiString() + "}" + InnerTerm.FinalToAsciiString() + "*d" + _dVar.ToAsciiString();
            return "\\oint_{" + _lineIden.ToAsciiString() + "}(" + InnerTerm.FinalToAsciiString() + ")d" + _dVar.ToAsciiString();
        }

        public override string ToTexString()
        {
            if (InnerEx is ExVector)
                return "\\oint_{" + _lineIden.ToTexString() + "}" + InnerTerm.ToTexString() + "*d" + _dVar.ToTexString();
            return "\\oint_{" + _lineIden.ToTexString() + "}(" + InnerTerm.ToTexString() + ")d" + _dVar.ToTexString();
        }

        public override string ToAsciiString()
        {
            if (InnerEx is ExVector)
                return "\\oint_{" + _lineIden.ToAsciiString() + "}" + InnerTerm.ToAsciiString() + "*d" + _dVar.ToAsciiString();
            return "\\oint_{" + _lineIden.ToAsciiString() + "}(" + InnerTerm.ToAsciiString() + ")d" + _dVar.ToAsciiString();
        }

        public static LineIntegral ConstructLineIntegral(ExComp innerEx, AlgebraComp surfaceIden, AlgebraComp withRespectTo)
        {
            LineIntegral lineIntegral = new LineIntegral(innerEx);
            lineIntegral._lineIden = surfaceIden;
            lineIntegral._dVar = withRespectTo;

            return lineIntegral;
        }

        private ExComp EvaluateScalarField(ref TermType.EvalData pEvalData, AlgebraComp pathVar, AndRestriction pathRestriction, TypePair<string, ExComp>[] useDefs)
        {
            string totalFuncStr = "";
            for (int i = 0; i < useDefs.Length; ++i)
            {
                totalFuncStr += "(\\frac{d" + useDefs[i].Data1 + "}{d" + pathVar.ToDispString() + "})^{2}";
                if (i != useDefs.Length - 1)
                    totalFuncStr += "+";
            }

            totalFuncStr = "\\sqrt{" + totalFuncStr + "}";

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "d" + pathVar.ToDispString() + "=" + totalFuncStr + WorkMgr.EDM,
                "Find the path differential.");

            WorkStep lastStep;

            ExComp[] derived = new ExComp[useDefs.Length];
            for (int i = 0; i < derived.Length; ++i)
            {
                pEvalData.WorkMgr.FromFormatted("");
                lastStep = pEvalData.WorkMgr.GetLast();

                lastStep.GoDown(ref pEvalData);
                derived[i] = Derivative.TakeDeriv(useDefs[i].Data2, pathVar, ref pEvalData);
                lastStep.GoUp(ref pEvalData);

                lastStep.WorkHtml = WorkMgr.STM + "\\frac{d" + useDefs[i].Data1 + "}{d" + pathVar.ToDispString() + "}=" + WorkMgr.ToDisp(derived[i]) + WorkMgr.EDM;
            }

            ExComp[] squared = new ExComp[derived.Length];
            for (int i = 0; i < squared.Length; ++i)
            {
                lastStep = pEvalData.WorkMgr.GetLast();

                squared[i] = PowOp.StaticCombine(derived[i], new Number(2.0));

                pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "(\\frac{d" + useDefs[i].Data1 + "}{d" + pathVar.ToDispString() + "})^{2}=" + WorkMgr.ToDisp(squared[i]) + WorkMgr.EDM);
            }

            ExComp combined = null;
            for (int i = 0; i < squared.Length; ++i)
            {
                if (combined == null)
                    combined = squared[i];
                else
                    combined = AddOp.StaticCombine(combined, squared[i]);
            }

            ExComp surfaceDifferential = PowOp.StaticCombine(combined, AlgebraTerm.FromFraction(Number.One, new Number(2.0)));
            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "d" + _dVar.ToDispString() + "=" + totalFuncStr + "=" + WorkMgr.ToDisp(surfaceDifferential) + WorkMgr.EDM);

            AlgebraTerm innerTerm = InnerTerm;
            for (int i = 0; i < useDefs.Length; ++i)
            {
                innerTerm = innerTerm.Substitute(new AlgebraComp(useDefs[i].Data1), useDefs[i].Data2);
            }

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + this.FinalToDispStr() + "=" + "\\oint_{" + _lineIden.ToDispString() + 
                "}(" + innerTerm.FinalToDispStr() + ")ds" + WorkMgr.EDM, "Substitute in the parameterized path.");

            ExComp totalInner = MulOp.StaticCombine(innerTerm, surfaceDifferential);

            pEvalData.WorkMgr.FromFormatted("", "Use the path domain to convert to a definite integral.");
            lastStep = pEvalData.WorkMgr.GetLast();

            lastStep.GoDown(ref pEvalData);
            Integral integral = Integral.ConstructIntegral(totalInner, pathVar, pathRestriction.GetLower(), pathRestriction.GetUpper());
            lastStep.GoUp(ref pEvalData);
            
            ExComp integralEval = integral.Evaluate(false, ref pEvalData);

            lastStep.WorkHtml = WorkMgr.STM + integral.FinalToDispStr() + (integralEval is Integral ? "" : "=" + WorkMgr.ToDisp(integralEval)) + WorkMgr.EDM;

            return integralEval;
        }

        private ExComp EvaluateVectorField(ref TermType.EvalData pEvalData, AlgebraComp pathVar, AndRestriction pathRestriction, TypePair<string, ExComp>[] useDefs)
        {
            ExComp[] pathDerivs = new ExComp[useDefs.Length];
            WorkStep lastStep;
            string pathDerivStr = _dVar.ToDispString() + "'(" + pathVar.ToDispString() + ")";

            for (int i = 0; i < useDefs.Length; ++i)
            {
                pEvalData.WorkMgr.FromFormatted("", "Work towards calculating " + WorkMgr.STM + pathDerivStr + WorkMgr.EDM);
                lastStep = pEvalData.WorkMgr.GetLast();

                lastStep.GoDown(ref pEvalData);
                pathDerivs[i] = Derivative.TakeDeriv(useDefs[i].Data2, pathVar, ref pEvalData);
                lastStep.GoUp(ref pEvalData);

                lastStep.WorkHtml = WorkMgr.STM + "\\frac{d" + useDefs[i].Data1 + "}{d" + pathVar.ToDispString() + "}=" + WorkMgr.ToDisp(pathDerivs[i]) + WorkMgr.EDM;
            }

            string pathStr = "";
            for (int i = 0; i < pathDerivs.Length; ++i)
            {
                pathStr += WorkMgr.ToDisp(pathDerivs[i]);

                if (i != pathDerivs.Length - 1)
                    pathStr += ",";
            }

            pathStr = "[" + pathStr + "]";

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + "d" + _dVar.ToDispString() + "=" + pathDerivStr + "=" + pathStr + WorkMgr.EDM);

            // Rewrite the function in terms of the path variable.
            AlgebraTerm innerTerm = InnerTerm;
            foreach (TypePair<string, ExComp> useDef in useDefs)
            {
                innerTerm = innerTerm.Substitute(new AlgebraComp(useDef.Data1), useDef.Data2);
            }

            ExComp innerEx = innerTerm is AlgebraTerm ? (innerTerm as AlgebraTerm).RemoveRedundancies() : innerTerm;
            if (!(innerEx is ExVector))
                return this;

            ExVector innerVec = innerEx as ExVector;

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + this.FinalToDispStr() + "=\\oint_{" + _lineIden.ToDispString() + "}" + innerVec.FinalToDispStr() + "*d" + _dVar.ToDispString() + WorkMgr.EDM,
                "Substitute in the parameterized path.");

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + innerVec.FinalToDispStr() + "*" + pathStr + WorkMgr.EDM, "Calculate the dot product.");

            // Take the dot product of the function and the path equation.
            ExComp[] overallTerms = new ExComp[useDefs.Length];
            for (int i = 0; i < overallTerms.Length; ++i)
            {
                overallTerms[i] = MulOp.StaticCombine(innerVec.Get(i), pathDerivs[i]);
            }

            if (overallTerms.Length < 1)
                return this;

            // Add all terms together.
            ExComp overallTerm = overallTerms[0];
            for (int i = 1; i < overallTerms.Length; ++i)
            {
                overallTerm = AddOp.StaticCombine(overallTerm, overallTerms[i]);
            }

            pEvalData.WorkMgr.FromFormatted(WorkMgr.STM + innerVec.FinalToDispStr() + "*" + pathStr + "=" + WorkMgr.ToDisp(overallTerm) + WorkMgr.EDM);

            Integral integral = Integral.ConstructIntegral(overallTerm, pathVar, pathRestriction.GetLower(), pathRestriction.GetUpper());

            pEvalData.WorkMgr.FromFormatted("", "Use the path domain to convert to a definite integral.");
            lastStep = pEvalData.WorkMgr.GetLast();

            lastStep.GoDown(ref pEvalData);
            ExComp evaluated = integral.Evaluate(false, ref pEvalData);
            lastStep.GoUp(ref pEvalData);

            lastStep.WorkHtml = WorkMgr.STM + integral.FinalToDispStr() + (evaluated is Integral ? "" : "=" + WorkMgr.ToDisp(evaluated)) + WorkMgr.EDM;

            return evaluated;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            CallChildren(harshEval, ref pEvalData);

            // Get the line.
            List<FunctionDefinition> vectorFuncs = pEvalData.FuncDefs.GetAllVecEquations(1);
            FunctionDefinition vectorFunc = null;
            if (vectorFuncs != null || vectorFuncs.Count == 0)
            {
                vectorFunc = FuncDefHelper.GetMostCurrentDef(vectorFuncs, _lineIden);
            }

            List<FunctionDefinition> paraFuncs = pEvalData.FuncDefs.GetProbableParametricEquations(1);

            int maxIndex = int.MinValue;
            if (paraFuncs != null)
                maxIndex = FuncDefHelper.GetMostCurrentIndex(paraFuncs);

            if (vectorFunc == null && (paraFuncs == null || paraFuncs.Count == 0))
                return this;


            TypePair<string, ExComp>[] useDefs;
            AlgebraComp pathVar = null;

            if (vectorFunc != null && vectorFunc.FuncDefIndex > maxIndex)
            {
                useDefs = pEvalData.FuncDefs.GetDefinitionToPara(vectorFunc);
                pathVar = vectorFunc.InputArgs[0];
            }
            else if (paraFuncs.Count < 2)
                return this;
            else
            {
                useDefs = new TypePair<string, ExComp>[paraFuncs.Count];
                pathVar = paraFuncs[0].InputArgs[0];
                for (int i = 0; i < useDefs.Length; ++i)
                {
                    ExComp definition = pEvalData.FuncDefs.GetDefinition(paraFuncs[i]).Value;
                    useDefs[i] = new TypePair<string, ExComp>(paraFuncs[i].Iden.Var.Var, definition);
                }
            }

            AndRestriction pathRestriction = pEvalData.GetVariableRestriction(pathVar);
            if (pathRestriction == null)
                return this;

            ExComp innerEx = InnerEx;
            if (innerEx is ExVector)
                return EvaluateVectorField(ref pEvalData, pathVar, pathRestriction, useDefs);
            else if (innerEx is ExMatrix)
                return this;
            else
                return EvaluateScalarField(ref pEvalData, pathVar, pathRestriction, useDefs);
        }
    }
}
