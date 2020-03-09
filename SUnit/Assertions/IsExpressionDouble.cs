using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
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

        public IsTestDouble Zero => FailWhenNull(n => n == 0.0);
        public IsTestDouble Negative => FailWhenNull(n => double.IsNegative(n));
        public IsTestDouble Positive => FailWhenNull(n => n > 0.0);
        public IsTestDouble NaN => FailWhenNull(n => double.IsNaN(n));
    }

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
