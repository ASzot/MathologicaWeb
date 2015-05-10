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
                "<span class='hidden'>" + Example.Input + "|" + Example.UseRad.ToString() + "</span>" +
                "<div>" + 
                    "<span class='noselect pointable'>`" + Example.InputDisp + "`</span>" +  
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

            _exampleTxts.Shuffle();
        }

        public ExampleInputData GetExample(int index)
        {
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