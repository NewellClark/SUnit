using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// A test created by applying a constraint to a nullable decimal.
    /// </summary>
    public class IsTestDecimal : ActualValueTest<decimal?, IIsExpressionDecimal, IsTestDecimal>
    {
        internal IsTestDecimal(decimal? actual, IConstraint<decimal?> constraint) : base(actual, constraint) { }

        private protected override IIsExpressionDecimal CreateExpression(decimal? actual, ConstraintModifier<decimal?> constraintModifier)
        {
            return new IsExpressionDecimal(actual, constraintModifier);
        }
    }
}
