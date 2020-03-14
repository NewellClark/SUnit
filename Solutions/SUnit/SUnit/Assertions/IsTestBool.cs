using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// The type that is returned when you say <code>Assert.That(something).Is.True;</code>.
    /// </summary>
    public class IsTestBool : ActualValueTest<bool?, IIsExpressionBool, IsTestBool>
    {
        internal IsTestBool(bool? actual, IConstraint<bool?> constraint)
            : base(actual, constraint) { }

        protected private override IIsExpressionBool CreateExpression(bool? actual, ConstraintModifier<bool?> constraintModifier)
        {
            return new IsExpressionBool(actual, constraintModifier);
        }
    }
}
