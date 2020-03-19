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


    internal abstract class ValueExpression<T, TExpression, TTest> 
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
