using MathSolverWebsite.MathSolverLibrary;
using System.Collections.Generic;

namespace MathSolverWebsite.Website_Logic
{
    public class ExampleMgr
    {
        private List<TopicExample> _examples = new List<TopicExample>();
        private int _iterIndex = 0;

        public bool IsEmpty
        {
            get { return _examples.Count == 0; }
        }

        public void AddExample(TopicExample example)
        {
            _examples.Add(example);
        }

        public TopicExample GetNext()
        {
            if (_iterIndex == _examples.Count)
            {
                _iterIndex = 0;
            }

            return _examples[_iterIndex++];
        }

        public void ShuffleContents()
        {
            _iterIndex = 0;

            ListExtensions.Shuffle(_examples);
        }
    }
}