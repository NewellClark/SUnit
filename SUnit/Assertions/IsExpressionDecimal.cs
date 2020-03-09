using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// Contains assertions specific to decimals.
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
        public IsTestDecimal Zero => ApplyConstraint(n => n == decimal.Zero);

        /// <summary>
        /// Tests if the decimal is positive. Zero is NOT positive.
        /// </summary>
        public IsTestDecimal Positive => ApplyConstraint(n => n > 0);

        /// <summary>
        /// Tests if the decimal is negative.
        /// </summary>
        public IsTestDecimal Negative => ApplyConstraint(n => n < 0);
    }

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
