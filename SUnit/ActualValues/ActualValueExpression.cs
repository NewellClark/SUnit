using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// A base class for implementing <see cref="IActualValueExpression{T, TExpression, TTest}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TExpression">This should be set to your own type, or an interface you implement.</typeparam>
    /// <typeparam name="TTest">The type of <see cref="Test"/> that is created by calling 
    /// <see cref="ApplyConstraint(IConstraint{T})"/>. Must be derived from 
    /// <see cref="ActualValueTest{T, TExpression, TTest}"/>.</typeparam>
    /// <remarks>
    /// Subclassing is very straightforward. Derived classes will have almost no logic in them. The main reason for subclassing
    /// in to allow specialization for certain types of actual value. For example, we want to support special
    /// operations such as <c>Is.Empty</c> for collections, and <c>Is.True</c> and <c>Is.False</c> for booleans.
    /// </remarks>
    internal abstract class ActualValueExpression<T, TExpression, TTest> : IActualValueExpression<T, TExpression, TTest>
        where TExpression : IActualValueExpression<T, TExpression, TTest>
        where TTest : ActualValueTest<T, TExpression, TTest>
    {
        private readonly T actual;
        private readonly ConstraintModifier<T> constraintModifier;

        /// <summary>
        /// Creates a new <see cref="ActualValueExpression{T, TExpression, TTest}"/> for the specified
        /// actual value and with the specified constraint modifier.
        /// </summary>
        /// <param name="actual">The actual value that is under test.</param>
        /// <param name="constraintModifier">A function to modify any constraints that are applied
        /// to the actual value.</param>
        protected private ActualValueExpression(T actual, ConstraintModifier<T> constraintModifier)
        {
            Debug.Assert(constraintModifier != null);

            this.actual = actual;
            this.constraintModifier = constraintModifier;
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="ActualValueTest{T, TExpression, TTest}"/>
        /// by applying the specified constraint to the specified actual value.
        /// </summary>
        /// <param name="actual">The actual value that is being tested.</param>
        /// <param name="constraint">The constraint to apply to the actual value.</param>
        /// <returns>
        /// An <see cref="ActualValueTest{T, TExpression, TTest}"/> that applies the specified constraint
        /// to the specified actual value.
        /// </returns>
        protected private abstract TTest CreateTest(T actual, IConstraint<T> constraint);

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="ActualValueExpression{T, TExpression, TTest}"/> 
        /// with the specified actual value and the specified constraint modifier.
        /// </summary>
        /// <param name="actual">The actual value that is under test.</param>
        /// <param name="constraintModifier">A function to modify any constraints that are applied.</param>
        /// <returns>
        /// A new <see cref="ActualValueExpression{T, TExpression, TTest}"/> for the specified actual value
        /// and with the specified constraint modifier function.
        /// </returns>
        /// <remarks>
        /// This should simply be passing the arguments through to your own constructor, which in turn should 
        /// just be passing the arguments to the base class constructor.
        /// </remarks>
        protected private abstract TExpression CreateExpression(T actual, ConstraintModifier<T> constraintModifier);

        public TTest ApplyConstraint(IConstraint<T> constraint)
        {
            if (constraint is null) throw new ArgumentNullException(nameof(constraint));

            return CreateTest(actual, constraintModifier(constraint));
        }


        public TExpression ApplyModifier(ConstraintModifier<T> constraintModifier)
        {
            if (constraintModifier is null) throw new ArgumentNullException(nameof(constraintModifier));

            IConstraint<T> combinedConstraintModifier(IConstraint<T> constraint)
            {
                var existing = this.constraintModifier;
                var @new = constraintModifier;

                return existing(@new(constraint));
            }

            return CreateExpression(actual, combinedConstraintModifier);
        }
    }
}
