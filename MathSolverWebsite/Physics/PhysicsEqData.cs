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
        private List<string> _variables;
        private List<string> _unitListings;

        public string Path
        {
            get { return _path; }
        }

        public string DispName
        {
            get { return _dispName; }
        }

        public PhysicsEqData(string path)
        {
            _path = path;
        }

        public override string GetHintStr()
        {
            return "";
        }

        public SolveResult Evaluate(string selectedOption, List<string> variables, List<string> values, ref EvalData pEvalData)
        {
            // Plug in all of the values.
            List<ExComp> exValues = new List<ExComp>();
            LexicalParser lexParser = new LexicalParser(pEvalData);
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
                leftTerm = leftTerm.Substitute(varComps[i], exValues[i]);
                rightTerm = rightTerm.Substitute(varComps[i], exValues[i]);
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

        public bool Init(string initStr, List<string> variables, List<string> unitListings, ref EvalData pEvalData)
        {
            List<string> parseErrors = new List<string>();
            List<List<TypePair<LexemeType, string>>> lts = new List<List<TypePair<LexemeType, string>>>();

            LexicalParser parser = new LexicalParser(pEvalData);
            List<EquationSet> eqs = parser.ParseInput(initStr, out lts, ref parseErrors);

            if (eqs.Count != 1)
                return false;

            _left = eqs[0].Left;
            _right = eqs[0].Right;

            if (_left == null || _right == null)
                return false;

            _variables = variables;
            _unitListings = unitListings;

            return true;
        }
    }
}