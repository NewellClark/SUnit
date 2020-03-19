using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.NewAssertions
{
    public delegate IConstraint<T> ConstraintModifier<T>(IConstraint<T> constraint);

    public interface IValueExpression<T>
    {
        public abstract ValueTest<T> ApplyConstraint(IConstraint<T> constraint);

        public abstract IValueExpression<T> ApplyModifier(ConstraintModifier<T> modifier);
    }


    internal abstract class ValueExpression<T> : IValueExpression<T>
    {
        private readonly T actual;
        private readonly ConstraintModifier<T> modifier;

        protected private ValueExpression(T actual, ConstraintModifier<T> modifier)
        {
            Debug.Assert(modifier != null);

            this.actual = actual;
            this.modifier = modifier;
        }

        protected private abstract ValueTest<T> ApplyConstraint(T actual, IConstraint<T> constraint);

        protected private abstract IValueExpression<T> ApplyModifier(T actual, ConstraintModifier<T> modifier);

        public ValueTest<T> ApplyConstraint(IConstraint<T> constraint)
        {
            if (constraint is null) throw new ArgumentNullException(nameof(constraint));

            return ApplyConstraint(actual, modifier(constraint));
        }

        public IValueExpression<T> ApplyModifier(ConstraintModifier<T> modifier)
        {
            if (modifier is null) throw new ArgumentNullException(nameof(modifier));

            IConstraint<T> combinedModifier(IConstraint<T> constraint)
            {
                var existing = this.modifier;
                var @new = modifier;

                return existing(@new(constraint));
            }

            return ApplyModifier(actual, combinedModifier);
        }
    }


    //public interface IValueExpression<T, TExpression, TTest, TThat>
    //    where TExpression : IValueExpression<T, TExpression, TTest, TThat>
    //    where TTest : ValueTest<T, TExpression, TTest, TThat>
    //{
    //    public abstract TTest ApplyConstraint(IConstraint<T> constraint);

    //    public abstract TExpression ApplyModifier(ConstraintModifier<T> modifier);
    //}

    //internal abstract class ValueExpression<T, TExpression, TTest, TThat>
    //    : IValueExpression<T, TExpression, TTest, TThat>
    //    where TExpression : IValueExpression<T, TExpression, TTest, TThat>
    //    where TTest : ValueTest<T, TExpression, TTest, TThat>
    //{
    //    private readonly T actual;
    //    private readonly ConstraintModifier<T> modifier;

    //    protected private ValueExpression(T actual, ConstraintModifier<T> modifier)
    //    {
    //        Debug.Assert(modifier != null);

    //        this.actual = actual;
    //        this.modifier = modifier;
    //    }

    //    protected private abstract TTest ApplyConstraint(T actual, IConstraint<T> constraint);

    //    protected private abstract TExpression ApplyModifier(T actual, ConstraintModifier<T> modifier);

    //    public TTest ApplyConstraint(IConstraint<T> constraint)
    //    {
    //        if (constraint is null) throw new ArgumentNullException(nameof(constraint));

    //        return ApplyConstraint(actual, modifier(constraint));
    //    }

    //    public TExpression ApplyModifier(ConstraintModifier<T> modifier)
    //    {
    //        if (modifier is null) throw new ArgumentNullException(nameof(modifier));

    //        IConstraint<T> combinedModifier(IConstraint<T> constraint)
    //        {
    //            var existing = this.modifier;
    //            var @new = modifier;

    //            return existing(@new(constraint));
    //        }

    //        return ApplyModifier(actual, combinedModifier);
    //    }
    //}
}
