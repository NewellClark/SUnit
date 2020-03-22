using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{ 
    /// <summary>
    /// The thing that gets returned whenever you say "is". That's all you need to know.
    /// Don't be intimidated by the long type-names with many generic parameters. Just say "Is", 
    /// and let your intellisense do the talking.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TExpression">The type of the current <see cref="IValueExpression{T}"/>.</typeparam>
    /// <typeparam name="TTest">The type of test created by applying constraints to the current expression.</typeparam>
    public interface IIsExpression<T, TExpression, TTest> : IValueExpression<T>
        where TExpression : IIsExpression<T, TExpression, TTest>
        where TTest : ValueTest<T>
    {
        /// <inheritdoc/>
        public new TExpression ApplyModifier(ConstraintModifier<T> modifier);

        IValueExpression<T> IValueExpression<T>.ApplyModifier(ConstraintModifier<T> modifier) => ApplyModifier(modifier);

        /// <inheritdoc/>
        public new TTest ApplyConstraint(IConstraint<T> constraint);

        ValueTest<T> IValueExpression<T>.ApplyConstraint(IConstraint<T> constraint) => ApplyConstraint(constraint);

#pragma warning disable CA1716 // Identifiers should not match keywords
        /// <summary>
        /// Inverts the next constraint.
        /// </summary>
        public TExpression Not => ApplyModifier(constraint => !constraint);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Asserts that the value under test is equal to the specified expected value.
        /// </summary>
        /// <param name="expected">The value we expect to be equal to.</param>
        /// <returns>A test that passes if the value under test is equal to the specified expected value.</returns>
        public TTest EqualTo(T expected)
        {
            var constraint = new EqualToConstraint<T>(expected);

            return ApplyConstraint(constraint);
        }

        /// <summary>
        /// Asserts that the value under test is null.
        /// </summary>
        public TTest Null => ApplyConstraint(new NullConstraint<T>());
    }
}
