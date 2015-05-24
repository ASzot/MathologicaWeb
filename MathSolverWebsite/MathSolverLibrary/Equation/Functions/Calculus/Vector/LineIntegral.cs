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

        public LineIntegral(ExComp innerEx)
            : base(innerEx)
        {

        }

        public override ExComp Clone()
        {
            return ConstructLineIntegral(InnerTerm, _lineIden, _dVar);
        }

        protected override AlgebraTerm CreateInstance(params ExComp[] args)
        {
            return ConstructLineIntegral(args[0], _lineIden, _dVar);
        }

        public override string FinalToTexString()
        {
            return "\\oint_{" + _lineIden.ToTexString() + "}(" + InnerTerm.FinalToTexString() + ")d" + _dVar.ToTexString();
        }

        public override string FinalToAsciiString()
        {
            return "\\oint_{" + _lineIden.ToAsciiString() + "}(" + InnerTerm.FinalToAsciiString() + ")d" + _dVar.ToAsciiString();
        }

        public override string ToTexString()
        {
            return "\\oint_{" + _lineIden.ToTexString() + "}(" + InnerTerm.ToTexString() + ")d" + _dVar.ToTexString();
        }

        public override string ToAsciiString()
        {
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
            ExComp[] derived = new ExComp[useDefs.Length];
            for (int i = 0; i < derived.Length; ++i)
            {
                derived[i] = Derivative.TakeDeriv(useDefs[i].Data2, pathVar, ref pEvalData);
            }

            ExComp[] squared = new ExComp[derived.Length];
            for (int i = 0; i < squared.Length; ++i)
            {
                squared[i] = PowOp.StaticCombine(derived[i], new Number(2.0));
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

            AlgebraTerm innerTerm = InnerTerm;
            for (int i = 0; i < useDefs.Length; ++i)
            {
                innerTerm = innerTerm.Substitute(new AlgebraComp(useDefs[i].Data1), useDefs[i].Data2);
            }

            ExComp totalInner = MulOp.StaticCombine(innerTerm, surfaceDifferential);

            Integral integral = Integral.ConstructIntegral(totalInner, pathVar, pathRestriction.GetLower(), pathRestriction.GetUpper());

            return integral.Evaluate(false, ref pEvalData);
        }

        private ExComp EvaluateVectorField(ref TermType.EvalData pEvalData, AlgebraComp pathVar, AndRestriction pathRestriction, TypePair<string, ExComp>[] useDefs)
        {
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

            // Take the dot product of the function and the path equation.
            ExComp[] overallTerms = new ExComp[useDefs.Length];
            for (int i = 0; i < overallTerms.Length; ++i)
            {
                overallTerms[i] = MulOp.StaticCombine(innerVec.Get(i), useDefs[i].Data2);
            }

            if (overallTerms.Length < 1)
                return this;

            // Add all terms together.
            ExComp overallTerm = overallTerms[0];
            for (int i = 1; i < overallTerms.Length; ++i)
            {
                overallTerm = AddOp.StaticCombine(overallTerm, overallTerms[i]);
            }

            Integral integral = Integral.ConstructIntegral(overallTerm, pathVar, pathRestriction.GetLower(), pathRestriction.GetUpper());

            return integral.Evaluate(false, ref pEvalData);
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            // Get the line.
            List<FunctionDefinition> vectorFuncs = pEvalData.FuncDefs.GetAllVecEquations(1);
            FunctionDefinition vectorFunc = null;
            if (vectorFuncs != null || vectorFuncs.Count == 0)
            {
                vectorFunc = FuncDefHelper.GetMostCurrentDef(vectorFuncs, _lineIden);
            }

            List<FunctionDefinition> paraFuncs = pEvalData.FuncDefs.GetProbableParametricEquations(1);

            int maxIndex = int.MinValue;
            if (paraFuncs == null)
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
