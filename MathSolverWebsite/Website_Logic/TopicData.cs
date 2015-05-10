using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MathSolverWebsite.MathSolverLibrary;

namespace MathSolverWebsite.Website_Logic
{
    public struct TopicExample
    {
        public int CommandIndex;
        public string Input;
        public string InputDisp;
        public bool? UseRad;

        public TopicExample(int commandIndex, string input, string inputDisp, bool? useRad)
        {
            if (input != null)
            {
                input = input.Replace("&gt;", "\\gt");
                input = input.Replace("&lt;", "\\lt");
            }
            if (inputDisp != null)
            {
                inputDisp = inputDisp.Replace("&gt;", "\\gt");
                inputDisp = inputDisp.Replace("&lt;", "\\lt");
            }

            CommandIndex = commandIndex;
            Input = input;
			InputDisp = inputDisp;
            UseRad = useRad;
        }

        public string ToHtml(HttpServerUtility server)
        {
            return "<a class='mathEquationLink' href='Default?Index=" +
                    server.UrlEncode(CommandIndex.ToString()) + "&InputDisp=" + server.UrlEncode(Input) + "'>" + MathSolverLibrary.WorkMgr.STM +
                    (InputDisp == null ? Input : InputDisp) + MathSolverLibrary.WorkMgr.EDM + "</a>";
        }
    }

    public struct TopicData : TopicPath
    {
        private string s_dispName;
        private string _path;
        private string _info;
        private string s_descTxt;
        private const int EXAMPLE_LIMIT = 3;
        private IEnumerable<TopicExample> _exampleInputs;

        private const int MAX_DESC_PREV_LENGTH = 400;


        public string Path
        {
            get { return _path; }
        }

        public string BasePath
        {
            get { return _path.Split('/')[0].Replace("_", " "); }
        }

        public string DispName
        {
            get { return s_dispName; }
        }

        public string Info
        {
            get { return _info; }
        }

        public string DescTxt
        {
            get { return s_descTxt; }
        }

        public IEnumerable<TopicExample> ExampleInputs
        {
            get { return _exampleInputs; }
        }

        public TopicData(string dispName, string descTxt, string info, IEnumerable<TopicExample> examples, string path)
        {
            s_dispName = dispName;
            s_descTxt = descTxt;
            _exampleInputs = examples;
            _path = path;
            _info = info;
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

        public string GetTrunExplainStr()
        {
            if (s_descTxt.Length < MAX_DESC_PREV_LENGTH)
                return s_descTxt;
            string trunDescTxt = s_descTxt.Substring(0, MAX_DESC_PREV_LENGTH);
            return trunDescTxt + "...";
        }

        public string GetHintStr()
        {
            string explainStr = GetTrunExplainStr();
            explainStr = Regex.Replace(explainStr, "<a(.*)?>", "<p>");
            explainStr = explainStr.Replace("</a>", "</p>");
            return "<p>" + GetTrunExplainStr() + "</p>";
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

    public class ExampleMgr
    {
        private List<TopicExample> _examples = new List<TopicExample>();
        private int _iterIndex = 0;

        public void AddExample(TopicExample example)
        {
            _examples.Add(example);
        }

        public bool IsEmpty
        {
            get { return _examples.Count == 0; }
        }

        public void ShuffleContents()
        {
            _iterIndex = 0;

            _examples.Shuffle();
        }

        public TopicExample GetNext()
        {
            if (_iterIndex == _examples.Count)
            {
                _iterIndex = 0;
            }

            return _examples[_iterIndex++];
        }
    }

    public class TopicDatas
    {
        private List<TopicData> _topicDatas = new List<TopicData>();

        public List<TopicData> TopicDataInfo
        {
            get { return _topicDatas; }
        }

        private static ExampleMgr _exampleMgr = new ExampleMgr();

        public static ExampleMgr ExampleMgr
        {
            get { return _exampleMgr; }
        }

        public TopicDatas()
        {

        }

        public TopicDatas(List<TopicData> topicDatas)
        {
            _topicDatas = topicDatas;
        }

        public List<TopicData> GetTopics(string basePath)
        {
            var matches = from td in _topicDatas
                          where td.BasePath == basePath
                          select td;

            return matches.ToList();
        }

        public TopicData? GetTopic(string name)
        {
            foreach (var topicData in _topicDatas)
            {
                if (topicData.DispName == name)
                    return topicData;
            }

            return null;
        }

        public bool LoadTopics(string xmlFilePath)
        {
            if (_topicDatas != null && _exampleMgr != null && _topicDatas.Count != 0 && !_exampleMgr.IsEmpty)
                return true;

            _topicDatas = new List<TopicData>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            XmlNodeList topics = xmlDoc.GetElementsByTagName("topic");

            foreach (XmlNode topic in topics)
            {
                string name = null;
                string path = null;
                string desc = null;
                string info = "";
                bool? useRad = null;
                List<TopicExample> exampleInputs = new List<TopicExample>();

                foreach (XmlNode node in topic.ChildNodes)
                {
                    if (node.Name == "topicName")
                        name = node.InnerText;
                    else if (node.Name == "topicDesc")
                        desc = node.InnerText;
                    else if (node.Name == "topicPath")
                        path = node.InnerText;
                    else if (node.Name == "info")
                        info = node.InnerText;
                    else if (node.Name == "useRad")
                    {
                        bool tmpUseRad;
                        if (bool.TryParse(node.InnerText, out tmpUseRad))
                            useRad = tmpUseRad;
                    }
                    else if (node.Name == "exampleInput")
                    {
                        string input = null;
                        string inputDisp = null;
                        int inputIndex = 0;
                        foreach (XmlNode subNode in node.ChildNodes)
                        {
                            if (subNode.Name == "input")
                                input = subNode.InnerText;
                            else if (subNode.Name == "inputIndex")
                            {
                                int.TryParse(subNode.InnerText, out inputIndex);
                            }
                            else if (subNode.Name == "inputDisp")
                                inputDisp = subNode.InnerText;
                        }

                        if (input == null || inputIndex == -1)
                            return false;

                        TopicExample example = new TopicExample(inputIndex, input, inputDisp == null ? input : inputDisp, useRad);

                        //_exampleMgr.AddExample(example);
                        exampleInputs.Add(example);
                    }
                    else
                        return false;
                }

                if (name == null || desc == null || path == null || exampleInputs.Count == 0)
                    return false;

                _topicDatas.Add(new TopicData(name, desc, info, exampleInputs, path));
            }

            _topicDatas = (from td in _topicDatas
                           orderby td.DispName
                           select td).ToList();

            return true;
        }

        public TopicDatas GetTopicDataMatches(string searchStr)
        {
            var matches = from topicData in _topicDatas
                          where Regex.IsMatch(topicData.DispName, searchStr, RegexOptions.IgnoreCase)
                          select topicData;
            return new TopicDatas(matches.ToList());
        }
    }
}