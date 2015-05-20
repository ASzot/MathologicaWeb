﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg
{
    class ExColVec : ExVector
    {
        public override int Length
        {
            get
            {
                return base.Rows;
            }
        }

        public ExColVec(int length)
        {
            _exData = new ExComp[length][];
            for (int i = 0; i < length; ++i)
            {
                _exData[i] = new ExComp[1];
            }

            _subComps = new List<ExComp>();
            _subComps.Add(new ExTrash());
        }

        public ExColVec(params ExComp[] exs)
        {
            // The elements have to be flipped.
            ExComp[][] tranposed = new ExComp[exs.Length][];

            for (int i = 0; i < tranposed.Length; ++i)
            {
                tranposed[i] = new ExComp[] { exs[i] };
            }

            _exData = tranposed;
            _subComps = new List<ExComp>();
            _subComps.Add(new ExTrash());
        }

        public override ExVector CreateEmptyBody()
        {
            return new ExColVec(Length);
        }

        public override ExVector CreateVec(params ExComp[] exs)
        {
            return new ExColVec(exs);
        }

        public override ExComp Get(int index)
        {
            return Get(index, 0);
        }

        public override void Set(int index, ExComp val)
        {
            Set(index, 0, val);
        }
    }
}