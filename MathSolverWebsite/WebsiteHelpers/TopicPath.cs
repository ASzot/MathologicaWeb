using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathSolverWebsite.WebsiteHelpers
{
    public interface TopicPath
    {
        public string Path { get; }
        public string DispName { get; }
        public string GetHintStr();
    }
}