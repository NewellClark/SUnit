using SUnit.Constraints;
using System;
using System.Collections.Generic;

namespace SUnit.Assertions
{
    public class EnumerableThat<T> : That<IEnumerable<T>>
    {
        internal EnumerableThat(IEnumerable<T> actual) 
            : this(new EnumerableExpression<T>(actual, c => c)) { }

        internal EnumerableThat(IEnumerableExpression<T> expression)
            : base(expression) { }

        protected private new IEnumerableExpression<T> Expression => (IEnumerableExpression<T>)base.Expression;

        public new IEnumerableIsExpression<T> Is => new EnumerableIsExpression<T>(Expression);

        public EnumerableTest<T> Contains(T expected)
        {
            return Expression.ApplyConstraint(new ContainsConstraint<T>(expected));
        }
    }
}
