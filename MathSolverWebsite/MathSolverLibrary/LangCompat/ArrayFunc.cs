using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using MathSolverWebsite.MathSolverLibrary.Parsing;

namespace MathSolverWebsite.MathSolverLibrary.LangCompat
{
    static class ArrayFunc
    {
        public static List<T> ToList<T>(T[] arr)
        {
            return arr.ToList();
        }

        public static T[] ToArray<T>(List<T> list)
        {
            return list.ToArray();
        }

        public static List<TypePair<LexemeType, LexicalParser.MatchTolken>> OrderList(List<TypePair<LexemeType, LexicalParser.MatchTolken>> list)
        {
            list = (from ele in list
                orderby ele.GetData2().Index
                select ele).ToList();
            return list;
        }

        public static List<TypePair<int, TrigFunction>> OrderList(List<TypePair<int, TrigFunction>> list)
        {
            list.OrderBy(trigFuncIntPow => trigFuncIntPow.GetData1());

            return list;
        }

        public static List<T> RemoveIndex<T>(List<T> list, int index)
        {
            list.RemoveAt(index);
            return list;
        }
    }
}