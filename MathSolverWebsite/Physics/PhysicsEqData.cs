using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Parsing;
using MathSolverWebsite.MathSolverLibrary.TermType;
using MathSolverWebsite.MathSolverLibrary;



using MathSolverWebsite.WebsiteHelpers;

namespace MathSolverWebsite.Physics
{
    class PhysicsEqData : TopicPath
    {
        private ExComp _left;
        private ExComp _right;
        private string _path;
        private string _dispName;
        private string _info;
        private Dictionary<string, bool> _variables;
        private List<string> _variableHints;
        private List<string> _unitListings;

        public string Path
        {
            get { return _path; }
        }

        public string DispName
        {
            get { return _dispName; }
        }

        public PhysicsEqData(string path, string dispStr, string info)
        {
            _path = path;
            _dispName = dispStr;
            _info = info;
        }

        public string GetHintStr()
        {
            return _dispName;
        }

        public SolveResult Evaluate(string selectedOption, List<string> variables, List<string> values, bool useRad)
        {
            // Plug in all of the values.
            var funcDefs = new MathSolverLibrary.Information_Helpers.FuncDefHelper();
            EvalData evalData = new EvalData(useRad, new WorkMgr(), funcDefs);

            List<ExComp> exValues = new List<ExComp>();
            LexicalParser lexParser = new LexicalParser(evalData);
            List<List<TypePair<LexemeType, string>>> lts = new List<List<TypePair<LexemeType, string>>>();
            List<string> parseErrors = new List<string>();

            List<AlgebraComp> varComps = new List<AlgebraComp>();
            foreach (string val in values)
            {
                List<EquationSet> eqs = lexParser.ParseInput(val, out lts, ref parseErrors);
                if (eqs.Count != 1 || !eqs[0].IsSingular)
                    return SolveResult.Failure();
                exValues.Add(eqs[0].Left);
                varComps.Add(new AlgebraComp(val));
            }

            AlgebraTerm leftTerm = _left.ToAlgTerm();
            AlgebraTerm rightTerm = _right.ToAlgTerm();
            for (int i = 0; i < varComps.Count; ++i)
            {
                if (!_variables.ContainsKey(varComps[i].Var.Var))
                    return SolveResult.Failure();

                if (!_variables[varComps[i].Var.Var])
                {
                    leftTerm = leftTerm.Substitute(varComps[i], exValues[i]);
                    rightTerm = rightTerm.Substitute(varComps[i], exValues[i]);
                }
                else
                {
                    // This is a function that needs to be defined.
                }
            }

            if (selectedOption == "check")
            {
                EqualityCheckTermType equalityCheck = new EqualityCheckTermType(leftTerm, rightTerm, LexemeType.EqualsOp);
                return equalityCheck.ExecuteCommand(null, ref evalData);
            }
            else
            {
                string solveForVar = selectedOption.Remove(0, "Solve for ".Length);

            }

            return SolveResult.Failure();
        }

        public string[] GetOptions(List<string> variables)
        {
            List<string> checkVars = new List<string>();
            foreach (string variable in variables)
            {
                checkVars.Add(variable);
            }
            for (int i = 0; i < variables.Count; ++i)
            {
                checkVars.Remove(variables[i]);
            }

            if (checkVars.Count == 0)
                return new string[] { "Check" };

            string[] options = new string[checkVars.Count];
            for (int i = 0; i < options.Length; ++i)
            {
                options[i] = "Solve for " + options[i];
            }

            return options;
        }

        public bool Init(string initStr, Dictionary<string, bool> variables, List<string> variableHints, List<string> unitListings, ref EvalData pEvalData)
        {
            if (variables.Count != unitListings.Count || variables.Count != variableHints.Count)
                return false;

            List<string> parseErrors = new List<string>();
            List<List<TypePair<LexemeType, string>>> lts = new List<List<TypePair<LexemeType, string>>>();

            LexicalParser parser = new LexicalParser(pEvalData);
            List<EquationSet> eqs = parser.ParseInput(initStr, out lts, ref parseErrors);

            if (eqs != null && eqs.Count != 1)
                return false;

            _left = eqs[0].Left;
            _right = eqs[0].Right;

            if (_left == null || _right == null)
                return false;

            _variables = variables;
            _variableHints = variableHints;
            _unitListings = unitListings;

            return true;
        }

        public string ToHtml()
        {
            string finalStr = "";

            finalStr += "<p class='sectionHeading'>" + _dispName + "</p>";

            finalStr += "<p>" + _info + "</p>";

            finalStr += "<p class='majorEquation'>" + WorkMgr.STM + _left.ToAlgTerm().FinalToDispStr() + "=" + _right.ToAlgTerm().FinalToDispStr() + WorkMgr.EDM + "</p>";

            for (int i = 0; i < _variables.Keys.Count; ++i)
            {
                string key = _variables.Keys.ElementAt(i);

                finalStr += "<p>" + WorkMgr.STM + key + ":" + WorkMgr.EDM +
                    "<span class='mathquill-editable' style='width: auto; margin-left: 10px; margin-right: 10px; position: relative; top: 20px; overflow-x: hidden;' ></span>" + 
                    WorkMgr.STM + _unitListings[i] + WorkMgr.EDM + "</p>";
            }

            return finalStr;
        }
    }
}