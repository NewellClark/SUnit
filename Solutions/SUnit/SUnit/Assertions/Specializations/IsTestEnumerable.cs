using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <inheritdoc/>
    public class IsTestEnumerable<T> : ActualValueTest<IEnumerable<T>, IIsExpressionEnumerable<T>, IsTestEnumerable<T>>
    {
        internal IsTestEnumerable(IEnumerable<T> actual, IConstraint<IEnumerable<T>> constraint) 
            : base(actual, constraint) { }

        private protected override IIsExpressionEnumerable<T> CreateExpression(
            IEnumerable<T> actual, ConstraintModifier<IEnumerable<T>> constraintModifier)
        {
            return new IsExpressionEnumerable<T>(actual, constraintModifier);
        }
    }
}
