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

			if ((queryExecute != null && queryExecute == "") ||
				(queryParse != null && queryParse == "") ||
				(selectedIndexStr != null && (selectedIndexStr == "" || !int.TryParse(selectedIndexStr, out selectedIndex))) ||
				(useRadStr == null || !Boolean.TryParse(useRadStr, out useRad)) ||
				apiPass != PASSWORD)
			{
				Response.Redirect("/", true);
				return;
			}
 				

            string output;
            if (queryExecute == null && queryParse != null)
                output = EvalParse(queryParse, useRad);
			else if (queryParse == null && queryExecute != null)
				output = EvalSolve(queryExecute, useRad, selectedIndex);
			else
			{
				Response.Redirect("/", true);
				return;
			}
            outputDiv.InnerHtml = output;
        }

        private string EvalParse(string input, bool useRad)
        {
            var funcDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

            var evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), funcDefHelper);
            var parseErrors = new List<string>();
            var termEval = MathSolverLibrary.MathSolver.ParseInput(input, ref evalData, ref parseErrors);
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
            var termEval = MathSolver.ParseInput(input, ref evalData, ref parseErrors);
            if (termEval == null)
                return "ERROR:" + String.Join("|", parseErrors);
            evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), funcDefHelper);
            var solveResult = termEval.ExecuteCommandIndex(selectedIndex, ref evalData);
            if (!solveResult.Success)
                return "ERROR:EVAL";

            string rawResultStr;
            string solveResultHtml = MathSolverWebsite.Website_Logic.HtmlHelper.SolveResultToHtml(solveResult, out rawResultStr, evalData);

            string workHtml = MathSolverWebsite.Website_Logic.HtmlHelper.OutputWorkStepsToHtml(evalData.GetWorkMgr());

            return solveResultHtml + workHtml;
        }
    }
}