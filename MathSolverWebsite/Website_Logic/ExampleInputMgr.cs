using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MathSolverWebsite.MathSolverLibrary;

namespace MathSolverWebsite.Website_Logic
{
    public struct ExampleInputData
    {
        public ExampleInputData(string title, TopicExample example)
        {
            Title = title;
            Example = example;
        }

        public string ToHtml()
        {
            return
                "<p class='pob-sub-title'>" + Title + "</p>" +
                "<div style='text-align: center;' class='pob-problem'>" +
                    "<span class='hidden'>" + Example.Input + "|" + Example.CommandIndex.ToString() + "|" + Example.UseRad.ToString() + "</span>" +
                    "<div>" +
                        "<span class='noselect pointable'>`" + Example.InputDisp + "`</span>" +
                    "</div>" +
                "</div>";
        }

        public TopicExample Example;
        public string Title;
    }

    public class ExampleInputMgr
    {
        private List<ExampleInputData> _exampleTxts;

        public ExampleInputMgr(TopicDatas topicDatas)
        {
            _exampleTxts = new List<ExampleInputData>();
            foreach (TopicData topicData in topicDatas.TopicDataInfo)
            {
                foreach (TopicExample te in topicData.ExampleInputs)
                {
                    _exampleTxts.Add(new ExampleInputData(topicData.DispName, te));
                }
            }

            ListExtensions.Shuffle(_exampleTxts);
        }

        public ExampleInputData GetExample(int index)
        {
            if (_exampleTxts == null || _exampleTxts.Count == 0)
                return new ExampleInputData("Solving Power Equations", new TopicExample(0, "x^{2}=4", "x^{2}=4", null));
            return _exampleTxts[index];
        }

        public int IncrementIndex(int index)
        {
            if (index + 1 >= _exampleTxts.Count)
                return 0;
            return index + 1;
        }

        public int DecrementIndex(int index)
        {
            if (index - 1 < 0)
                return _exampleTxts.Count - 1;
            return index - 1;
        }
    }
}