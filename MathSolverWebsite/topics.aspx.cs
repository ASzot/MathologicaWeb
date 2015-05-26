using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Hosting;

using MathSolverWebsite.Website_Logic;

namespace MathSolverWebsite
{
    public partial class TopicsPage : System.Web.UI.Page
    {
        public const string EXAMPLE_FILE_PATH = "~/Content/Topics.xml";
        private static TopicDatas _topics = null;

        public static TopicDatas Topics
        {
            get
            {
                if (_topics == null)
                    LoadTopics();
                return _topics;
            }
            set { _topics = value; }
        }

        public static bool LoadTopics()
        {
            if (_topics == null)
            {
                _topics = new TopicDatas();

                return _topics.LoadTopics(HostingEnvironment.MapPath(EXAMPLE_FILE_PATH));
            }

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string topicStr = Request.QueryString["tn"];
                if (topicStr == null)
                    Response.Redirect("~/practice");
                if (topicStr.Contains("."))
                    topicStr = topicStr.Split('.')[0];
                topicStr = HttpUtility.HtmlDecode(topicStr);
                _topics = null;
                if (LoadTopics())
                {
                    var dispTopics = _topics.GetTopics(topicStr);
                    practiceTopicTitleId.InnerText = topicStr;
                    rightHelp.InnerHtml = HtmlHelper.TopicDataToHtmlTree(dispTopics.Cast<TopicPath>().ToList(), "prac/topic", Server);
                }
            }
        }
    }
}