using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MathSolverWebsite.Website_Logic
{
    public struct TopicData : TopicPath
    {
        private const int EXAMPLE_LIMIT = 3;
        private const int MAX_DESC_PREV_LENGTH = 400;
        private IEnumerable<TopicExample> _exampleInputs;
        private string _info;
        private string _path;
        private string s_descTxt;
        private string s_dispName;

        public string BasePath
        {
            get { return _path.Split('/')[0].Replace("_", " "); }
        }

        public string DescTxt
        {
            get { return s_descTxt; }
        }

        public string DispName
        {
            get { return s_dispName; }
        }

        public IEnumerable<TopicExample> ExampleInputs
        {
            get { return _exampleInputs; }
        }

        public string Info
        {
            get { return _info; }
        }

        public string Path
        {
            get { return _path; }
        }

        public TopicData(string dispName, string descTxt, string info, IEnumerable<TopicExample> examples, string path)
        {
            s_dispName = dispName;
            s_descTxt = descTxt;
            _exampleInputs = examples;
            _path = path;
            _info = info;
        }

        public string GetHintStr()
        {
            string explainStr = GetTrunExplainStr();
            explainStr = Regex.Replace(explainStr, "<a(.*)?>", "<p>");
            explainStr = explainStr.Replace("</a>", "</p>");
            return "<p>" + GetTrunExplainStr() + "</p>";
        }

        public string GetTrunExplainStr()
        {
            if (s_descTxt.Length < MAX_DESC_PREV_LENGTH)
                return s_descTxt;
            string trunDescTxt = s_descTxt.Substring(0, MAX_DESC_PREV_LENGTH);
            return trunDescTxt + "...";
        }

        public TopicData RemoveBasePath()
        {
            string[] split = _path.Split('/');
            string removedStr = "";
            for (int i = 1; i < split.Length; ++i)
            {
                removedStr += split[i] + "/";
            }

            return new TopicData(s_dispName, s_descTxt, _info, _exampleInputs, removedStr);
        }

        public string ToHtml(HttpServerUtility server, bool useNameLink = true)
        {
            string firstNameTag, secondNameTag;
            if (useNameLink)
            {
                firstNameTag = "<a href='/prac/topic?Name=" + server.UrlEncode(s_dispName) + "'>";
                secondNameTag = "</a>";
            }
            else
            {
                firstNameTag = "<p class='sectionHeading'>";
                secondNameTag = "</p>";
            }

            string final = firstNameTag + s_dispName + secondNameTag;
            final += "<p class='sample-desc-txt'>" + (useNameLink ? GetTrunExplainStr() : s_descTxt) + "</p>";
            if (!useNameLink)
                final += "<p>" + _info + "</p>";
            if (!useNameLink)
                final += "<h3>Examples</h3>";

            int i;
            for (i = 0; i < _exampleInputs.Count(); ++i)
            {
                TopicExample exampleInput = _exampleInputs.ElementAt(i);
                final += "<a class='mathEquationLink' href='/Default?Index=" +
                    server.UrlEncode(exampleInput.CommandIndex.ToString()) + "&InputDisp=" + server.UrlEncode(exampleInput.Input);

                if (exampleInput.UseRad.HasValue)
                    final += "&UseRad=" + exampleInput.UseRad.Value.ToString();

                final += "'>" + MathSolverLibrary.WorkMgr.STM + (exampleInput.InputDisp == null ? exampleInput.Input : exampleInput.InputDisp) + MathSolverLibrary.WorkMgr.EDM + "</a>";

                if (useNameLink && i == EXAMPLE_LIMIT - 1)
                    break;

                if (i != _exampleInputs.Count() - 1)
                    final += "<br /><br />";
            }

            // Its possible there were too many examples and we had to stop.
            if (useNameLink && EXAMPLE_LIMIT < _exampleInputs.Count())
                final += "<br /><br />" + firstNameTag + "More..." + secondNameTag;

            return final;
        }
    }
}