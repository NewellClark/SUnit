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

        protected private abstract IValueExpression<T> ApplyModifier(T actual, ConstraintModifier<T> modifier);

        public That<T> ApplyModifier(ConstraintModifier<T> modifier)
        {
            if (modifier is null) throw new ArgumentNullException(nameof(modifier));

            return new That<T>(ApplyModifier(actual, modifier));
        }

        public That<T> And => ApplyModifier(constraint => this.constraint & constraint);

        public That<T> Or => ApplyModifier(constraint => this.constraint | constraint);

        public That<T> Xor => ApplyModifier(constraint => this.constraint ^ constraint);
    }




    //public abstract class ValueTest<T, TExpression, TTest, TThat> : Test
    //    where TExpression : IValueExpression<T, TExpression, TTest, TThat>
    //    where TTest : ValueTest<T, TExpression, TTest, TThat>
    //{
    //    private readonly T actual;
    //    private readonly IConstraint<T> constraint;

    //    internal ValueTest(T actual, IConstraint<T> constraint)
    //    {
    //        Debug.Assert(constraint != null);

    //        this.actual = actual;
    //        this.constraint = constraint;
    //    }

    //    public sealed override bool Passed => constraint.Apply(actual);

    //    protected private abstract TThat ApplyModifier(T actual, ConstraintModifier<T> modifier);

    //    public TThat ApplyModifier(ConstraintModifier<T> modifier)
    //    {
    //        if (modifier is null) throw new ArgumentNullException(nameof(modifier));

    //        return ApplyModifier(actual, modifier);
    //    }

    //    public TThat And => ApplyModifier(c => constraint & c);

    //    public TThat Or => ApplyModifier(c => constraint | c);

    //    public new TThat Xor => ApplyModifier(c => constraint ^ c);
    //}
}
