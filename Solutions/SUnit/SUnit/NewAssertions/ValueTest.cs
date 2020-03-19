using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.NewAssertions
{
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

        public sealed override bool Passed => constraint.Apply(actual);

        protected private abstract That<T> ApplyModifier(T actual, ConstraintModifier<T> modifier);

        public That<T> ApplyModifier(ConstraintModifier<T> modifier)
        {
            if (modifier is null) throw new ArgumentNullException(nameof(modifier));

            return ApplyModifier(actual, modifier);
        }

        public That<T> And => ApplyModifier(constraint => this.constraint & constraint);

        public That<T> Or => ApplyModifier(constraint => this.constraint | constraint);

        public new That<T> Xor => ApplyModifier(constraint => this.constraint ^ constraint);
    }


    public abstract class ValueTest<T, TThat> : ValueTest<T>
        where TThat : That<T>
    {
        protected private ValueTest(T actual, IConstraint<T> constraint) 
            : base(actual, constraint) { }

        public new TThat ApplyModifier(ConstraintModifier<T> modifier) => (TThat)base.ApplyModifier(modifier);

        public new TThat And => (TThat)base.And;

        public new TThat Or => (TThat)base.Or;

        public new TThat Xor => (TThat)base.Xor;
    }
}
