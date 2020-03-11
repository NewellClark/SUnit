using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// A <see cref="Test"/> created by applying a <see cref="IConstraint{T}"/> to a nullable decimal.
    /// </summary>
    public class IsTestDecimal : ActualValueTest<decimal?, IIsExpressionDecimal, IsTestDecimal>
    {
        internal IsTestDecimal(decimal? actual, IConstraint<decimal?> constraint) : base(actual, constraint) { }

        /// <inheritdoc/>
        private protected override IIsExpressionDecimal CreateExpression(decimal? actual, ConstraintModifier<decimal?> constraintModifier)
        {
            return new IsExpressionDecimal(actual, constraintModifier);
        }
    }
}
