using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// A test that is performed on a value that is under test.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueTest<T> : Test
    {
        private readonly T actual;
        private readonly IConstraint<T> constraint;

        protected private ValueTest(T actual, IConstraint<T> constraint)
        {
            Debug.Assert(constraint != null);

            this.actual = actual;
            this.constraint = constraint;
        }

        /// <inheritdoc/>
        public sealed override bool Passed => constraint.Apply(actual);

        protected private abstract That<T> ApplyModifier(T actual, ConstraintModifier<T> modifier);

        /// <summary>
        /// Applies the specified modifier to the current <see cref="ValueTest{T}"/>.
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public That<T> ApplyModifier(ConstraintModifier<T> modifier)
        {
            if (modifier is null) throw new ArgumentNullException(nameof(modifier));

            return ApplyModifier(actual, modifier);
        }

        /// <summary>
        /// Boolean AND.
        /// </summary>
        public That<T> And => ApplyModifier(constraint => this.constraint & constraint);

        /// <summary>
        /// Boolean OR.
        /// </summary>
        public That<T> Or => ApplyModifier(constraint => this.constraint | constraint);

        /// <summary>
        /// Exclusive OR, or XOR.
        /// </summary>
        public new That<T> Xor => ApplyModifier(constraint => this.constraint ^ constraint);
    }

    /// <inheritdoc/>
    public abstract class ValueTest<T, TThat> : ValueTest<T>
        where TThat : That<T>
    {
        protected private ValueTest(T actual, IConstraint<T> constraint) 
            : base(actual, constraint) { }

        /// <inheritdoc/>
        public new TThat ApplyModifier(ConstraintModifier<T> modifier) => (TThat)base.ApplyModifier(modifier);

        /// <inheritdoc/>
        public new TThat And => (TThat)base.And;

        /// <inheritdoc/>
        public new TThat Or => (TThat)base.Or;

        /// <inheritdoc/>
        public new TThat Xor => (TThat)base.Xor;
    }
}
