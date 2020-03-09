using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// A <see cref="Test"/> that applies a <see cref="IConstraint{T}"/> to an actual nullable double.
    /// </summary>
    public sealed class IsTestDouble : ActualValueTest<double?, IIsExpressionDouble, IsTestDouble>
    {
        internal IsTestDouble(double? actual, IConstraint<double?> constraint) 
            : base(actual, constraint) { }

        private protected override IIsExpressionDouble CreateExpression(double? actual, ConstraintModifier<double?> constraintModifier)
        {
            return new IsExpressionDouble(actual, constraintModifier);
        }
    }
}
