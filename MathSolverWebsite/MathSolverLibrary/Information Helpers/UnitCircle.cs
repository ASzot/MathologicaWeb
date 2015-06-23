﻿using MathSolverWebsite.MathSolverLibrary.Equation;
using MathSolverWebsite.MathSolverLibrary.Equation.Functions;
using MathSolverWebsite.MathSolverLibrary.Equation.Operators;
using System.Collections.Generic;

namespace MathSolverWebsite.MathSolverLibrary.Information_Helpers
{
    internal class UnitCircle
    {
        public static List<UnitCirclePoint> UnitCirclePoints = new List<UnitCirclePoint>(16);

        public static UnitCirclePoint GetAngleForPoint_X(ExComp x)
        {
            // Range of acos is [0, pi].

            if (x is AlgebraTerm)
                x = (x as AlgebraTerm).RemoveRedundancies();

            // This will work because we are starting at angle zero and will reach
            foreach (UnitCirclePoint unitCirclePoint in UnitCirclePoints)
            {
                ExComp ucX = unitCirclePoint.X.CloneEx();
                if (ucX is AlgebraTerm)
                    ucX = (ucX as AlgebraTerm).RemoveRedundancies();

                if (ucX.IsEqualTo(x))
                    return unitCirclePoint;

                AlgebraTerm secondCheck = ucX.ToAlgTerm();
                secondCheck.ConvertPowFracsToDecimal();
                if (secondCheck.RemoveRedundancies().IsEqualTo(x))
                    return unitCirclePoint;
            }

            return null;
        }

        public static UnitCirclePoint GetAngleForPoint_Y(ExComp y)
        {
            // Range of asin is [-pi/2, pi/2].

            if (y is AlgebraTerm)
                y = (y as AlgebraTerm).RemoveRedundancies();

            // Check fourth quadrant.
            for (int i = UnitCirclePoints.Count - 5; i < UnitCirclePoints.Count; ++i)
            {
                ExComp ucY = UnitCirclePoints[i].Y.CloneEx();
                if (ucY is AlgebraTerm)
                    ucY = (ucY as AlgebraTerm).RemoveRedundancies();

                if (ucY.IsEqualTo(y))
                {
                    return UnitCirclePoints[i];
                }

                AlgebraTerm secondCheck = ucY.ToAlgTerm();
                secondCheck.ConvertPowFracsToDecimal();
                if (secondCheck.RemoveRedundancies().IsEqualTo(y))
                    return UnitCirclePoints[i];
            }

            // Check the first quadrant.
            for (int i = 0; i < 5; ++i)
            {
                ExComp ucY = UnitCirclePoints[i].Y.CloneEx();
                if (ucY is AlgebraTerm)
                    ucY = (ucY as AlgebraTerm).RemoveRedundancies();

                if (ucY.IsEqualTo(y))
                {
                    return UnitCirclePoints[i];
                }

                AlgebraTerm secondCheck = ucY.ToAlgTerm();
                secondCheck.ConvertPowFracsToDecimal();
                if (secondCheck.RemoveRedundancies().IsEqualTo(y))
                    return UnitCirclePoints[i];
            }

            return null;
        }

        public static UnitCirclePoint GetAngleForPoint_Y_over_X(ExComp y_over_x)
        {
            // Range of atan is [-pi/2, pi/2].

            if (y_over_x is AlgebraTerm)
                y_over_x = (y_over_x as AlgebraTerm).RemoveRedundancies();

            for (int i = UnitCirclePoints.Count - 5; i < UnitCirclePoints.Count; ++i)
            {
                ExComp ucYoverX = UnitCirclePoints[i].Y_over_X.CloneEx();
                if (ucYoverX is AlgebraTerm)
                    ucYoverX = (ucYoverX as AlgebraTerm).RemoveRedundancies();

                if (ucYoverX.IsEqualTo(y_over_x))
                    return UnitCirclePoints[i];
                AlgebraTerm secondCheck = ucYoverX.ToAlgTerm();
                secondCheck.ConvertPowFracsToDecimal();
                if (secondCheck.RemoveRedundancies().IsEqualTo(y_over_x))
                    return UnitCirclePoints[i];
            }

            // Check the first quadrant.
            for (int i = 0; i < 5; ++i)
            {
                ExComp ucYoverX = UnitCirclePoints[i].Y_over_X.CloneEx();
                if (ucYoverX is AlgebraTerm)
                    ucYoverX = (ucYoverX as AlgebraTerm).RemoveRedundancies();

                if (ucYoverX.IsEqualTo(y_over_x))
                    return UnitCirclePoints[i];
            }

            return null;
        }

        public static UnitCirclePoint GetPointForAngle(Number angleNum, Number angleDen)
        {
            foreach (UnitCirclePoint unitCirclePoint in UnitCirclePoints)
            {
                if (unitCirclePoint != null && unitCirclePoint.AngleNum == angleNum && unitCirclePoint.AngleDen == angleDen)
                    return unitCirclePoint;
            }

            return null;
        }

        /// <summary>
        /// Adds all of the defined points on the unit circle.
        /// Checks whether the points have already been added.
        /// Must be called on initialization.
        /// </summary>
        public static void Init()
        {
            if (UnitCirclePoints.Count != 0)
                return;

            Number zero = Number.GetZero();
            Number one = Number.GetOne();
            Number two = new Number(2.0);
            AlgebraTerm half = AlgebraTerm.FromFraction(Number.GetOne(), new Number(2.0));
            PowerFunction sqrt3 = new PowerFunction(new Number(3.0), half);
            PowerFunction sqrt2 = new PowerFunction(new Number(2.0), half);
            AlgebraTerm sqrt3Over2 = AlgebraTerm.FromFraction(sqrt3, new Number(2.0));
            AlgebraTerm sqrt3Over3 = AlgebraTerm.FromFraction(sqrt3, new Number(3.0));
            AlgebraTerm sqrt2Over2 = AlgebraTerm.FromFraction(sqrt2, new Number(2.0));
            AlgebraTerm sqrt3_mul2_Over2 = AlgebraTerm.FromFraction(new AlgebraTerm(new Number(2.0), new MulOp(), sqrt3), new Number(3.0));

            UnitCirclePoints.Add(new UnitCirclePoint(0.0, 0.0, one, zero, zero, Number.GetUndefined(), one, Number.GetUndefined()));

            UnitCirclePoints.Add(new UnitCirclePoint(1.0, 6.0, sqrt3Over2, half, sqrt3Over3, sqrt3, sqrt3_mul2_Over2, two));
            UnitCirclePoints.Add(new UnitCirclePoint(1.0, 4.0, sqrt2Over2, sqrt2Over2, one, one, sqrt2, sqrt2));
            UnitCirclePoints.Add(new UnitCirclePoint(1.0, 3.0, half, sqrt3Over2, sqrt3, sqrt3Over3, two, sqrt3_mul2_Over2));

            UnitCirclePoints.Add(new UnitCirclePoint(1.0, 2.0, zero, one, Number.GetUndefined(), Number.GetZero(), Number.GetUndefined(), one));

            UnitCirclePoints.Add(new UnitCirclePoint(2.0, 3.0, MulOp.Negate(half), sqrt3Over2, MulOp.Negate(sqrt3), MulOp.Negate(sqrt3Over3), MulOp.Negate(two), sqrt3_mul2_Over2));
            UnitCirclePoints.Add(new UnitCirclePoint(3.0, 4.0, MulOp.Negate(sqrt2Over2), sqrt2Over2, MulOp.Negate(one), MulOp.Negate(one), MulOp.Negate(sqrt2), sqrt2));
            UnitCirclePoints.Add(new UnitCirclePoint(5.0, 6.0, MulOp.Negate(sqrt3Over2), half, MulOp.Negate(sqrt3Over3), MulOp.Negate(sqrt3), MulOp.Negate(sqrt3_mul2_Over2), two));

            UnitCirclePoints.Add(new UnitCirclePoint(1.0, 1.0, MulOp.Negate(one), zero, zero, Number.GetUndefined(), MulOp.Negate(one), Number.GetUndefined()));

            UnitCirclePoints.Add(new UnitCirclePoint(7.0, 6.0, MulOp.Negate(sqrt3Over2), MulOp.Negate(half), sqrt3Over3, sqrt3, MulOp.Negate(sqrt3_mul2_Over2), MulOp.Negate(two)));
            UnitCirclePoints.Add(new UnitCirclePoint(5.0, 4.0, MulOp.Negate(sqrt2Over2), MulOp.Negate(sqrt2Over2), one, one, MulOp.Negate(sqrt2), MulOp.Negate(sqrt2)));
            UnitCirclePoints.Add(new UnitCirclePoint(4.0, 3.0, MulOp.Negate(half), MulOp.Negate(sqrt3Over2), sqrt3, sqrt3Over3, MulOp.Negate(two), MulOp.Negate(sqrt3_mul2_Over2)));

            UnitCirclePoints.Add(new UnitCirclePoint(3.0, 2.0, zero, MulOp.Negate(one), Number.GetUndefined(), zero, Number.GetUndefined(), MulOp.Negate(one)));

            UnitCirclePoints.Add(new UnitCirclePoint(5.0, 3.0, half, MulOp.Negate(sqrt3Over2), MulOp.Negate(sqrt3), MulOp.Negate(sqrt3Over3), two, MulOp.Negate(sqrt3_mul2_Over2)));
            UnitCirclePoints.Add(new UnitCirclePoint(7.0, 4.0, sqrt2Over2, MulOp.Negate(sqrt2Over2), MulOp.Negate(one), MulOp.Negate(one), sqrt2, MulOp.Negate(sqrt2)));
            UnitCirclePoints.Add(new UnitCirclePoint(11.0, 6.0, sqrt3Over2, MulOp.Negate(half), MulOp.Negate(sqrt3Over3), MulOp.Negate(sqrt3), sqrt3_mul2_Over2, MulOp.Negate(two)));
        }
    }

    internal class UnitCirclePoint
    {
        public Number AngleDen;

        public Number AngleNum;

        public ExComp over_X;

        public ExComp over_Y;

        public ExComp X;

        public ExComp X_over_Y;

        public ExComp Y;

        public ExComp Y_over_X;

        /// <summary>
        /// Assign the values identifying this point on the unit circle. Compute the Y_over_X, X_over_Y, over_X, and the over_Y.
        /// All the formatting of the results is also fixed.
        /// </summary>
        /// <param name="angleNum"></param>
        /// <param name="angleDen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public UnitCirclePoint(Number angleNum, Number angleDen, ExComp x, ExComp y)
        {
            if (x is AlgebraTerm)
                x = (x as AlgebraTerm).RemoveRedundancies();
            if (y is AlgebraTerm)
                y = (y as AlgebraTerm).RemoveRedundancies();
            AngleNum = angleNum;
            AngleDen = angleDen;
            this.X = x;
            this.Y = y;

            Y_over_X = DivOp.StaticCombine(y.CloneEx(), x.CloneEx()).ToAlgTerm().MakeFormattingCorrect().RemoveRedundancies();
            X_over_Y = DivOp.StaticCombine(x.CloneEx(), y.CloneEx()).ToAlgTerm().MakeFormattingCorrect().RemoveRedundancies();

            AlgebraTerm overXDiv = DivOp.StaticCombine(Number.GetOne(), x.CloneEx()).ToAlgTerm();
            AlgebraTerm overYDiv = DivOp.StaticCombine(Number.GetOne(), y.CloneEx()).ToAlgTerm();

            over_X = overXDiv.MakeFormattingCorrect().RemoveRedundancies();
            over_Y = overYDiv.MakeFormattingCorrect().RemoveRedundancies();
        }

        /// <summary>
        /// Assign all the values to the data of the class.
        /// </summary>
        /// <param name="angleNum"></param>
        /// <param name="angleDen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="y_over_x"></param>
        /// <param name="x_over_y"></param>
        /// <param name="overX"></param>
        /// <param name="overY"></param>
        public UnitCirclePoint(double angleNum, double angleDen, ExComp x, ExComp y, ExComp y_over_x, ExComp x_over_y, ExComp overX, ExComp overY)
        {
            this.AngleNum = new Number(angleNum);
            this.AngleDen = new Number(angleDen);
            this.over_X = overX;
            this.over_Y = overY;
            this.X = x;
            this.Y = y;
            this.X_over_Y = x_over_y;
            this.Y_over_X = y_over_x;
        }

        public UnitCirclePoint(double angleNum, double angleDen, ExComp x, ExComp y)
            : this(new Number(angleNum), new Number(angleDen), x, y)
        {
        }

        public ExComp GetAngle()
        {
            //Number nAngNum = (Number)AngleNum.Clone();
            //Number nAngDen = (Number)AngleDen.Clone();
            Number nAngNum = AngleNum;
            Number nAngDen = AngleDen;

            // Do we have the zero angle.
            if (Number.OpEqual(nAngDen, 0.0))
                return AngleDen;

            ExComp num = MulOp.StaticCombine(nAngNum, Constant.ParseConstant("pi"));
            ExComp final = DivOp.StaticCombine(num, nAngDen);
            return final;
        }

        public override string ToString()
        {
            return AngleNum.ToString() + "& " + AngleDen.ToString() + "& " + X.ToString() + "& " + Y.ToString() +
                "& " + Y_over_X.ToString() + "& " + X_over_Y.ToString() + "& " + Y_over_X.ToString() + "& " +
                over_X.ToString() + "& " + over_Y.ToString();
        }
    }
}