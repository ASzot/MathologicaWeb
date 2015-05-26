using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace MathSolverWebsite.Website_Logic
{
    public class TopicDatas
    {
        private static ExampleMgr _exampleMgr = new ExampleMgr();
        private List<TopicData> _topicDatas = new List<TopicData>();

        public static ExampleMgr ExampleMgr
        {
            get { return _exampleMgr; }
        }

        public List<TopicData> TopicDataInfo
        {
            get { return _topicDatas; }
        }

        public TopicDatas()
        {
        }

        public TopicDatas(List<TopicData> topicDatas)
        {
            _topicDatas = topicDatas;
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

        public TopicDatas GetTopicDataMatches(string searchStr)
        {
            var matches = from topicData in _topicDatas
                          where Regex.IsMatch(topicData.DispName, searchStr, RegexOptions.IgnoreCase)
                          select topicData;
            return new TopicDatas(matches.ToList());
        }

        public List<TopicData> GetTopics(string basePath)
        {
            var matches = from td in _topicDatas
                          where td.BasePath == basePath
                          select td;

            return matches.ToList();
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

            //_topicDatas = (from td in _topicDatas
            //               orderby td.DispName
            //               select td).ToList();

            return true;
        }
    }
}