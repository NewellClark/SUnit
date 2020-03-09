using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// The return value of <see cref="That{TActual}.Is"/>, for values of <typeparamref name="T"/> that 
    /// have not been special-cased.
    /// </summary>
    /// <typeparam name="T">The type of the actual value that is under test.</typeparam>
    public interface IIsExpression<T> : IIsExpression<T, IIsExpression<T>, IsTest<T>> { }

    internal sealed class IsExpression<T> : ActualValueExpression<T, IIsExpression<T>, IsTest<T>>, IIsExpression<T>
    {
        internal IsExpression(T actual, ConstraintModifier<T> constraintModifier)
            : base(actual, constraintModifier) { }

        internal IsExpression(T actual) : this(actual, c => c) { }

        protected private override IsTest<T> CreateTest(T actual, IConstraint<T> constraint)
        {
            return new IsTest<T>(actual, constraint);
        }

        protected private override IIsExpression<T> CreateExpression(T actual, ConstraintModifier<T> constraintModifier)
        {
            return new IsExpression<T>(actual, constraintModifier);
        }
    }
}
