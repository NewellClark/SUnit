using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// The return type of <see cref="ThatDouble.Is"/>. Used to apply constraints to nullable doubles.
    /// </summary>
    public interface IIsExpressionDouble : IIsExpression<double?, IIsExpressionDouble, IsTestDouble>
    {
        private IsTestDouble FailWhenNull(Predicate<double> predicate)
        {
            bool lifted(double? value)
            {
                if (!value.HasValue)
                    return false;
                return predicate(value.Value);
            }

            return ApplyConstraint(lifted);
        }

        /// <summary>
        /// Tests that the actual value is zero.
        /// </summary>
        public IsTestDouble Zero => FailWhenNull(n => n == 0.0);

        /// <summary>
        /// Tests that the actual value is negative.
        /// </summary>
        public IsTestDouble Negative => FailWhenNull(n => double.IsNegative(n));

        /// <summary>
        /// Tests that the actual value is positive. Zero is NOT positive!
        /// </summary>
        public IsTestDouble Positive => FailWhenNull(n => n > 0.0);

        /// <summary>
        /// Tests that the actual value is NaN (basically you divided zero by zero or something crazy like that).
        /// </summary>
        public IsTestDouble NaN => FailWhenNull(n => double.IsNaN(n));
    }

    /// <summary>
    /// The implementation of <see cref="IIsExpressionDouble"/> that is used by SUnit.
    /// </summary>
    internal class IsExpressionDouble : ActualValueExpression<double?, IIsExpressionDouble, IsTestDouble>, IIsExpressionDouble
    {
        internal IsExpressionDouble(double? actual, ConstraintModifier<double?> constraintModifier) 
            : base(actual, constraintModifier) { }
        internal IsExpressionDouble(double? actual) 
            : base(actual, c => c) { }

        private protected override IsTestDouble CreateTest(double? actual, IConstraint<double?> constraint)
        {
            return new IsTestDouble(actual, constraint);
        }

        private protected override IIsExpressionDouble CreateExpression(double? actual, ConstraintModifier<double?> constraintModifier)
        {
            return new IsExpressionDouble(actual, constraintModifier);
        }


    }
}
