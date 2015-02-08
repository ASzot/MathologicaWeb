using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MathSolverWebsite.WebsiteHelpers;

namespace MathSolverWebsite
{
    public partial class HelpTopicSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string searchStr = Request.QueryString["Search"];
                if (searchStr != null)
                    searchStr = HtmlHelper.CleanQueryStr(searchStr);

                if (searchStr == null)
                {
                    Response.Redirect("HelpPage");
                }

                searchStr = Server.UrlDecode(searchStr);

                searchTxtBox.Text = searchStr;

                var topicDatas = HelpPage.Topics.GetTopicDataMatches(searchStr);

                if (topicDatas.TopicDataInfo.Count == 0)
                    resultContentDiv.InnerHtml = "No results.";
                else
                    resultContentDiv.InnerHtml = HtmlHelper.TopicDataToHtmlList(topicDatas, "Matching Topics", Server);
            }
        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            string searchStr = searchTxtBox.Text;

            if (searchStr == "")
                Response.Redirect("HelpPage");

            Response.Redirect("HelpTopicSearch?Search=" + searchStr);
        }
    }
}