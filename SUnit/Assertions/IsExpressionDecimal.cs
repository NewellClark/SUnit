using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// The return type of <see cref="ThatDecimal.Is"/>. Used to apply constraints to decimals 
    /// and nullable decimals.
    /// </summary>
    public interface IIsExpressionDecimal : IIsExpression<decimal?, IIsExpressionDecimal, IsTestDecimal>
    {
        private IsTestDecimal FailWhenNull(Predicate<decimal> predicate)
        {
            bool lifted(decimal? value)
            {
                if (!value.HasValue)
                    return false;
                return predicate(value.Value);
            }

            return ApplyConstraint(lifted);
        }

        /// <summary>
        /// Tests if the decimal is exactly zero.
        /// </summary>
        public IsTestDecimal Zero => EqualTo(0m);

        /// <summary>
        /// Tests if the decimal is positive. Zero is NOT positive.
        /// </summary>
        public IsTestDecimal Positive => this.GreaterThan(0m);

        /// <summary>
        /// Tests if the decimal is negative.
        /// </summary>
        public IsTestDecimal Negative => this.LessThan(0m);
    }

    /// <summary>
    /// The implementation of <see cref="IIsExpressionDecimal"/> that is used by SUnit.
    /// </summary>
    internal sealed class IsExpressionDecimal : ActualValueExpression<decimal?, IIsExpressionDecimal, IsTestDecimal>, IIsExpressionDecimal
    {
        internal IsExpressionDecimal(decimal? actual, ConstraintModifier<decimal?> constraintModifier)
            : base(actual, constraintModifier) { }
        internal IsExpressionDecimal(decimal? actual) : base(actual, c => c) { }

        private protected override IsTestDecimal CreateTest(decimal? actual, IConstraint<decimal?> constraint)
        {
            return new IsTestDecimal(actual, constraint);
        }

        private protected override IIsExpressionDecimal CreateExpression(decimal? actual, ConstraintModifier<decimal?> constraintModifier)
        {
            return new IsExpressionDecimal(actual, constraintModifier);
        }
    }
}
