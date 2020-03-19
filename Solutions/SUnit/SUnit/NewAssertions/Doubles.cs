using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using SUnit.Constraints;

namespace SUnit.NewAssertions
{
    public interface IDoubleExpression : IValueExpression<double?>
    {
        public new DoubleTest ApplyConstraint(IConstraint<double?> constraint);

        ValueTest<double?> IValueExpression<double?>.ApplyConstraint(IConstraint<double?> constraint)
        {
            return ApplyConstraint(constraint);
        }

        public new IDoubleExpression ApplyModifier(ConstraintModifier<double?> modifier);

        IValueExpression<double?> IValueExpression<double?>.ApplyModifier(ConstraintModifier<double?> modifier)
        {
            return ApplyModifier(modifier);
        }
    }

    public interface IDoubleIsExpression : IIsExpression<double?, IDoubleIsExpression, DoubleTest>
    {
        public new DoubleTest EqualTo(double? expected)
        {
            return ApplyConstraint(new FloatingPointEqualToConstraint(expected));
        }

        DoubleTest IIsExpression<double?, IDoubleIsExpression, DoubleTest>.EqualTo(double? expected) => EqualTo(expected);
        
        public DoubleTest Zero => EqualTo(0.0);
    }

    public class DoubleThat : That<double?>
    {
        internal DoubleThat(double? actual) : this(new DoubleExpression(actual, c => c)) { }
        internal DoubleThat(IDoubleExpression expression) : base(expression) { }

        public new IDoubleExpression Expression => (IDoubleExpression)base.Expression;

        public new IDoubleIsExpression Is => new DoubleIsExpression(Expression);
    }

    public class DoubleTest : ValueTest<double?, DoubleThat>
    {
        internal DoubleTest(double? actual, IConstraint<double?> constraint)
            : base(actual, constraint) { }

        private protected override That<double?> ApplyModifier(double? actual, ConstraintModifier<double?> modifier)
        {
            return new DoubleThat(new DoubleExpression(actual, modifier));
        }
    }

    internal class DoubleExpression : ValueExpression<double?, IDoubleExpression, DoubleTest>, IDoubleExpression
    {
        internal DoubleExpression(double? actual, ConstraintModifier<double?> modifier)
            : base(actual, modifier) { }

        private protected override DoubleTest ApplyConstraint(double? actual, IConstraint<double?> constraint)
        {
            return new DoubleTest(actual, constraint);
        }

        private protected override IDoubleExpression ApplyModifier(double? actual, ConstraintModifier<double?> modifier)
        {
            return new DoubleExpression(actual, modifier);
        }
    }

    internal class DoubleIsExpression : IDoubleIsExpression
    {
        private readonly IDoubleExpression expression;

        internal DoubleIsExpression(IDoubleExpression expression)
        {
            Debug.Assert(expression != null);

            this.expression = expression;
        }

        public IDoubleIsExpression ApplyModifier(ConstraintModifier<double?> modifier)
        {
            return new DoubleIsExpression(expression.ApplyModifier(modifier));
        }

        public DoubleTest ApplyConstraint(IConstraint<double?> constraint)
        {
            return expression.ApplyConstraint(constraint);
        }
    }
}
