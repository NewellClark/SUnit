using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
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
