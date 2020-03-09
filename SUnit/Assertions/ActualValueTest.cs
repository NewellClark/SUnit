using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// Base class for a <see cref="Test"/> that is created by applying a <see cref="IConstraint{T}"/> to
    /// given actual value.
    /// </summary>
    /// <typeparam name="T">The type of the actual value under test.</typeparam>
    /// <typeparam name="TExpression">The type of <see cref="IActualValueExpression{T, TExpression, TTest}"/> that 
    /// produced the test. This will be the return value of <see cref="And"/>, <see cref="Or"/>, 
    /// etc. to allow the user to chain multiple constraints on the same value.</typeparam>
    /// <typeparam name="TTest">The type of <see cref="Test"/> produced. This should be the most derived type 
    /// that is publically visible of whatever is subclassing this class.</typeparam>
    public abstract class ActualValueTest<T, TExpression, TTest> : Test
        where TExpression : IActualValueExpression<T, TExpression, TTest>
        where TTest : ActualValueTest<T, TExpression, TTest>
    {
        private readonly T actual;
        private readonly IConstraint<T> constraint;

        /// <summary>
        /// Indicates whether the test passed. 
        /// </summary>
        public sealed override bool Passed => constraint.Apply(actual);

        /// <summary>
        /// Creates a new <see cref="ActualValueTest{T, TExpression, TTest}"/> by applying the specified 
        /// <see cref="IConstraint{T}"/> to the specified actual value. The test passes if the constraint passes.
        /// </summary>
        /// <param name="actual">The actual value to test.</param>
        /// <param name="constraint">The <see cref="IConstraint{T}"/> to apply to the actual value.</param>
        protected private ActualValueTest(T actual, IConstraint<T> constraint)
        {
            Debug.Assert(constraint != null);

            this.actual = actual;
            this.constraint = constraint;
        }

        /// <summary>
        /// Creates a new <see cref="IActualValueExpression{T, TExpression, TTest}"/> by applying a constraint modifier
        /// to the current constraint. This is used to implement <see cref="And"/>, <see cref="Or"/>, etc.
        /// </summary>
        /// <param name="actual">The actual value under test.</param>
        /// <param name="constraintModifier">Constraint modifier function to apply.</param>
        /// <returns></returns>
        protected private abstract TExpression CreateExpression(T actual, ConstraintModifier<T> constraintModifier);

        /// <summary>
        /// Creates a new <see cref="IActualValueExpression{T, TExpression, TTest}"/> with the specified constraint-modifier
        /// function.
        /// </summary>
        /// <param name="constraintModifier">The constraint modifier to apply.</param>
        /// <returns></returns>
        public TExpression ApplyModifier(ConstraintModifier<T> constraintModifier)
        {
            return CreateExpression(actual, constraintModifier);
        }

        /// <summary>
        /// Boolean AND operator.
        /// </summary>
        public TExpression And => ApplyModifier(c => constraint & c);

        /// <summary>
        /// Boolean OR operator.
        /// </summary>
        public TExpression Or => ApplyModifier(c => constraint | c);

        /// <summary>
        /// Boolean XOR operator.
        /// </summary>
        public new TExpression Xor => ApplyModifier(c => constraint ^ c);
    }
}
