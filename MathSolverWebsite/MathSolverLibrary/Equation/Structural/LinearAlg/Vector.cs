using MathSolverWebsite.MathSolverLibrary.Equation.Operators;

namespace MathSolverWebsite.MathSolverLibrary.Equation.Structural.LinearAlg
{
    internal class ExVector : ExMatrix
    {
        public const string I = "\\vec{i}";
        public const string J = "\\vec{j}";
        public const string K = "\\vec{k}";

        public virtual int Length
        {
            get { return base.Cols; }
        }

        public ExComp X
        {
            get { return 0 < Length ? Get(0) : Number.Zero; }
        }

        public ExComp Y
        {
            get { return 1 < Length ? Get(1) : Number.Zero; }
        }

        public ExComp Z
        {
            get
            {
                // Not all vectors will have Z component whereas
                // they are garunteed to have a least an x and y component.
                return 2 < Length ? Get(2) : Number.Zero;
            }
        }

        public ExComp[] Components
        {
            get
            {
                return base._exData[0];
            }
        }

        public ExVector(int length)
            : base(1, length)
        {
        }

        public ExVector(params ExComp[] exs)
            : base(exs)
        {
        }

        public virtual ExComp Get(int index)
        {
            return Get(0, index);
        }

        public virtual void Set(int index, ExComp val)
        {
            Set(0, index, val);
        }

        public static ExComp Dot(ExVector vec0, ExVector vec1)
        {
            if (vec0.Length != vec1.Length)
                return Number.Undefined;

            ExComp totalSum = Number.Zero;
            for (int i = 0; i < vec0.Length; ++i)
            {
                ExComp prod = MulOp.StaticCombine(vec0.Get(i), vec1.Get(i));
                totalSum = AddOp.StaticCombine(prod, totalSum);
            }
            return totalSum;
        }

        public virtual ExVector CreateEmptyBody()
        {
            return new ExVector(Length);
        }

        public virtual ExVector CreateVec(params ExComp[] exs)
        {
            return new ExVector(exs);
        }

        public ExComp GetVecLength()
        {
            ExComp sum = Number.Zero;
            for (int i = 0; i < this.Length; ++i)
            {
                sum = AddOp.StaticCombine(PowOp.StaticCombine(Get(i), new Number(2.0)), sum);
            }

            return PowOp.StaticCombine(sum, AlgebraTerm.FromFraction(Number.One, new Number(2.0)));
        }

        public override ExComp Clone()
        {
            ExVector vec = new ExVector(this.Length);
            for (int i = 0; i < this.Length; ++i)
            {
                vec.Set(i, this.Get(i).Clone());
            }

            return vec;
        }

        public ExVector Normalize()
        {
            ExVector vec = this.CreateEmptyBody();
            ExComp vecLength = GetVecLength();

            for (int i = 0; i < this.Length; ++i)
            {
                ExComp setVal = DivOp.StaticCombine(this.Get(i), vecLength.Clone());
                vec.Set(i, setVal);
            }

            return vec;
        }
    }
}