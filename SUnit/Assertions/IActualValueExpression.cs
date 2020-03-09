using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// A function that produces a new <see cref="IConstraint{T}"/> by modifying an existing <see cref="IConstraint{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value that the <see cref="IConstraint{T}"/> applies to.</typeparam>
    /// <param name="constraint">The input constraint.</param>
    /// <returns>A new <see cref="IConstraint{T}"/> produced by modifying the existing one.</returns>
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

        public TTest ApplyConstraint(Predicate<T> predicate)
        {
            return ApplyConstraint(Constraint.Create(predicate));
        }

        /// <summary>
        /// Applies a constraint modifier to the current <see cref="IActualValueExpression{T, TExpression, TTest}"/>. Any
        /// constraint that gets applied will be modified by the specified modifier.
        /// </summary>
        /// <param name="modifier">A function that modifies a <see cref="IConstraint{T}"/>.</param>
        /// <returns>
        /// A new <see cref="IActualValueExpression{T, TExpression, TTest}"/> with the specified 
        /// constraint modifier applied.
        /// </returns>
        public abstract TExpression ApplyModifier(ConstraintModifier<T> modifier);

        //  Sorry, "Not" is the best name for it. I'll consider putting in 
        //  an alias if it becomes a pain point.
#pragma warning disable CA1716 // Identifiers should not match keywords
        /// <summary>
        /// Inverts any <see cref="IConstraint{T}"/> that is applied to the current 
        /// <see cref="IActualValueExpression{T, TExpression, TTest}"/>.
        /// </summary>
        public TExpression Not => ApplyModifier(constraint => !constraint);
#pragma warning restore CA1716 // Identifiers should not match keywords
    }
}
