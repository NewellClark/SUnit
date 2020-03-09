using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    public class IsTestDouble : ActualValueTest<double?, IIsExpressionDouble, IsTestDouble>
    {
        internal IsTestDouble(double? actual, IConstraint<double?> constraint) 
            : base(actual, constraint) { }

        private protected override IIsExpressionDouble CreateExpression(double? actual, ConstraintModifier<double?> constraintModifier)
        {
            return new IsExpressionDouble(actual, constraintModifier);
        }
    }
}
