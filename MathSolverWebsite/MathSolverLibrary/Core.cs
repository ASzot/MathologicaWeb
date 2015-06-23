﻿using MathSolverWebsite.MathSolverLibrary.Equation;
using System;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary
{
    public static class DoubleHelper
    {
        public static bool IsInteger(this double d)
        {
            if (double.IsInfinity(d) || double.IsNaN(d))
                return false;
            string str = d.ToString();
            return !str.Contains(".");
        }
    }

    public static class StringHelper
    {
        public static string RemoveSurroundingParas(this string str)
        {
            if (str.StartsWith("(") && str.EndsWith(")"))
            {
                str = str.Remove(0, 1);
                str = str.Remove(str.Length - 1, 1);
            }
            return str;
        }

        public static string SurroundWithParas(this string str)
        {
            return str.Insert(str.Length, ")").Insert(0, "(");
        }
    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random Local;

        public static Random getThisThreadsRandom()
        {
            return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Environment.CurrentManagedThreadId)));
        }
    }

    public class TypePair<T, K>
    {
        private T _data1;
        private K _data2;

        public void SetData1(T value)
        {
            _data1 = value;
        }

        public T GetData1()
        {
            return _data1;
        }

        public void SetData2(K value)
        {
            _data2 = value;
        }

        public K GetData2()
        {
            return _data2;
        }

        public TypePair(T data1, K data2)
        {
            _data1 = data1;
            _data2 = data2;
        }

        public TypePair()
        {
        }

        public override string ToString()
        {
            return GetData1().ToString() + ":" + GetData2().ToString();
        }
    }

    internal static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.getThisThreadsRandom().Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    internal static class ObjectHelper
    {
        public static bool ContainsEx(this List<Equation.ExComp> exs, Equation.ExComp ex)
        {
            foreach (ExComp compareEx in exs)
            {
                if (compareEx.IsEqualTo(ex))
                    return true;
            }

            return false;
        }
    }
}