﻿using MathSolverWebsite.MathSolverLibrary;
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
                FuncDef = MathSolver.FinalizeOutput(WorkMgr.ToDisp(def.Value));
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
        private const string EXAMPLE_INDEX_SES_KEY = "ExampleIndex";
        private const string SUB_MESSAGE_SES_KEY = "SubKey";

        private const int MAX_INPUT_LEN = 200;
        private static ExampleInputMgr _exampleInputMgr = null;
        private static DataMgr _dataMgr = null;

        public static DataMgr DataMgr
        {
            get 
            {
                if (_dataMgr == null)
                    CreateDataMgr();
                return _dataMgr; 
            }
        }

        private string SubWarningMessage
        {
            set
            {
                Session[SUB_MESSAGE_SES_KEY] = value;
            }
            get
            {
                object subMes = Session[SUB_MESSAGE_SES_KEY];
                return subMes == null ? "" : (string)subMes;
            }
        }

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

        public int ExampleIndex
        {
            get
            {
                object indexObj = Session[EXAMPLE_INDEX_SES_KEY];
                return indexObj != null ? (int)indexObj : 0;
            }
            set
            {
                Session[EXAMPLE_INDEX_SES_KEY] = value;
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

        private void CreateExamples()
        {
            if (_exampleInputMgr == null)
            {
                _exampleInputMgr = new ExampleInputMgr(TopicsPage.Topics);
            }
        }

        private static void CreateDataMgr()
        {
            if (_dataMgr == null)
            {
                _dataMgr = new DataMgr();
                _dataMgr.Init();
            }
        }

        private static void GetInputData(string rawInput, out string input, out Boolean isCompact)
        {
            input = null;
            isCompact = false;

            string[] parts = rawInput.Split('?');

            //input = Server.HtmlDecode(parts[0]);
            input = parts[0];

            if (parts.Length < 2 || !bool.TryParse(parts[1], out isCompact))
                isCompact = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MathSolver.Init();

            if (!Page.IsPostBack)
            {
                CreateDataMgr();
                CreateExamples();

                this.FuncDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

                updateExampleTimer_Tick(null, null);

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
                    else
                        UseRad = true;

                    // The input passed through HTML GET is always in LaTeX therefore, there is never any plain text input.
                    if (UpdateUI(eqQueryStr, false, selIndex))
                        DisplaySolveResult();
                }
                else
                {
                    UseRad = true;
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
                IEnumerable<KeyValuePair<FunctionDefinition, ExComp>> allFuncDefs = funcDefHelper.GetAllDefinitions();
                funcDisps = (from funcDef in allFuncDefs
                             select new FuncDispConv(funcDef)).ToList();
            }

            functionDefsListView.DataSource = funcDisps;
            functionDefsListView.DataBind();
        }

        /// <summary>
        /// Fired whenever the user clicks the solve button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hiddenSolveBtn_Click(object sender, EventArgs e)
        {
            DisplaySolveResult();
        }

        /// <summary>
        /// Fired whenever the user changes the input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hiddenUpdateBtn_Click(object sender, EventArgs e)
        {
            string inputStr;
            Boolean isCompact;
            GetInputData(hiddenUpdateTxtBox.Text, out inputStr, out isCompact);
            inputStr = Server.HtmlDecode(inputStr);
            UpdateUI(inputStr, isCompact);
        }

        /// <summary>
        /// Calculates and displays the solve result depending on the text in hiddenUpdateTxtBox
        /// </summary>
        private void DisplaySolveResult()
        {
            // Store it, just to be safe.
            bool useRad = UseRad;

            if (FuncDefHelper == null)
                FuncDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

            MathSolverLibrary.TermType.EvalData evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), FuncDefHelper);

            string inputStr;
            Boolean isCompact;
            GetInputData(hiddenUpdateTxtBox.Text, out inputStr, out isCompact);
            inputStr = Server.HtmlDecode(inputStr);

            evalData.SetPlainTextInput(isCompact);

            var parseErrors = new System.Collections.Generic.List<string>();
            var termEval = MathSolver.ParseInput(inputStr, ref evalData, ref parseErrors);

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
            string selectedValue = evalDropDownList.SelectedValue;

            evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), FuncDefHelper);
            evalData.SetPlainTextInput(isCompact);
            solveResult = termEval.ExecuteCommandIndex(selectedIndex, ref evalData);

            string rawResultStr;
            string solveResultHtml = HtmlHelper.SolveResultToHtml(solveResult, out rawResultStr, evalData);
            string workHtml = "";

            if (solveResult.Success)
            {
                workHtml = HtmlHelper.OutputWorkStepsToHtml(evalData.GetWorkMgr());
            }

            string selectedVal = selectedIndex.ToString();
            if (selectedIndex < evalDropDownList.Items.Count && selectedIndex >= 0)
                selectedVal = evalDropDownList.Items[selectedIndex].Text;

            string inputHtml = "<div class='input-disp-txt'><span class='mathquill-rendered-math'>" + inputStr + "</span></div>";
            inputHtml += "<div class='selected-cmd-txt'>" + termEval.GetCommands()[selectedIndex] + "</div>";
            inputHtml += "<p class='hidden'>" + inputStr + "</p>";

            inputHtml = "<div class='input-disp-area'>" + inputHtml + "</div>";

            calcOutput.InnerHtml = inputHtml + solveResultHtml +
                "<div class='sub-message'>" + 
                SubWarningMessage + 
                "<div>" + 
                "To delete this definition go to the 'plus' button in the toolbar hover over the definition and click delete. Or click <a href='/'>here</a>" + 
                "</div>" +
                "</div>" + 
                workHtml;

            if (_dataMgr != null)
            {
                // Save the input and output to storage for further analysis.
                _dataMgr.CreateBlobData(inputStr, selectedValue, inputHtml, User.Identity.IsAuthenticated);
            }

            BindListView();
        }

        /// <summary>
        /// Updates the user interface according to the input. 
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="selectIndex"></param>
        /// <returns></returns>
        private bool UpdateUI(string inputStr, bool isCompact, int selectIndex = 0)
        {
            GenTermType genTermEval = null;
            bool inputSizeExceeded = inputStr.Length > MAX_INPUT_LEN;

            List<string> parseErrors = new List<string>();
            if (inputStr != null && inputStr != "" && !inputSizeExceeded)
            {
                if (FuncDefHelper == null)
                    FuncDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

                EvalData evalData = new EvalData(UseRad, new WorkMgr(), FuncDefHelper);
                // The view being compact means the user is using the regular keyboard to input. 
                // Therefore, change to plain text input.
                evalData.SetPlainTextInput(isCompact);

                genTermEval = MathSolver.ParseInput(inputStr, ref evalData, ref parseErrors);
                if (evalData.GetMsgs() != null)
                {
                    string subWarningMessage = "";
                    foreach (string message in evalData.GetMsgs())
                    {
                        if (message.StartsWith("Sub"))
                            subWarningMessage += message + ". ";
                    }
                    if (subWarningMessage != "")
                        SubWarningMessage = subWarningMessage;
                }
                else
                    SubWarningMessage = "";
            }

            evalDropDownList.Items.Clear();

            // Display the commands for the parsed input.
            if (genTermEval != null && genTermEval.GetCommands() != null)
            {
                string[] cmds = genTermEval.GetCommands();

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
                genTermEval = null;
                string parseErrorsTxt = "";
                for (int i = 0; i < parseErrors.Count; ++i)
                {
                    parseErrorsTxt += "<p>" + parseErrors[i] + "</p>";
                }
                if (parseErrors.Count == 0)
                    parseErrorsTxt = "<p>" + (inputSizeExceeded ? "Input is too long" : DEF_PARSE_ERR_MSG) + "</p>";
                parseErrorSpan.InnerHtml = parseErrorsTxt;

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

        protected void updateExampleTimer_Tick(object sender, EventArgs e)
        {
            if (_exampleInputMgr == null)
                return;
            // Get the next example.
            ExampleIndex = _exampleInputMgr.IncrementIndex(ExampleIndex);

            // Display the example.
            var example = _exampleInputMgr.GetExample(ExampleIndex);
            exampleOutputContent.InnerHtml = example.ToHtml();
        }

        protected void exampleNavBackBtn_Click(object sender, EventArgs e)
        {
            if (_exampleInputMgr == null)
                return;
            // Get the previous example.
            ExampleIndex = _exampleInputMgr.DecrementIndex(ExampleIndex);

            var example = _exampleInputMgr.GetExample(ExampleIndex);
            exampleOutputContent.InnerHtml = example.ToHtml();
        }

        protected void exampleNavForwardBtn_Click(object sender, EventArgs e)
        {
            if (_exampleInputMgr == null)
                return;
            // Get the next example.
            ExampleIndex = _exampleInputMgr.IncrementIndex(ExampleIndex);

            var example = _exampleInputMgr.GetExample(ExampleIndex);
            exampleOutputContent.InnerHtml = example.ToHtml();
        }


    }
}