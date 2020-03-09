using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// The type of <see cref="Test"/> that is returned when test methods are called on 
    /// the <see cref="That{TActual}.Is"/> property when creating assertions.
    /// </summary>
    /// <typeparam name="T">The type of the actual value that is being tested.</typeparam>
    public sealed class IsTest<T> : ActualValueTest<T, IIsExpression<T>, IsTest<T>>
    {
        internal IsTest(T actual, IConstraint<T> constraint) : base(actual, constraint) { }

        protected private override IIsExpression<T> CreateExpression(T actual, ConstraintModifier<T> constraintModifier)
        {
            return new IsExpression<T>(actual, constraintModifier);
        }
    }
}
