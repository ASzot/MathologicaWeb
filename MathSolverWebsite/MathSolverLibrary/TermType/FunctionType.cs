using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Parsing;
using System.Collections.Generic;
using System.Linq;

namespace MathSolverWebsite.MathSolverLibrary.TermType
{
    internal class FunctionTermType : TermType
    {
        private AlgebraSolver _agSolver;
        private ExComp _assignTo;
        private FunctionDefinition _func;


        public FunctionTermType()
            : base()
        {
        }

        public override Equation.SolveResult ExecuteCommand(string command, ref EvalData pEvalData)
        {
            base.ExecuteCommand(command, ref pEvalData);

            _agSolver.ResetIterCount();

            if (command == "Find inverse")
            {
                pEvalData.AttemptSetInputType(InputType.FunctionInverse);

                if (pEvalData.WorkMgr.AllowWork && _func.InputArgCount > 0)
                {
                    string funcStr = WorkMgr.ToDisp(_func);
                    string callArgStr = WorkMgr.ToDisp(_func.InputArgs[0]);
                    pEvalData.WorkMgr.FromSides(_func, _assignTo, "To find the inverse switch `" + WorkMgr.ToDisp(_func) + "` with `" + callArgStr +
                        "` and solve for `" + funcStr + "`");
                }

                // Find the inverse.
                AlgebraComp inverseFunc = new AlgebraComp(_func.Iden.ToString() + "^(-1)" + "(" + _func.InputArgs[0].ToString() + ")");
                AlgebraTerm left = _func.InputArgs[0].ToAlgTerm();
                AlgebraTerm right = _assignTo.Clone().ToAlgTerm().Substitute(_func.InputArgs[0], inverseFunc);

                if (pEvalData.WorkMgr.AllowWork)
                    pEvalData.WorkMgr.FromSides(left, right, "`" + WorkMgr.ToDisp(inverseFunc) + "` is the inverse function, solve for it.");

                return _agSolver.SolveEquationEquality(inverseFunc.Var, left, right, ref pEvalData);
            }
            else if (command.StartsWith("Assign"))
            {
                // Assign the function.
                if (_assignTo is AlgebraTerm)
                {
                    (_assignTo as AlgebraTerm).ApplyOrderOfOperations();
                    _assignTo = (_assignTo as AlgebraTerm).MakeWorkable();
                }

                pEvalData.FuncDefs.Define(_func, _assignTo, ref pEvalData);
                return SolveResult.Solved();
            }
            else if (command.StartsWith("Domain of "))
            {
                string varForKey = command.Substring("Domain of ".Length, command.Length - "Domain of ".Length);
                AlgebraVar varFor = new AlgebraVar(varForKey);

                return _agSolver.CalculateDomain(_assignTo, varFor, ref pEvalData);
            }
            else if (command == "Graph")
            {
                if (pEvalData.AttemptSetGraphData(_assignTo, _func.InputArgs[0].Var.Var))
                    return SolveResult.Solved();
                else
                    return SolveResult.Failure();
            }

            return SolveResult.InvalidCmd(ref pEvalData);
        }

        private bool ContainsSpecialFuncs(AlgebraTerm term)
        {
            foreach (ExComp subTerm in term.SubComps)
            {
                if ((subTerm is Equation.Functions.Calculus.Derivative) ||
                    (subTerm is Equation.Functions.Calculus.Integral) ||
                    (subTerm is Equation.Functions.Calculus.Vector.FieldTransformation) ||
                    (subTerm is Equation.Functions.ChooseFunction) ||
                    (subTerm is Equation.Functions.PermutationFunction) ||
                    (subTerm is Equation.Functions.Calculus.Limit) ||
                    (subTerm is Equation.Structural.LinearAlg.ExMatrix) ||
                    (subTerm is Equation.Structural.LinearAlg.Determinant) ||
                    (subTerm is Equation.Structural.LinearAlg.MatrixInverse))
                {
                    return true;
                }

                if (subTerm is AlgebraTerm && ContainsSpecialFuncs(subTerm as AlgebraTerm))
                    return true;
            }

            return false;
        }

        public bool Init(EqSet eqSet, List<TypePair<LexemeType, string>> lt, Dictionary<string, int> solveVars, string probSolveVar)
        {
            // Also allow the single variable assigns like y=x^2
            AlgebraComp funcIden = null;

            if (eqSet.Left is FunctionDefinition)
            {
                _func = eqSet.Left as FunctionDefinition;
                _assignTo = eqSet.Right;
            }
            else if (eqSet.Right is FunctionDefinition)
            {
                _func = eqSet.Right as FunctionDefinition;
                _assignTo = eqSet.Left;
            }
            else if (eqSet.Left is AlgebraComp && !eqSet.RightTerm.Contains(eqSet.Left as AlgebraComp))
            {
                funcIden = eqSet.Left as AlgebraComp;
                _assignTo = eqSet.Right;
            }
            else if (eqSet.Right is AlgebraComp && !eqSet.LeftTerm.Contains(eqSet.Right as AlgebraComp))
            {
                funcIden = eqSet.Right as AlgebraComp;
                _assignTo = eqSet.Left;
            }
            else
                return false;

            if (funcIden != null)
            {
                if (_assignTo.ToAlgTerm().Contains(funcIden))
                    return false;

                // The input variable for the function needs to be assumed.
                AlgebraComp[] useVars;
                if (_assignTo is Equation.Structural.LinearAlg.ExMatrix ||
                    _assignTo is Number)
                {
                    useVars = new AlgebraComp[] { new AlgebraComp(AlgebraVar.GarbageVar) };
                }
                else if (probSolveVar == funcIden.Var.Var)
                    return false;
                else
                {
                    useVars = new AlgebraComp[] { new AlgebraComp(probSolveVar) };
                    // For graphing later.
                    solveVars.Remove(funcIden.Var.Var);
                }

                _func = new FunctionDefinition(funcIden, useVars, null, false);
            }

            if (_assignTo == null || Number.IsUndef(_assignTo))
                return false;

            //if (_func.HasCallArgs)
            //    return false;

            _agSolver = new AlgebraSolver();
            _agSolver.CreateUSubTable(solveVars);

            List<string> solveVarKeys = (from solveVar in solveVars
                                         select solveVar.Key).Distinct().ToList();

            for (int i = 0; i < solveVarKeys.Count; ++i)
            {
                if (solveVarKeys[i] == probSolveVar)
                {
                    solveVarKeys.RemoveAt(i);
                    break;
                }
            }


            solveVarKeys.Insert(0, probSolveVar);

            List<string> tmpCmds = new List<string>();
            if (solveVars.Count == 1 && !_func.IsMultiValued && _func.InputArgs != null && _func.HasValidInputArgs)
            {
                AlgebraTerm term = _assignTo.ToAlgTerm();
                string graphStr = term.ToJavaScriptString(true);
                if (graphStr != null)
                    tmpCmds.Add("Graph");
            }

            if (!ContainsSpecialFuncs(new AlgebraTerm(_assignTo)) &&
                !_func.IsMultiValued &&
                _assignTo.ToAlgTerm().Contains(_func.InputArgs[0]))
            {
                tmpCmds.Add("Find inverse");
            }

            tmpCmds.Add(_func.HasValidInputArgs ? "Assign function" : "Assign value");
            for (int i = 0; i < solveVarKeys.Count; ++i)
            {
                tmpCmds.Add("Domain of " + solveVarKeys[i]);
            }

            _cmds = tmpCmds.ToArray();

            return true;
        }
    }
}