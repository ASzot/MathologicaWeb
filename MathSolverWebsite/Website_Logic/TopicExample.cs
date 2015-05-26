using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                input = input.Replace("(", "\\left(");
                input = input.Replace(")", "\\right)");
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
}