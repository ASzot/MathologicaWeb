using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus.Vector
{
    class LineIntegral : Integral
    {
        private AlgebraComp _surfaceIden;

        public AlgebraComp SurfaceIden
        {
            get { return _surfaceIden; }
        }

        public LineIntegral(ExComp innerEx)
            : base(innerEx)
        {
        }

        public static LineIntegral ConstructLineIntegral(ExComp innerEx, AlgebraComp surfaceIden, AlgebraComp withRespectTo)
        {
            LineIntegral lineInt = new LineIntegral(innerEx);
            lineInt._dVar = withRespectTo;
            lineInt._surfaceIden = surfaceIden;
            return lineInt;
        }

        public override ExComp Evaluate(bool harshEval, ref TermType.EvalData pEvalData)
        {
            return base.Evaluate(harshEval, ref pEvalData);
        }
    }
}