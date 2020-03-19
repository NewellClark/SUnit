using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// Creates a new <see cref="IConstraint{T}"/> by modifying an existing one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="constraint">The constraint to modify.</param>
    /// <returns>A new <see cref="IConstraint{T}"/> created by applying the modifier function to
    /// the specified constraint.</returns>
    public delegate IConstraint<T> ConstraintModifier<T>(IConstraint<T> constraint);

    /// <summary>
    /// An expression that contains a value under test.
    /// </summary>
    /// <typeparam name="T">Type of value under test.</typeparam>
    public interface IValueExpression<T>
    {
        /// <summary>
        /// Applies a <see cref="IConstraint{T}"/> to the value under test, producing
        /// a <see cref="ValueTest{T}"/>.
        /// </summary>
        /// <param name="constraint">The <see cref="IConstraint{T}"/> to apply.</param>
        /// <returns>A <see cref="ValueTest{T}"/> created from the value under test and the <see cref="IConstraint{T}"/>.</returns>
        public abstract ValueTest<T> ApplyConstraint(IConstraint<T> constraint);

        /// <summary>
        /// Crates a new <see cref="IValueExpression{T}"/> by applying a <see cref="ConstraintModifier{T}"/> 
        /// to the current <see cref="IValueExpression{T}"/>.
        /// Any <see cref="IConstraint{T}"/>s that are applied will be modified by the constraint modifier.
        /// </summary>
        /// <param name="modifier">The modifier to apply to the current expression.</param>
        /// <returns>A new <see cref="IValueExpression{T}"/> created by applying the modifier to the current 
        /// <see cref="IValueExpression{T}"/>.</returns>
        public abstract IValueExpression<T> ApplyModifier(ConstraintModifier<T> modifier);
    }


    /// <inheritdoc/>
    public interface IValueExpression<T, TExpression, TTest> : IValueExpression<T>
        where TExpression : IValueExpression<T>
        where TTest : ValueTest<T>
    {
        /// <inheritdoc/>
        public new TTest ApplyConstraint(IConstraint<T> constraint);

        ValueTest<T> IValueExpression<T>.ApplyConstraint(IConstraint<T> constraint) => ApplyConstraint(constraint);

        /// <inheritdoc/>
        public new TExpression ApplyModifier(ConstraintModifier<T> modifier);

        IValueExpression<T> IValueExpression<T>.ApplyModifier(ConstraintModifier<T> modifier) => ApplyModifier(modifier);
    }


    internal abstract class ValueExpression<T, TExpression, TTest> 
        : IValueExpression<T, TExpression, TTest>
        where TExpression : IValueExpression<T>
        where TTest : ValueTest<T>
    {
        private readonly T actual;
        private readonly ConstraintModifier<T> modifier;

        protected private ValueExpression(T actual, ConstraintModifier<T> modifier)
        {
            Debug.Assert(modifier != null);

            this.actual = actual;
            this.modifier = modifier;
        }

        protected private abstract TTest ApplyConstraint(T actual, IConstraint<T> constraint);

        protected private abstract TExpression ApplyModifier(T actual, ConstraintModifier<T> modifier);

        public TTest ApplyConstraint(IConstraint<T> constraint)
        {
            if (constraint is null) throw new ArgumentNullException(nameof(constraint));

            return ApplyConstraint(actual, modifier(constraint));
        }

        public TExpression ApplyModifier(ConstraintModifier<T> modifier)
        {
            if (modifier is null) throw new ArgumentNullException(nameof(modifier));

            IConstraint<T> combinedModifier(IConstraint<T> constraint)
            {
                Debug.Assert(constraint != null);

                var existing = this.modifier;
                var @new = modifier;

                return existing(@new(constraint));
            }

            return ApplyModifier(actual, combinedModifier);
        }
    }

}
