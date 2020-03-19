using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using SUnit.Constraints;

namespace SUnit.Assertions
{
    /// <inheritdoc/>
    public interface IDoubleExpression : IValueExpression<double?, IDoubleExpression, DoubleTest> { }


    /// <inheritdoc/>
    public interface IDoubleIsExpression : IIsExpression<double?, IDoubleIsExpression, DoubleTest>
    {
        /// <summary>
        /// Tests whether the actual value is equal to the expected value, within a dynamically-adjusted tolerance.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>A test that passes if the value is almost equal to the expected value. </returns>
        public new DoubleTest EqualTo(double? expected)
        {
            return ApplyConstraint(new FloatingPointEqualToConstraint(expected));
        }

        DoubleTest IIsExpression<double?, IDoubleIsExpression, DoubleTest>.EqualTo(double? expected) => EqualTo(expected);
        
        /// <summary>
        /// Tests whether the actual value is zero.
        /// </summary>
        public DoubleTest Zero => EqualTo(0.0);

        /// <summary>
        /// Tests whether the actual value is positive (zero is not positive).
        /// </summary>
        public DoubleTest Positive => this.GreaterThan(0.0);

        /// <summary>
        /// Tests whether the actual value is negative (zero is not negative).
        /// </summary>
        public DoubleTest Negative => this.LessThan(0.0);

        /// <summary>
        /// Tests whether the actual value is NaN (Not a Number).
        /// </summary>
        public DoubleTest NaN => ApplyConstraint(new NanConstraint());
    }


    /// <inheritdoc/>
    public class DoubleThat : That<double?>
    {
        internal DoubleThat(double? actual) : this(new DoubleExpression(actual, c => c)) { }
        internal DoubleThat(IDoubleExpression expression) : base(expression) { }

        protected private new IDoubleExpression Expression => (IDoubleExpression)base.Expression;

        /// <inheritdoc/>
        public new IDoubleIsExpression Is => new DoubleIsExpression(Expression);
    }


    /// <inheritdoc/>
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
