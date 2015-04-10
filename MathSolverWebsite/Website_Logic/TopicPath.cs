using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathSolverWebsite.Website_Logic
{
    public interface TopicPath
    {
        string Path { get; }
        string DispName { get; }
        string GetHintStr();
    }
}