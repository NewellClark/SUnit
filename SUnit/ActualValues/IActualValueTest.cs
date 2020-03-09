using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.ActualValues
{
    public interface IActualValueTest<T, TExpression, TTest>
        where TExpression : IActualValueExpression<T, TExpression, TTest>
        where TTest : IActualValueTest<T, TExpression, TTest>
    {
        public abstract TExpression ApplyModifier(ConstraintModifier<T> modifier);

        protected private abstract IConstraint<T> Constraint { get; }

        public TExpression And
        {
            get
            {
                var constraint = Constraint;
                return ApplyModifier(c => constraint & c);
            }
        }

        public TExpression Or => ApplyModifier(c => Constraint | c);

        public TExpression Xor => ApplyModifier(c => Constraint ^ c);
    }
}
