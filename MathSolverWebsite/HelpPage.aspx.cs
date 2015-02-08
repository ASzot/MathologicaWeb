using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MathSolverWebsite.WebsiteHelpers;

namespace MathSolverWebsite
{
	public partial class HelpPage : System.Web.UI.Page
	{
        public const string EXAMPLE_FILE_PATH = "~/Content/Topics.xml";

        private static TopicDatas _topics = null;
        private static bool _topicLoadFailure = false;

        public static TopicDatas Topics
        {
            get { return _topics; }
            set { _topics = value; }
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            searchTxtBox.Focus();

            if (_topics == null)
                _topics = new TopicDatas();

            _topicLoadFailure = !_topics.LoadTopics(Server.MapPath(EXAMPLE_FILE_PATH));
            topicContentDiv.InnerHtml = HtmlHelper.TopicDataToHtmlTree(_topics, "All Topics", Server);
		}

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            string searchStr = searchTxtBox.Text;

            if (searchStr == "")
                return;

            Response.Redirect("HelpTopicSearch?Search=" + searchStr);
        }
	}
}