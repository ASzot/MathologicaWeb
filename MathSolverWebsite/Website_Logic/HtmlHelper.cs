using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MathSolverWebsite.MathSolverLibrary;
using MathSolverWebsite.MathSolverLibrary.Equation;

namespace MathSolverWebsite.Website_Logic
{
    static class HtmlHelper
    {
        private static string RestrictionToHtml(Restriction rest)
        {
            string final = "";

            final += "<div>";
            final += "<p>Restriction</p>";
            final += "<p>" + WorkMgr.STM + rest.ToMathAsciiStr() + WorkMgr.EDM + "</p>";
            final += "</div>";

            final = MathSolver.FinalizeOutput(final);

            return final;
        }

        private static string SolSolveForToHtml(Solution sol)
        {
            string final = "";
            if (sol.SolveFor != null)
            {
                final += sol.SolveFor.ToTexString();
                final += " " + sol.ComparisonOpToTexStr() + " ";
            }

            return final;
        }

        private static string WorkStepToHtml(WorkStep workStep)
        {
            string final = "";

            if (workStep.WorkHtml != null || workStep.WorkDesc != null || (workStep.SubWorkSteps != null && workStep.SubWorkSteps.Count != 0))
                final += "<li class='workListItem'>";

            if (workStep.WorkHtml != null)
                final += "<p>" + workStep.WorkHtml + "</p>";

            if (workStep.WorkDesc != null)
                final += "<p class='workDescText'>" + workStep.WorkDesc + "</p>";

            if (workStep.SubWorkSteps != null && workStep.SubWorkSteps.Count != 0)
            {
                final += "<input type='button' class='sub-work-list-toggle-btn' value='+ Show Work Steps' />";
                final += "<ul class='sub-work-list' style='display: none;'>";
                foreach (WorkStep subWorkStep in workStep.SubWorkSteps)
                {
                    final += WorkStepToHtml(subWorkStep);
                }
                final += "</ul>";
            }

            if (final != "")
                final += "</li>";

            return final;
        }

        private static string SolutionToHtml(Solution sol)
        {
            string final = "";
            final += "<p class='solutionHeader'>Solution</p>";

            string solSolveFor = SolSolveForToHtml(sol);
            string solveForStr = sol.SolveFor != null ? sol.SolveFor.ToDispString() : "";

            if (sol.Result != null)
                final += "<div class='solutionData'>" + WorkMgr.STM + solSolveFor + sol.ResultToMathAsciiStr() + WorkMgr.EDM + "</div>";
            if (sol.GeneralResult != null)
                final += "<div class='solutionData'>" + WorkMgr.STM + solSolveFor + sol.GeneralToMathAsciiStr() + WorkMgr.EDM + "</p>";
            if (sol.ApproximateResult != null)
            {
                final += "<div class='solutionData'>" + WorkMgr.STM + solveForStr + "\\approx" + sol.ApproximateToMathAsciiStr() + WorkMgr.EDM + "</p>";
            }
            if (sol.Multiplicity != 1)
                final += "<p class='solutionData'>Multiplicity of " + sol.Multiplicity.ToString() + "</p>";

            final = MathSolver.FinalizeOutput(final);

            return final;
        }

        public static string SolveResultToHtml(SolveResult sr, out string rawSolStr, MathSolverLibrary.TermType.EvalData evalData)
        {
            rawSolStr = "";
            string final = "";

            final += "<ul class='sol-list'>";
            if (sr.Success)
            {
                if (evalData.GraphEqStrs != null && evalData.GraphVar != null)
                {
                    string finalJavascript = "";
                    finalJavascript += "<script>";
                    finalJavascript += "var grapher = JXG.JSXGraph.initBoard('graphbox', {boundingbox: [-10, 10, 10, -10], axis: true});";
                    foreach (string graphEqStr in evalData.GraphEqStrs)
                    {
                        finalJavascript += "grapher.create('functiongraph', [function(" + evalData.GraphVar + ") { return " + graphEqStr + ";}]);";
                    }
                    finalJavascript += "</script>";
                    final += finalJavascript;
                    final += "<li class='resultListItem graphListItem noselect'>";
                    final += "<div id='graphbox' class='graph-disp'></div>";
                    final += "</li>";
                }
                if (evalData.Msgs != null)
                {
                    foreach (var msg in evalData.Msgs)
                    {
                        final += "<li class='resultListItem'>";
                        final += "<p>" + msg + "</p>";
                        final += "</li>";
                    }
                }
                if (evalData.HasPartialSolutions)
                {
                    final += "<li class='resultListItem'>";
                    final += "<p>Partial Solution</p>";
                    final += "</li>";

                    for (int i = 0; i < evalData.PartialSolutions.Count; ++i)
                    {
                        string dispStr = WorkMgr.STM + evalData.PartialSolToMathAsciiStr(i) + WorkMgr.EDM;
                        final += "<li class='resultListItem'>";
                        final += "<p>" + dispStr + "</p>";
                        final += "</li>";
                        rawSolStr += "Partial Solution : " + dispStr;
                    }
                }

                if (sr.Solutions != null)
                {
                    foreach (Solution sol in sr.Solutions)
                    {
                        string dispStr = SolutionToHtml(sol);
                        final += "<li class='resultListItem'>";
                        final += dispStr;
                        final += "</li>";
                        rawSolStr += "Solution: " + dispStr;
                    }
                }

                if (sr.Restrictions != null)
                {
                    foreach (Restriction rest in sr.Restrictions)
                    {
                        string dispStr = RestrictionToHtml(rest);
                        final += "<li class='resultListItem'>";
                        final += dispStr;
                        final += "</li>";
                        rawSolStr += "Restriction: " + dispStr;
                    }
                }

                IEnumerable<string> iterVarStrs = sr.GetIterationVarStrs();
                foreach (string iterVarStr in iterVarStrs)
                {
                    final += "<li class='resultListItem'>";
                    final += "<p>Where " + WorkMgr.STM + iterVarStr + "\\in\\mathbb{Z}" + WorkMgr.EDM + "</p>";
                    final += "</li>";
                }

                bool showZHint = final.Contains("\\mathbb{Z}");
                bool showRHint = final.Contains("\\mathbb{R}");

                if (showZHint || showRHint)
                {
                    final += "<li class='resultListItem'>";
                    if (showZHint)
                        final += "<p class='sideNoteText'>(`\\mathbb{Z}` is the set of all integers)</p>";
                    if (showRHint)
                        final += "<p class='sideNoteText'>(`\\mathbb{R}` is the set of all real numbers)</p>";
                    final += "</li>";
                }
            }
            else
            {
                rawSolStr += "Failure";
                final += "<li class='resultListItem'>";

                final += "<p class='error'>Failure</p>";
                final += "<p>";
                foreach (var failure in evalData.FailureMsgs)
                {
                    final += failure + "<br />";
                }
                final += "</p>";

                final += "</li>";
            }

            final += "</ul>";
            final += "</p>";

            return final;
        }

        public static string CleanQueryStr(string queryStr)
        {
            return queryStr.Replace(".aspx", "").Replace(".html", "");
        }

        public static string OutputWorkStepsToHtml(WorkMgr workMgr)
        {
            if (workMgr.WorkSteps.Count == 0)
                return "";

            string final = "";

            final += 
                "<p class='sectionHeading'>" +
                "<input class='workCollapseBtn' type='button' value='Work'></input>" + 
                "</p>";

            final += "<ul id='workDisplayList' class='work-disp-list'>";

            foreach (WorkStep workStep in workMgr.WorkSteps)
            {
                final += WorkStepToHtml(workStep);
            }

            final += "</ul>";

            return final;
        }

        public static string TopicDataToHtmlList(TopicDatas topics, string headerStr, HttpServerUtility server)
        {
            string html = "<p class='sectionHeading'>" + headerStr + "</p>";

            html += "<ul>";

            foreach (TopicData topicData in topics.TopicDataInfo)
            {
                html += "<li class='helpTopicListItem' style='margin-bottom: 30px;'>" + topicData.ToHtml(server) + "</li>";
            }

            html += "</ul>";

            return html;
        }

        public static string TopicDataToHtmlTree(List<TopicPath> topics, string baseUrl, HttpServerUtility server)
        {
            string html = "";

            //html += "<ul class='collapsible-list' id='collapseUl'>";

            string trashVal;
            html += GetFormattedTopicTree(topics, 0, server, baseUrl, out trashVal);

            //html += "</ul>";

            return html;
        }

        private static string GetFormattedTopicTree(List<TopicPath> topicDatas, int currentBranch, HttpServerUtility server, string baseUrl, out string branchName)
        {
            branchName = null;
            if (topicDatas.Count == 1)
            {
                string[] branch = topicDatas[0].Path.Split('/');
                if (currentBranch == branch.Length)
                {
                    branchName = branch[currentBranch - 1];

                    string hintPopup = topicDatas[0].GetHintStr() == "" ? topicDatas[0].DispName : topicDatas[0].GetHintStr();

                    return "<div class='slink-div'><a class='specific-link' href='" + baseUrl + "?Name=" + server.UrlEncode(topicDatas[0].DispName) + "'>"
                        + branch[currentBranch - 1] + "</a></div>";
                }
            }

            // Split by branch.
            Dictionary<string, List<TopicPath>> topicDataTree = new Dictionary<string, List<TopicPath>>();

            foreach (TopicPath topicData in topicDatas)
            {
                string[] branch = topicData.Path.Split('/');
                string currentBranchStr = branch[currentBranch];
                if (!topicDataTree.ContainsKey(currentBranchStr))
                    topicDataTree.Add(currentBranchStr, new List<TopicPath>());
                topicDataTree[currentBranchStr].Add(topicData);
            }

            string final = "";
            currentBranch++;
            foreach (var topicDataBranch in topicDataTree)
            {
                string nextBranch;
                string nextFormatted = GetFormattedTopicTree(topicDataBranch.Value, currentBranch, server, baseUrl, out nextBranch);

                if (nextBranch == null || nextBranch != topicDataBranch.Key)
                {
                    int spacing = currentBranch * 10;
                    final += "<div class='topic-container' style='margin-left: " + spacing.ToString() + "px;'>";
                    final += "<h4>" + topicDataBranch.Key + "</h4>";
                    final += "<div>" + nextFormatted + "</div>";
                    final += "</div>";
                }
                else
                    final += nextFormatted;
            }

            return final;
        }
    }
}