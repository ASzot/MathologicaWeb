using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Protocols;

namespace MathSolverWebsite.MathSolverLibrary.LangCompat
{
    public static class TypeHelper
    {
        public static object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        public static MatchCollection Matches(string str, string patternStr)
        {
            return Regex.Matches(str, patternStr);
        }

        public static Match Match(string str, string patternStr)
        {
            return Regex.Match(str, patternStr);
        }
    }
}