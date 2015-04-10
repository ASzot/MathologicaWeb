using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MathSolverWebsite.Website_Logic
{
    public static class TopicMgr
    {
        private const string FILE_PATH = "~/Content/Topics.xml";

        private static TopicDatas _topics = null;

        public static TopicDatas Topics
        {
            get
            {
                if (_topics == null)
                    LoadTopics();
                return _topics;
            }
        }

        public static bool LoadTopics()
        {
            if (_topics != null && _topics.TopicDataInfo.Count != 0)
                return true;

            _topics = new TopicDatas();
            return _topics.LoadTopics(HostingEnvironment.MapPath(FILE_PATH));
        }
    }
}