using MathSolverWebsite.MathSolverLibrary.Equation.Functions.Calculus.Vector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Functions
{
    internal abstract class AppliedFunction : AlgebraFunction
    {
        protected FunctionType _functionType;
        protected Type _type;

        public FunctionType GetFunctionType()
        {
            return _functionType;
        }

        public ExComp GetInnerEx()
        {
            return GetInnerTerm().RemoveRedundancies();
        }

        public AlgebraTerm GetInnerTerm()
        {
            return new AlgebraTerm(_subComps.ToArray());
        }

        public AppliedFunction(ExComp ex, FunctionType functionType, Type type)
        {
            if (ex is AlgebraFunction)
                _subComps.Add(ex);
            else
                base.AssignTo(ex.ToAlgTerm());

            _functionType = functionType;
            _type = type;
        }

        public override AlgebraTerm ApplyOrderOfOperations()
        {
            AlgebraTerm innerTerm = GetInnerTerm();
            innerTerm = innerTerm.ApplyOrderOfOperations();

            _subComps = new List<ExComp>();
            _subComps = innerTerm.GetSubComps();

            return this;
        }

        public override void AssignTo(AlgebraTerm algebraTerm)
        {
            if (algebraTerm.GetType() == this.GetType())
            {
                base.AssignTo(algebraTerm);
            }
            else
                throw new ArgumentException();
        }

        public override ExComp MakeWorkable()
        {
            return base.MakeWorkable();
        }

        public override ExComp CloneEx()
        {
            return CreateInstance(GetInnerTerm().CloneEx());
        }

        protected void CallChildren(bool harshEval, ref TermType.EvalData pEvalData)
        {
            for (int i = 0; i < _subComps.Count; ++i)
            {
                if (_subComps[i] is AlgebraFunction)
                    _subComps[i] = (_subComps[i] as AlgebraFunction).Evaluate(harshEval, ref pEvalData);
            }
        }

        public override AlgebraTerm CompoundFractions()
        {
            AlgebraTerm compoundedFracs = GetInnerTerm().CompoundFractions();

            return CreateInstance(compoundedFracs);
        }

        public override AlgebraTerm CompoundFractions(out bool valid)
        {
            AlgebraTerm compoundedFractions = GetInnerTerm().CompoundFractions(out valid);

            return CreateInstance(compoundedFractions);
        }

        public override AlgebraTerm ConvertImaginaryToVar()
        {
            AlgebraTerm converted = GetInnerTerm().ConvertImaginaryToVar();
            base.AssignTo(converted);
            return this;
        }

        public override List<FunctionType> GetAppliedFunctionsNoPow(AlgebraComp varFor)
        {
            List<FunctionType> appliedFuncs = new List<FunctionType>();
            if (this.Contains(varFor))
                appliedFuncs.Add(_functionType);
            return appliedFuncs;
        }

        public override double GetCompareVal()
        {
            return 0.5;
        }

        public override List<Restriction> GetDomain(AlgebraVar varFor, AlgebraSolver agSolver, ref TermType.EvalData pEvalData)
        {
            // The domain is all real numbers.
            return new List<Restriction>();
        }

        public override List<ExComp[]> GetGroups()
        {
            List<ExComp[]> groups = new List<ExComp[]>();
            ExComp[] onlyGroup = { this.CloneEx() };
            groups.Add(onlyGroup);
            return groups;
        }

        public override AlgebraTerm HarshEvaluation()
        {
            AlgebraTerm harshEval = GetInnerTerm().HarshEvaluation();

            AlgebraTerm created = CreateInstance(harshEval);
            return created;
        }

        public override bool IsEqualTo(ExComp ex)
        {
            if (ex.GetType() == this.GetType())
            {
                return this.GetInnerTerm().IsEqualTo((ex as AppliedFunction).GetInnerTerm());
            }
            else
                return false;
        }

        public override AlgebraTerm Order()
        {
            return CreateInstance(GetInnerTerm().Order());
        }

        public override AlgebraTerm RemoveOneCoeffs()
        {
            ExComp innerEx = GetInnerEx();
            if (innerEx is AlgebraTerm)
                innerEx = (innerEx as AlgebraTerm).RemoveOneCoeffs();
            return CreateInstance(innerEx);
        }

        public override ExComp RemoveRedundancies(bool postWorkable = false)
        {
            ExComp nonRedundantInner = GetInnerTerm().RemoveRedundancies(postWorkable);
            return CreateInstance(nonRedundantInner);
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn)
        {
            AlgebraTerm term = GetInnerTerm().Substitute(subOut, subIn);
            return CreateInstance(term);
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn, ref bool success)
        {
            AlgebraTerm term = GetInnerTerm().Substitute(subOut, subIn, ref success);
            return CreateInstance(term);
        }

        public override bool TermsRelatable(ExComp comp)
        {
            return this.IsEqualTo(comp);
        }

        public override AlgebraTerm ToAlgTerm()
        {
            return this;
        }

        protected virtual AlgebraTerm CreateInstance(params ExComp[] args)
        {
            return (AlgebraTerm)Activator.CreateInstance(_type, args[0]);
        }
    }

    internal abstract class AppliedFunction_NArgs : AppliedFunction
    {
        protected ExComp[] _args;

        public AppliedFunction_NArgs(FunctionType functionType, Type type, params ExComp[] args)
            : base(args[0], functionType, type)
        {
            _args = args;
        }

        public override ExComp CloneEx()
        {
            IEnumerable<ExComp> cloned = from arg in _args
                                         select arg.CloneEx();
            return CreateInstance(cloned.ToArray());
        }

        public override AlgebraTerm CompoundFractions()
        {
            return this;
        }

        public override AlgebraTerm CompoundFractions(out bool valid)
        {
            valid = false;

            return this;
        }

        public override AlgebraTerm HarshEvaluation()
        {
            IEnumerable<ExComp> harshEval = from arg in _args
                                            select arg.ToAlgTerm().HarshEvaluation();
            AlgebraTerm created = CreateInstance(harshEval.ToArray());
            return created;
        }

        public override AlgebraTerm Order()
        {
            IEnumerable<ExComp> ordered = from arg in _args
                                          select arg.ToAlgTerm().Order();
            return CreateInstance(ordered.ToArray());
        }

        public override AlgebraTerm RemoveOneCoeffs()
        {
            IEnumerable<ExComp> noOneCoeffs = from arg in _args
                                              select (arg is AlgebraTerm ? (arg as AlgebraTerm).RemoveOneCoeffs() : arg);
            return CreateInstance(noOneCoeffs.ToArray());
        }

        public override ExComp RemoveRedundancies(bool postWorkable = false)
        {
            IEnumerable<ExComp> noRedun = from arg in _args
                                          select arg.ToAlgTerm().RemoveRedundancies(postWorkable);
            return CreateInstance(noRedun.ToArray());
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn)
        {
            IEnumerable<ExComp> substituted = from arg in _args
                                              select arg.ToAlgTerm().Substitute(subOut, subIn);
            return CreateInstance(substituted.ToArray());
        }

        public override AlgebraTerm Substitute(ExComp subOut, ExComp subIn, ref bool success)
        {
            ExComp[] substituted = new ExComp[_args.Length];
            for (int i = 0; i < _args.Length; ++i)
            {
                substituted[i] = _args[i].ToAlgTerm().Substitute(subOut, subIn, ref success);
            }

            return CreateInstance(substituted);
        }

        protected override AlgebraTerm CreateInstance(params ExComp[] args)
        {
            return (AlgebraTerm)Activator.CreateInstance(_type, args);
        }
    }

    internal abstract class BasicAppliedFunc : AppliedFunction
    {
        protected string _useEnd = ")";
        protected string _useStart = "(";
        protected string s_name;

        public virtual string GetFuncName()
        {
            return s_name;
        }

        public BasicAppliedFunc(ExComp innerEx, string name, FunctionType ft, Type type)
            : base(innerEx, ft, type)
        {
            s_name = name;
        }

        public static ExComp Parse(string parseStr, ExComp innerEx, ref List<string> pParseErrors)
        {
            if (parseStr == "sin")
                return new SinFunction(innerEx);
            else if (parseStr == "cos")
                return new CosFunction(innerEx);
            else if (parseStr == "tan")
                return new TanFunction(innerEx);
            else if (parseStr == "log")
                return new LogFunction(innerEx);   // By default we are log base 10.
            else if (parseStr == "ln")
            {
                LogFunction log = new LogFunction(innerEx);
                log.SetBase(Constant.ParseConstant("e"));
                return log;
            }
            else if (parseStr == "sec")
                return new SecFunction(innerEx);
            else if (parseStr == "csc")
                return new CscFunction(innerEx);
            else if (parseStr == "cot")
                return new CotFunction(innerEx);
            else if (parseStr == "asin" || parseStr == "arcsin")
                return new ASinFunction(innerEx);
            else if (parseStr == "acos" || parseStr == "arccos")
                return new ACosFunction(innerEx);
            else if (parseStr == "atan" || parseStr == "arctan")
                return new ATanFunction(innerEx);
            else if (parseStr == "acsc" || parseStr == "arccsc")
                return new ACscFunction(innerEx);
            else if (parseStr == "asec" || parseStr == "arcsec")
                return new ASecFunction(innerEx);
            else if (parseStr == "acot" || parseStr == "arccot")
                return new ACotFunction(innerEx);
            else if (parseStr == "sqrt")
                return new AlgebraTerm(innerEx, new Operators.PowOp(), new AlgebraTerm(Number.GetOne(), new Operators.DivOp(), new Number(2.0)));
            else if (parseStr == "det")
                return new Structural.LinearAlg.Determinant(innerEx);
            else if (parseStr == "curl")
                return new CurlFunc(innerEx);
            else if (parseStr == "div")
                return new DivergenceFunc(innerEx);
            else if (parseStr == "!")
                return new FactorialFunction(innerEx);

            return null;
        }

        public override string FinalToDispStr()
        {
            return s_name + _useStart + GetInnerTerm().FinalToDispStr() + _useEnd;
        }

        public override string ToAsciiString()
        {
            return s_name + _useStart + GetInnerTerm().ToAsciiString() + _useEnd;
        }

        public override string ToJavaScriptString(bool useRad)
        {
            string innerStr = GetInnerTerm().ToJavaScriptString(useRad);
            if (GetInnerTerm() == null)
                return null;
            return "Math." + s_name + "(" + innerStr + ")";
        }

        public override string ToString()
        {
            if (MathSolver.USE_TEX_DEBUG)
                return ToTexString();
            return s_name + _useStart + GetInnerTerm().ToString() + _useEnd;
        }

        public override string ToTexString()
        {
            return s_name + _useStart + GetInnerTerm().ToTexString() + _useEnd;
        }
    }
}