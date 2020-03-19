using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.NewAssertions
{
    public interface IIsExpression<T, TExpression, TTest> : IValueExpression<T>
        where TExpression : IIsExpression<T, TExpression, TTest>
        where TTest : ValueTest<T>
    {
        public new TExpression ApplyModifier(ConstraintModifier<T> modifier);

        IValueExpression<T> IValueExpression<T>.ApplyModifier(ConstraintModifier<T> modifier) => ApplyModifier(modifier);

        public new TTest ApplyConstraint(IConstraint<T> constraint);

        ValueTest<T> IValueExpression<T>.ApplyConstraint(IConstraint<T> constraint) => ApplyConstraint(constraint);

        public TExpression Not => ApplyModifier(constraint => !constraint);


        public TTest EqualTo(T expected)
        {
            var constraint = new EqualToConstraint<T>(expected);

            return ApplyConstraint(constraint);
        }

        public TTest Null => ApplyConstraint(new NullConstraint<T>());
    }

    public interface IIsExpression<T> : IIsExpression<T, IIsExpression<T>, ValueTest<T>> { }
}
