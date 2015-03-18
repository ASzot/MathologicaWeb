using MathSolverWebsite.MathSolverLibrary;
using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.WebsiteHelpers;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MathSolverWebsite
{
    public partial class _Default : Page
    {
        private const string DEF_PARSE_ERR_MSG = "Invalid input";

        private const string RAD_SES_KEY = "UseRad";
        private const string LAST_INPUT_SES_KEY = "LastInput";
        private const string PARSE_CONTINUE_SES_KEY = "ParseContinue";
        private const string SOLVE_CONTINUE_SES_KEY = "SolveContinue";
        private const string FUNC_DEF_SES_KEY = "FuncDefs";

        private const int MAX_INPUT_LEN = 200;
        private DataMgr _dataMgr = null;

        private MathSolverLibrary.Information_Helpers.FuncDefHelper FuncDefHelper
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

        public bool ParseContinue
        {
            get
            {
                object parseContinue = Session[PARSE_CONTINUE_SES_KEY];
                return parseContinue != null ? (bool)parseContinue : true; 
            }
            set
            {
                Session[PARSE_CONTINUE_SES_KEY] = value;
            }
        }

        public bool SolveContinue
        {
            get
            {
                object solveContinue = Session[SOLVE_CONTINUE_SES_KEY];
                return solveContinue != null ? (bool)solveContinue : true;
            }
            set
            {
                Session[SOLVE_CONTINUE_SES_KEY] = value;
            }
        }

        public string GenerateURLForInput(string inputStr, int evalIndex)
        {
            string domainStr = "www.mathologica.com/Default";
            return domainStr + "?Index=" + Server.UrlEncode(evalIndex.ToString()) + "&InputDisp=" + Server.UrlEncode(inputStr);
        }

        protected void hiddenDisplayBtn_Click(object sender, EventArgs e)
        {
            DisplaySolveResult();
        }

        protected void hiddenUpdate_Click(object sender, EventArgs e)
        {
            string inputTxt = Server.HtmlDecode(hiddenInputTxtBox.Text);
            UpdateUI(inputTxt);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            contentUpdate.UpdateMode = UpdatePanelUpdateMode.Conditional;

            // Load the help topics.
            if (HelpPage.Topics == null)
            {
                HelpPage.Topics = new TopicDatas();
                HelpPage.Topics.LoadTopics(Server.MapPath(HelpPage.EXAMPLE_FILE_PATH));
            }

            // For uploading analyze data to the cloud.
            if (_dataMgr == null)
            {
                _dataMgr = new DataMgr();
                _dataMgr.Init();
            }

            MathSolver.Init();

            if (!Page.IsPostBack)
            {
                ParseContinue = true;
                SolveContinue = true;

                string eqQueryStr = Request.QueryString["InputDisp"];
                string optionSelStr = Request.QueryString["Index"];
                string useRadStr = Request.QueryString["UseRad"];

                bool useRad = true;

                if (eqQueryStr != null)
                {
                    eqQueryStr = HtmlHelper.CleanQueryStr(eqQueryStr);
                    hiddenInputTxtBox.Text = eqQueryStr;

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
                            useRad = tmpUseRad;
                            UseRad = useRad;
                        }
                    }

                    if (UpdateUI(eqQueryStr, selIndex))
                        DisplaySolveResult();
                }
                else
                {
                    evaluteDropList.Items.Clear();
                    evaluteDropList.Items.Add("Enter input above.");
                }

                // Using radians is the default.
                if (!useRad)
                    degRadBtn.Checked = true;
                UseRad = useRad;
            }
        }

        private void DisplaySolveResult()
        {
            if (!SolveContinue || !ParseContinue)
            {
                HideLoading();
                // Clear the result section as well to not provide an incorrect answer.
                resultSectionDiv.InnerHtml = "";
                workSectionDiv.InnerHtml = "";

                return;
            }

            /////////////////////////////
            // Disable other solving.
            SolveContinue = false;

            // Store it, just to be safe. (The session variable could potentially change in the amount of the time 
            // the variable is accessed again, if the user is crafty.
            bool useRad = UseRad;

            if (FuncDefHelper == null)
                FuncDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

            MathSolverLibrary.TermType.EvalData evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), FuncDefHelper);
            string inputTxt = Server.HtmlDecode(hiddenInputTxtBox.Text);
            var parseErrors = new System.Collections.Generic.List<string>();
            var termEval = MathSolver.ParseInput(inputTxt, ref evalData, ref parseErrors);

            if (termEval == null)
            {
                HideLoading();
                // Clear the result section as well to not provide an incorrect answer.
                resultSectionDiv.InnerHtml = "";
                workSectionDiv.InnerHtml = "";

                return;
            }

            resultSectionDiv.Visible = true;
            workSectionDiv.Visible = true;

            SolveResult solveResult;

            if (evaluteDropList.SelectedIndex == -1)
            {
                HideLoading();
                return;
            }

            int selectedIndex = evaluteDropList.SelectedIndex;

            evalData = new MathSolverLibrary.TermType.EvalData(useRad, new WorkMgr(), FuncDefHelper);
            solveResult = termEval.ExecuteCommandIndex(selectedIndex, ref evalData);

            string rawResultStr;
            string solveResultHtml = HtmlHelper.SolveResultToHtml(solveResult, out rawResultStr, evalData);
            string workHtml = "";

            if (solveResult.Success)
            {
                workHtml = HtmlHelper.OutputWorkStepsToHtml(evalData.WorkMgr);

                if (LastInput != null)
                {
                    string linkStr = GenerateURLForInput(LastInput, selectedIndex);
                    workHtml +=
                        "<div style='padding-left: 10px; border-left: 1px solid #1592db; margin-top: 60px; font-size: 0.8em;'>" +
                        "<label>Share this problem:</label>" +
                        "<input style='font-size: 0.7em; margin-left: 10px; width:90%; max-width:250px;' type='text' value='" + linkStr + "' readonly></input>" +
                        "</div>";
                }
            }

            resultSectionDiv.InnerHtml = solveResultHtml;
            workSectionDiv.InnerHtml = workHtml;

            string selectedVal = selectedIndex.ToString();
            if (selectedIndex < evaluteDropList.Items.Count && selectedIndex >= 0)
                selectedVal = evaluteDropList.Items[selectedIndex].Text;

            //_dataMgr.CreateBlobData(LastInput ?? "null", selectedVal, rawResultStr);

            //var graphEqSets = termEval.GetDispGraphs();
            //string finalStr = "";
            //if (graphEqSets != null)
            //{
            //    for (int i = 0; i < graphEqSets.Count; ++i)
            //    {
            //        var graphEqSet = graphEqSets[i];
            //        finalStr += graphEqSet.FinalToTexString();
            //        if (i != graphEqSets.Count - 1)
            //            finalStr += ";";
            //    }
            //}

            //graphFuncTxtBox.Value = finalStr;

            ////////////////////////
            // Enable other solving.
            SolveContinue = true;

            HideLoading();
        }

        private void HideLoading()
        {
            loadingDiv.Attributes.Add("class", "hidden");
        }

        private bool UpdateUI(string inputStr, int selectIndex = 0)
        {
            string lastInput = LastInput;
            if (!SolveContinue || !ParseContinue || lastInput == inputStr)
            {
                if (!(lastInput == inputStr && evaluteDropList.Items.Count == 1 &&
                    DEF_PARSE_ERR_MSG == evaluteDropList.Items[0].Text))
                    parseErrorSpan.InnerHtml = "";

                if (lastInput != inputStr)
                {
                    evaluteDropList.Items.Clear();
                    evaluteDropList.Items.Add("Please wait...");
                }
                return false;
            }

            bool inputSizeExceeded = inputStr.Length > MAX_INPUT_LEN;

            MathSolverLibrary.TermType.TermType termEval = null;

            ParseContinue = false;
            LastInput = inputStr;

            System.Collections.Generic.List<string> parseErrors = new System.Collections.Generic.List<string>();

            if (inputStr != null && inputStr != "" && !inputSizeExceeded)
            {
                if (FuncDefHelper == null)
                    FuncDefHelper = new MathSolverLibrary.Information_Helpers.FuncDefHelper();

                MathSolverLibrary.TermType.EvalData evalData = new MathSolverLibrary.TermType.EvalData(UseRad, new WorkMgr(), FuncDefHelper);

                termEval = MathSolver.ParseInput(inputStr, ref evalData, ref parseErrors);
            }
            ParseContinue = true;

            // Update with the new commands.
            evaluteDropList.Items.Clear();

            // Display the commands for the parsed input.
            if (termEval != null)
            {
                parseErrorSpan.InnerHtml = "";
                string[] cmds = termEval.GetCommands();
                for (int i = 0; i < cmds.Length; ++i)
                {
                    string cmd = cmds[i];
                    ListItem listItem = new ListItem(cmd, i.ToString(), true);
                    evaluteDropList.Items.Add(listItem);
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
                evaluteDropList.Items.Add(inputSizeExceeded ? "Input is too long." : DEF_PARSE_ERR_MSG);

                return false;
            }

            // The first one is selected by default.
            evaluteDropList.Items[selectIndex].Selected = true;

            return true;
        }

        protected void angleRadBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radBtn = (RadioButton)sender;

            bool useRad;
            if (radBtn.ID == "degRadBtn")
                useRad = !radBtn.Checked;
            else
                useRad = radBtn.Checked;

            UseRad = useRad;
        }
    }
}