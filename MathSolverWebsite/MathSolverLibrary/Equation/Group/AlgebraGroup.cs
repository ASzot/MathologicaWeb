using System.Collections.Generic;
using System.Linq;

namespace MathSolverWebsite.MathSolverLibrary.Equation
{
    internal class AlgebraGroup
    {
        private ExComp[] _group;

        public ExComp[] Group
        {
            get { return _group; }
        }

        public int GroupCount
        {
            get { return _group.Count(); }
        }

        public ExComp this[int i]
        {
            get { return _group[i]; }
            set { _group[i] = value; }
        }

        public AlgebraGroup(ExComp[] group)
        {
            _group = group;
        }

        public AlgebraGroup GetVariableGroupComps(AlgebraComp varFor)
        {
            List<ExComp> addExComps = new List<ExComp>();

            foreach (ExComp groupComp in _group)
            {
                if (groupComp.IsEqualTo(varFor) || (groupComp is AlgebraTerm && (groupComp as AlgebraTerm).Contains(varFor)))
                    addExComps.Add(groupComp);
            }

            return new AlgebraGroup(addExComps.ToArray());
        }

        public bool IsZero()
        {
            return _group.Length == 1 && Number.Zero.IsEqualTo(_group[0]);
        }

        public override string ToString()
        {
            string finalStr = "";
            for (int i = 0; i < _group.Count(); ++i)
                finalStr += _group[i].ToString();
            return finalStr;
        }

        public static AlgebraTerm ToTerm(List<AlgebraGroup> gps)
        {
            AlgebraTerm term = new AlgebraTerm();
            foreach (AlgebraGroup gp in gps)
                term = AlgebraTerm.OpAdd(term, gp);

            return term;
        }

        public static AlgebraTerm GetConstantTo(List<AlgebraGroup> gps, AlgebraComp cmp)
        {
            IEnumerable<AlgebraTerm> terms = from squaredGroup in gps
                                             select squaredGroup.Group.GetUnrelatableTermsOfGroup(cmp).ToAlgTerm();

            AlgebraTerm totalTerm = new AlgebraTerm();
            foreach (AlgebraTerm term in terms)
            {
                totalTerm = AlgebraTerm.OpAdd(totalTerm, term);
            }

            if (totalTerm.SubComps.Count == 0)
                return Number.One.ToAlgTerm();

            return totalTerm;
        }

        public bool IsEqualTo(AlgebraGroup ag)
        {
            if (this.GroupCount != ag.GroupCount)
                return false;
            for (int i = 0; i < ag.GroupCount; ++i)
            {
                if (!this[i].IsEqualTo(ag[i]))
                    return false;
            }

            return true;
        }

        public AlgebraTerm ToTerm()
        {
            AlgebraTerm term = new AlgebraTerm();
            term.AddGroup(_group);
            return term;
        }
    }
}