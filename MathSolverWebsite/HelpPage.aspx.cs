using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Hosting;

using MathSolverWebsite.WebsiteHelpers;

namespace MathSolverWebsite
{
	public partial class HelpPage : System.Web.UI.Page
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



		protected void Page_Load(object sender, EventArgs e)
		{
            searchTxtBox.Focus();

            if (LoadTopics())
                topicContentDiv.InnerHtml = HtmlHelper.TopicDataToHtmlTree(_topics.TopicDataInfo.Cast<TopicPath>().ToList(), "All Topics", "HelpTopic", Server);
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

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            string searchStr = searchTxtBox.Text;

            if (searchStr == "")
                return;

            Response.Redirect("HelpTopicSearch?Search=" + searchStr);
        }
	}
}