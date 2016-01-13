using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MathSolverWebsite.MathSolverLibrary;

namespace MathSolverWebsite.api
{
    public partial class Query : System.Web.UI.Page
    {
        private const string PASSWORD = "98ed7b4eee1c4a2385a0d9a98b7e1532";
        protected void Page_Load(object sender, EventArgs e)
        {
            string apiPass = Request.QueryString["p"];

            string queryExecute = Request.QueryString["qe"];
            string queryParse = Request.QueryString["qp"];
            string useRadStr = Request.QueryString["ur"];
            string selectedIndexStr = Request.QueryString["s"];

            bool useRad;
            int selectedIndex = -1;
            if (((queryExecute == null || queryExecute == "" || selectedIndexStr == null || selectedIndexStr == "" || 
                !int.TryParse(selectedIndexStr, selectedIndex)) && (queryParse == null || queryParse == "")) || 
                useRadStr == null || useRadStr == "" || Boolean.TryParse(useRadStr, out useRad) || 
                apiPass != PASSWORD)
            {
                Response.Redirect("/", true);
                return;
            }

            string output;
            if (queryExecute == null)
                output = EvalParse(queryParse, useRad);
            else
                output = EvalSolve(queryExecute, useRadd, selectedIndex);

            outputQuery.Text = output;
        }

        private string EvalParse(string input, bool useRad)
        {
            var funcDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

            var evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), funcDefHelper);
            var parseErrors = new List<string>();
            var termEval = MathSolverLibrary.MathSolver.ParseInput(inputTxt, ref evalData, ref parseErrors);
            if (termEval == null)
            {
                return "ERROR:" + String.Join("|", parseErrors);
            }
            string[] cmds = termEval.GetCommands();

            return String.Join("|", cmds);
        }

        private string EvalSolve(string input, bool useRad, int selectedIndex)
        {
            var funcDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

            var evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), funcDefHelper);

            var parseErrors = new System.Collections.Generic.List<string>();
            var termEval = MathSolver.ParseInput(inputTxt, ref evalData, ref parseErrors);
            if (termEval == null)
                return "ERROR:" + String.Join("|", parseErrors);
            evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), FuncDefHelper);
            var solveResult = termEval.ExecuteCommandIndex(selectedIndex, ref evalData);
            if (solveResult == null || !solveResult.Success)
                return "ERROR:EVAL";

            string rawResultStr;
            string solveResultHtml = HtmlHelper.SolveResultToHtml(solveResult, out rawResultStr, evalData);

            string workHtml = HtmlHelper.OutputWorkStepsToHtml(evalData.GetWorkMgr());

            return solveResultHtml + workHtml;
        }
    }
}