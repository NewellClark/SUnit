using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// The return type of <see cref="ThatLong.Is"/>. Contains members for applying assertions to longs and
    /// nullable longs.
    /// </summary>
    public interface IIsExpressionLong : IIsExpression<long?, IIsExpressionLong, IsTestLong>
    {

        /// <summary>
        /// Tests that the actual value is zero.
        /// </summary>
        public IsTestLong Zero => EqualTo(0L);

        /// <summary>
        /// Tests that the actual value is positive. Zero is NOT positive!
        /// </summary>
        public IsTestLong Positive => this.GreaterThan(0L);

        /// <summary>
        /// Tests that the actual value is negative. Zero is NOT negative!
        /// </summary>
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
