using MathSolverWebsite.MathSolverLibrary;
using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Information_Helpers;
using MathSolverWebsite.MathSolverLibrary.TermType;
using MathSolverWebsite.Website_Logic;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using System.Web.Security;

namespace MathSolverWebsite
{
    public partial class _Default : Page
    {
        private class FuncDispConv
        {
            public string FuncName
            {
                get;
                set;
            }

            public string FuncDef
            {
                get;
                set;
            }

            public FuncDispConv(KeyValuePair<FunctionDefinition, ExComp> def)
            {
                FuncName = def.Key.ToDispString();
                FuncDef = WorkMgr.ExFinalToAsciiStr(def.Value);
            }

            public FuncDispConv(string funcName, string funcDef)
            {
                FuncName = funcName;
                FuncDef = funcDef;
            }
        }

        private const string DEF_PARSE_ERR_MSG = "Invalid input";
        private const string SAVE_PROB_DATABASE_NAME = "SavedProblems";

        public const string RAD_SES_KEY = "UseRad";
        private const string LAST_INPUT_SES_KEY = "LastInput";
        private const string PARSE_CONTINUE_SES_KEY = "ParseContinue";
        private const string SOLVE_CONTINUE_SES_KEY = "SolveContinue";
        private const string FUNC_DEF_SES_KEY = "FuncDefs";

        private const int MAX_INPUT_LEN = 200;
        private static int _bindCount = 0;


        private FuncDefHelper FuncDefHelper
        {
            set
            {
                Session[FUNC_DEF_SES_KEY] = value;
            }
            get
            {
                object funcDefHelperObj = Session[FUNC_DEF_SES_KEY];
                return funcDefHelperObj != null ? (MathSolverLibrary.Information_Helpers.FuncDefHelper)funcDefHelperObj : null;
            }
        }

        public bool UseRad
        {
            get
            {
                object useRadObj = Session[RAD_SES_KEY];
                return useRadObj != null ? (bool)useRadObj : true;
            }
            set
            {
                Session[RAD_SES_KEY] = value;
            }
        }

        public string LastInput
        {
            get
            {
                object lastInputObj = Session[LAST_INPUT_SES_KEY];
                return lastInputObj != null ? (string)lastInputObj : null;
            }
            set
            {
                Session[LAST_INPUT_SES_KEY] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MathSolver.Init();
            UseRad = true;

            if (!Page.IsPostBack)
            {
                string eqQueryStr = Request.QueryString["InputDisp"];
                string optionSelStr = Request.QueryString["Index"];
                string useRadStr = Request.QueryString["UseRad"];

                if (eqQueryStr != null)
                {
                    eqQueryStr = HtmlHelper.CleanQueryStr(eqQueryStr);
                    hiddenUpdateTxtBox.Text = Server.HtmlEncode(eqQueryStr);

                    int selIndex = 0;
                    if (optionSelStr != null)
                    {
                        int.TryParse(optionSelStr, out selIndex);
                        optionSelStr = HtmlHelper.CleanQueryStr(optionSelStr);
                    }

                    if (useRadStr != null)
                    {
                        bool tmpUseRad;
                        useRadStr = HtmlHelper.CleanQueryStr(useRadStr);
                        if (bool.TryParse(useRadStr, out tmpUseRad))
                        {
                            UseRad = tmpUseRad;
                        }
                    }

                    if (UpdateUI(eqQueryStr, selIndex))
                        DisplaySolveResult();
                }
                else
                {
                    evalDropDownList.Items.Clear();
                    evalDropDownList.Items.Add("Enter input above.");
                }

                radRadBtn.Checked = UseRad;
                degRadBtn.Checked = !UseRad;

                BindListView();
            }
        }

        private void BindListView()
        {
            FuncDefHelper funcDefHelper = FuncDefHelper;
            List<FuncDispConv> funcDisps;
            if (funcDefHelper == null)
            {
                funcDisps = new List<FuncDispConv>();
            }
            else
            {
                IEnumerable<KeyValuePair<FunctionDefinition, ExComp>> allFuncDefs = funcDefHelper.AllDefinitions;
                funcDisps = (from funcDef in allFuncDefs
                             select new FuncDispConv(funcDef)).ToList();
            }

            functionDefsListView.DataSource = funcDisps;
            functionDefsListView.DataBind();
        }

        protected void hiddenSolveBtn_Click(object sender, EventArgs e)
        {
            DisplaySolveResult();
        }

        protected void hiddenUpdateBtn_Click(object sender, EventArgs e)
        {
            string inputStr = Server.HtmlDecode(hiddenUpdateTxtBox.Text);
            UpdateUI(inputStr);
        }

        private void DisplaySolveResult()
        {
            // Store it, just to be safe. (The session variable could potentially change in the amount of the time 
            // the variable is accessed again, if the user is crafty.
            bool useRad = UseRad;

            if (FuncDefHelper == null)
                FuncDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

            MathSolverLibrary.TermType.EvalData evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), FuncDefHelper);

            string inputTxt = Server.HtmlDecode(hiddenUpdateTxtBox.Text);
            var parseErrors = new System.Collections.Generic.List<string>();
            var termEval = MathSolver.ParseInput(inputTxt, ref evalData, ref parseErrors);

            if (termEval == null)
            {
                return;
            }

            SolveResult solveResult;

            if (evalDropDownList.SelectedIndex == -1)
            {
                return;
            }

            int selectedIndex = evalDropDownList.SelectedIndex;

            evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), FuncDefHelper);
            solveResult = termEval.ExecuteCommandIndex(selectedIndex, ref evalData);

            string rawResultStr;
            string solveResultHtml = HtmlHelper.SolveResultToHtml(solveResult, out rawResultStr, evalData);
            string workHtml = "";

            if (solveResult.Success)
            {
                workHtml = HtmlHelper.OutputWorkStepsToHtml(evalData.WorkMgr);
            }

            string selectedVal = selectedIndex.ToString();
            if (selectedIndex < evalDropDownList.Items.Count && selectedIndex >= 0)
                selectedVal = evalDropDownList.Items[selectedIndex].Text;

            string inputHtml = "<div class='input-disp-txt'>`" + inputTxt + "`</div>";
            inputHtml += "<div class='selected-cmd-txt'>" + termEval.GetCommands()[selectedIndex] + "</div>";
            inputHtml += "<p class='hidden'>" + inputTxt + "</p>";

            inputHtml = "<div class='input-disp-area'>" + inputHtml + "</div>";

            calcOutput.InnerHtml = inputHtml + solveResultHtml + workHtml;

            BindListView();
        }

        private bool UpdateUI(string inputStr, int selectIndex = 0)
        {
            TermType termEval = null;
            bool inputSizeExceeded = inputStr.Length > MAX_INPUT_LEN;

            List<string> parseErrors = new List<string>();
            if (inputStr != null && inputStr != "" && !inputSizeExceeded)
            {
                if (FuncDefHelper == null)
                    FuncDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

                EvalData evalData = new EvalData(UseRad, new WorkMgr(), FuncDefHelper);

                termEval = MathSolver.ParseInput(inputStr, ref evalData, ref parseErrors);
            }

            evalDropDownList.Items.Clear();

            // Display the commands for the parsed input.
            if (termEval != null && termEval.GetCommands() != null)
            {
                string[] cmds = termEval.GetCommands();

                parseErrorSpan.InnerHtml = "";
                for (int i = 0; i < cmds.Length; ++i)
                {
                    string cmd = cmds[i];
                    ListItem listItem = new ListItem(cmd, i.ToString(), true);
                    evalDropDownList.Items.Add(listItem);
                }
            }
            else  // Display the appropriate error message.
            {
                termEval = null;
                string parseErrorsTxt = "";
                for (int i = 0; i < parseErrors.Count; ++i)
                {
                    parseErrorsTxt += "<p>" + parseErrors[i] + "</p>";
                }
                parseErrorSpan.InnerHtml = parseErrorsTxt;
                evalDropDownList.Items.Add(inputSizeExceeded ? "Input is too long" : DEF_PARSE_ERR_MSG);

                return false;
            }


            evalDropDownList.Items[selectIndex].Selected = true;

            return true;
        }

        protected void hiddenSaveProbBtn_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
                return;

            string saveProbTxt = Server.HtmlDecode(hiddenSavedProblemTxtBox.Text);
            string saveProbDate = Server.HtmlDecode(hiddenTimeTxtBox.Text);
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);
            Guid userId = (Guid)currentUser.ProviderUserKey;

            SaveProblem(saveProbTxt, userId.ToString("N"), saveProbDate);
        }

        private void SaveProblem(string problem, string userId, string time)
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string queryStr = "INSERT INTO SavedProblems (id, problem, entry_time) VALUES (@id, @prob, @time)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(queryStr, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.Parameters.AddWithValue("@prob", problem);
                        cmd.Parameters.AddWithValue(@"time", time);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 1)
                        {
                            // Success.
                        }
                        else
                        {
                            // Failure.
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void angleRadBtn_CheckedChanged(object sender, EventArgs e)
        {
            UseRad = !degRadBtn.Checked;
        }

        protected void functionDefsListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string funcIdentifier = (string)e.CommandArgument;
                int removeIndex = e.Item.DisplayIndex;
                functionDefsListView.DeleteItem(removeIndex);
                FuncDefHelper.Remove(funcIdentifier);
                BindListView();
            }
        }

        protected void functionDefsListView_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
        }


    }
}