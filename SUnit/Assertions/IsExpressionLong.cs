using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    public interface IIsExpressionLong : IIsExpression<long?, IIsExpressionLong, IsTestLong>
    {
        private IsTestLong FailWhenNull(Predicate<long> predicate)
        {
            bool lifted(long? value)
            {
                if (!value.HasValue)
                    return false;
                return predicate(value.Value);
            }

            return ApplyConstraint(lifted);
        }

        public IsTestLong Zero => EqualTo(0L);

        public IsTestLong Positive => this.GreaterThan(0L);

        public IsTestLong Negative => this.LessThan(0L);
    }

    internal class IsExpressionLong : ActualValueExpression<long?, IIsExpressionLong, IsTestLong>, IIsExpressionLong
    {
        internal IsExpressionLong(long? actual, ConstraintModifier<long?> constraintModifier)
            : base(actual, constraintModifier) { }
        internal IsExpressionLong(long? actual) : base(actual, c => c) { }

        private protected override IsTestLong CreateTest(long? actual, IConstraint<long?> constraint)
        {
            return new IsTestLong(actual, constraint);
        }

        private protected override IIsExpressionLong CreateExpression(long? actual, ConstraintModifier<long?> constraintModifier)
        {
            return new IsExpressionLong(actual, constraintModifier);
        }
    }
}
