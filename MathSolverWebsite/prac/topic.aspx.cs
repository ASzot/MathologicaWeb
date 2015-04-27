using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MathSolverWebsite.Website_Logic;

namespace MathSolverWebsite.prac
{
    public partial class TopicPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string topicStr = Request.QueryString["Name"];
            if (topicStr != null)
                topicStr = HtmlHelper.CleanQueryStr(topicStr);

            if (topicStr == null)
                Response.Redirect("/topics");

            topicStr = Server.UrlDecode(topicStr);

            Title = topicStr;

            TopicData? nlblTopicData = TopicsPage.Topics.GetTopic(topicStr);

            if (nlblTopicData == null)
                Response.Redirect("/topics");

            TopicData topicData = nlblTopicData.Value;

            contentDiv.InnerHtml = topicData.ToHtml(Server, false);

            Title = topicData.DispName;
        }
    }
}