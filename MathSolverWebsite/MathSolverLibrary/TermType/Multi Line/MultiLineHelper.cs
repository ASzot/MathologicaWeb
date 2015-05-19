using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

using LexemeTable = System.Collections.Generic.List<
MathSolverWebsite.MathSolverLibrary.TypePair<MathSolverWebsite.MathSolverLibrary.Parsing.LexemeType, string>>;

namespace MathSolverWebsite.MathSolverLibrary.TermType
{
    class MultiLineHelper
    {
        private List<TypePair<FunctionDefinition, ExComp>> _funcDefs = new List<TypePair<FunctionDefinition, ExComp>>();
        private List<AndRestriction> _intervalDefs = new List<AndRestriction>();
        private List<List<WorkStep>> _intervalWorkSteps = new List<List<WorkStep>>();
        private string[] _graphStrs = null;
        private string _graphVar = null;

        public bool ShouldGraph
        {
            get
            {
                return _graphVar != null && _graphStrs != null;
            }
        }

        public string[] GraphStrs
        {
            get { return _graphStrs; }
        }

        public string GraphVar
        {
            get { return _graphVar; }
        }

        public MultiLineHelper()
        {

        }

        public void DoAssigns(ref EvalData pEvalData)
        {
            foreach (TypePair<FunctionDefinition, ExComp> funcDef in _funcDefs)
            {
                pEvalData.FuncDefs.Define(funcDef.Data1, funcDef.Data2, ref pEvalData);
            }
            foreach (List<WorkStep> intervalWorkStep in _intervalWorkSteps)
            {
                if (intervalWorkStep != null)
                    pEvalData.WorkMgr.WorkSteps.AddRange(intervalWorkStep);
            }
            foreach (AndRestriction rest in _intervalDefs)
            {
                pEvalData.AddVariableRestriction(rest);
            }
        }

        public List<EqSet> AssignLines(List<EqSet> eqs, ref List<LexemeTable> lts, ref Dictionary<string, int> solveVars, out LexemeTable totalLt, ref EvalData pEvalData)
        {
            totalLt = new LexemeTable();
            int ltsCount = 0;
            for (int i = 0; i < eqs.Count; ++i)
            {
                if (AttemptAddFuncDef(eqs, i, ref pEvalData) || AttemptAddIntervalDef(eqs[i], lts[i], ref pEvalData))
                {
                    lts.RemoveRange(ltsCount, eqs[i].ValidComparisonOps.Count + 1);
                    eqs.RemoveAt(i--);
                }
                else
                    ltsCount += eqs[i].ValidComparisonOps.Count + 1;
            }

            foreach (LexemeTable lt in lts)
            {
                totalLt.AddRange(lt);
            }

            FunctionDefinition funcDef = null;
            bool allEqual = true;
            foreach (TypePair<FunctionDefinition, ExComp> searchFuncDef in _funcDefs)
            {
                if (funcDef == null)
                    funcDef = searchFuncDef.Data1;
                else
                {
                    if (funcDef.IsEqualTo(searchFuncDef.Data1))
                    {
                        allEqual = false;
                        break;
                    }
                }
            }

            if (!allEqual && funcDef != null)
            {
                solveVars.Remove(funcDef.Iden.Var.Var);

                if (solveVars.Count == 1)
                {
                    // There should only be one.
                    foreach (string solveVar in solveVars.Keys)
                        _graphVar = solveVar;

                    _graphStrs = new string[_funcDefs.Count];
                    bool allValid = true;
                    for (int i = 0; i < _funcDefs.Count; ++i)
                    {
                        _graphStrs[i] = _funcDefs[i].Data2.ToJavaScriptString(pEvalData.UseRad);
                        if (_graphStrs[i] == null)
                        {
                            allValid = false;
                            break;
                        }
                    }

                    if (!allValid)
                    {
                        _graphVar = null;
                        _graphStrs = null;
                    }
                }
            }

            solveVars = AlgebraSolver.GetIdenOccurances(totalLt);

            return eqs;
        }

        private bool AttemptAddIntervalDef(EqSet eqSet, LexemeTable lt, ref EvalData pEvalData)
        {
            if (eqSet.Sides.Count != 3)
                return false;

            if (eqSet.ComparisonOps.Count != 2)
                return false;

            AlgebraSolver algebraSolver = new AlgebraSolver();
            Dictionary<string, int> solveVars = AlgebraSolver.GetIdenOccurances(lt);
            string solveVar = AlgebraSolver.GetProbableVar(solveVars);

            int workStepCount = pEvalData.WorkMgr.WorkSteps.Count;
            bool addWork = eqSet.Sides[1].IsEqualTo(new AlgebraComp(solveVar));

            SolveResult result = algebraSolver.SolveEquationInequality(eqSet.Sides, eqSet.ComparisonOps, new AlgebraVar(solveVar), ref pEvalData);

            if (addWork)
                _intervalWorkSteps.Add(pEvalData.WorkMgr.WorkSteps.GetRange(workStepCount, pEvalData.WorkMgr.WorkSteps.Count - workStepCount));
            else
                _intervalWorkSteps.Add(null);
            pEvalData.WorkMgr.WorkSteps.RemoveRange(workStepCount, pEvalData.WorkMgr.WorkSteps.Count - workStepCount);

            if (!(result.Solutions != null && result.Solutions.Count != 0) ||
                (result.Restrictions == null || result.Restrictions.Count != 1) ||
                (!(result.Restrictions[0] is AndRestriction)))
                return false;

            _intervalDefs.Add(result.Restrictions[0] as AndRestriction);

            return true;
        }

        private bool AttemptAddFuncDef(List<EqSet> eqs, int i, ref EvalData pEvalData)
        {
            EqSet eqSet = eqs[i];
            if (eqSet.Sides.Count != 2 || eqSet.Sides[1] == null)
                return false;
            FunctionDefinition funcDef = null;
            ExComp assignTo = null;

            ExComp left = eqSet.Left;
            if (left is AlgebraTerm)
                left = (left as AlgebraTerm).RemoveRedundancies();

            ExComp right = eqSet.Right;
            if (right is AlgebraTerm)
                right = (right as AlgebraTerm).RemoveRedundancies();

            if (left is FunctionDefinition)
            {
                funcDef = left as FunctionDefinition;
                assignTo = right;
            }
            else if (right is FunctionDefinition)
            {
                funcDef = right as FunctionDefinition;
                assignTo = left;
            }
            else if (left is AlgebraComp)
            {
                funcDef = new FunctionDefinition(left as AlgebraComp, null, null, false);
                assignTo = right;
            }
            else if (right is AlgebraComp)
            {
                funcDef = new FunctionDefinition(right as AlgebraComp, null, null, false);
                assignTo = left;
            }
            else
                return false;

            _funcDefs.Add(new TypePair<FunctionDefinition, ExComp>(funcDef, assignTo));
            // Also call this function on every single other side.
            for (int j = 0; j < eqs.Count; ++j)
            {
                if (j == i)
                    continue;
                eqs[j].CallFunction(funcDef, assignTo);
            }

            //pEvalData.FuncDefs.Define(funcDef, assignTo, ref pEvalData);
            return true;
        }
    }
}