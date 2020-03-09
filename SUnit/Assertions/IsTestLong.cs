using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// A <see cref="Test"/> for applying constraints to actual <see cref="long"/> values.
    /// </summary>
    public class IsTestLong : ActualValueTest<long?, IIsExpressionLong, IsTestLong>
    {
        internal IsTestLong(long? actual, IConstraint<long?> constraint) 
            : base(actual, constraint) { }

        private protected override IIsExpressionLong CreateExpression(
            long? actual, ConstraintModifier<long?> constraintModifier)
        {
            return new IsExpressionLong(actual, constraintModifier);
        }
    }
}
