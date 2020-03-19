using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.NewAssertions
{
    public interface IIsExpression<T> : IValueExpression<T>
    {
        public new IIsExpression<T> ApplyModifier(ConstraintModifier<T> modifier);

        IValueExpression<T> IValueExpression<T>.ApplyModifier(ConstraintModifier<T> modifier) => ApplyModifier(modifier);
        
        public IIsExpression<T> Not => ApplyModifier(constraint => !constraint);


        public ValueTest<T> EqualTo(T expected)
        {
            var constraint = new EqualToConstraint<T>(expected);

            return ApplyConstraint(constraint);
        }

        public ValueTest<T> Null => ApplyConstraint(new NullConstraint<T>());
    }
}
