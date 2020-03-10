using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// A function that produces a new <see cref="IConstraint{T}"/> by applying a function to 
    /// an existing <see cref="IConstraint{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type that the <see cref="IConstraint{T}"/> applies to.</typeparam>
    /// <param name="constraint">The input constraint.</param>
    /// <returns>A new <see cref="IConstraint{T}"/> produced by applying a modification to the existing one.</returns>
    public delegate IConstraint<T> ConstraintModifier<T>(IConstraint<T> constraint);

    /// <summary>
    /// An expression that allows users to apply constraints to a supplied actual value.
    /// An example of an <see cref="IActualValueExpression{T, TExpression, TTest}"/> would be the
    /// <see cref="That{TActual}.Is"/> property that's commonly used when creating assertions. 
    /// </summary>
    /// <typeparam name="T">The type of the actual value. </typeparam>
    /// <typeparam name="TExpression">The type of the current <see cref="IActualValueExpression{T, TExpression, TTest}"/>.
    /// Please pretend that C# supports the Curiously Recurring Template Pattern and don't do anything funny.</typeparam>
    /// <typeparam name="TTest">The type of <see cref="Test"/> that is returned when a constraint is applied
    /// to the current <see cref="IActualValueExpression{T, TExpression, TTest}"/>.</typeparam>
    public interface IActualValueExpression<T, TExpression, TTest>
        where TExpression : IActualValueExpression<T, TExpression, TTest>
        where TTest : ActualValueTest<T, TExpression, TTest>
    {
        /// <summary>
        /// Applies a constraint to the current <see cref="IActualValueExpression{T, TExpression, TTest}"/>, producing
        /// a <see cref="Test"/>.
        /// </summary>
        /// <param name="constraint">The constraint to apply.</param>
        /// <returns>A <see cref="Test"/> that passes if the constraint is satisfied.</returns>
        public abstract TTest ApplyConstraint(IConstraint<T> constraint);

        /// <summary>
        /// Creates a <see cref="IConstraint{T}"/> from the specified <see cref="Predicate{T}"/> and
        /// applies it to the current <see cref="IActualValueExpression{T, TExpression, TTest}"/>.
        /// </summary>
        /// <param name="predicate">The <see cref="Predicate{T}"/> to use to build the <see cref="IConstraint{T}"/>.</param>
        /// <returns>A <see cref="Test"/> created by applying the specified constraint to the actual value.</returns>
        public TTest ApplyConstraint(Predicate<T> predicate)
        {
            return ApplyConstraint(Constraint.FromPredicate(predicate));
        }

        /// <summary>
        /// Creates a new <see cref="IActualValueExpression{T, TExpression, TTest}"/> by applying 
        /// a constraint modifier to the current <see cref="IActualValueExpression{T, TExpression, TTest}"/>. Any
        /// constraint that gets applied will be modified by the modifier.
        /// </summary>
        /// <param name="modifier">A function that modifies a <see cref="IConstraint{T}"/>.</param>
        /// <returns>
        /// A new <see cref="IActualValueExpression{T, TExpression, TTest}"/> with the specified 
        /// constraint modifier applied.
        /// </returns>
        public abstract TExpression ApplyModifier(ConstraintModifier<T> modifier);

        //  Sorry, "Not" is the best name for it. Unit test frameworks have traditionally been written 
        //  to read as much like plain english as possible.
        //  I'll consider putting in an alias if it becomes a pain point for Visual Basic programmers.
#pragma warning disable CA1716 // Identifiers should not match keywords
        /// <summary>
        /// Returns a new <see cref="IActualValueExpression{T, TExpression, TTest}"/> that inverts any
        /// <see cref="IConstraint{T}"/> that is applied to it.
        /// </summary>
        public TExpression Not => ApplyModifier(constraint => !constraint);
#pragma warning restore CA1716 // Identifiers should not match keywords
    }
}
