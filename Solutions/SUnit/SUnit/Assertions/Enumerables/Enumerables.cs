using SUnit.Constraints;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Assertions
{
    public interface IEnumerableExpression<T> 
        : IValueExpression<IEnumerable<T>, IEnumerableExpression<T>, EnumerableTest<T>> { }


    internal class EnumerableExpression<T> 
        : ValueExpression<IEnumerable<T>, IEnumerableExpression<T>, EnumerableTest<T>>,
        IEnumerableExpression<T>
    {
        internal EnumerableExpression(IEnumerable<T> actual, ConstraintModifier<IEnumerable<T>> modifier)
            : base(actual, modifier) { }

        private protected override EnumerableTest<T> ApplyConstraint(IEnumerable<T> actual, IConstraint<IEnumerable<T>> constraint)
        {
            return new EnumerableTest<T>(actual, constraint);
        }

        private protected override IEnumerableExpression<T> ApplyModifier(IEnumerable<T> actual, ConstraintModifier<IEnumerable<T>> modifier)
        {
            return new EnumerableExpression<T>(actual, modifier);
        }
    }


    internal class EnumerableIsExpression<T> : IEnumerableIsExpression<T>
    {
        private readonly IEnumerableExpression<T> expression;

        internal EnumerableIsExpression(IEnumerableExpression<T> expression)
        {
            Debug.Assert(expression != null);

            this.expression = expression;
        }

        public IEnumerableIsExpression<T> ApplyModifier(ConstraintModifier<IEnumerable<T>> modifier)
        {
            return new EnumerableIsExpression<T>(expression.ApplyModifier(modifier));
        }

        public EnumerableTest<T> ApplyConstraint(IConstraint<IEnumerable<T>> constraint)
        {
            return expression.ApplyConstraint(constraint);
        }
    }


    public class EnumerableTest<T> : ValueTest<IEnumerable<T>, EnumerableThat<T>>
    {
        internal EnumerableTest(IEnumerable<T> actual, IConstraint<IEnumerable<T>> constraint)
            : base(actual, constraint) { }

        private protected override That<IEnumerable<T>> ApplyModifier(IEnumerable<T> actual, ConstraintModifier<IEnumerable<T>> modifier)
        {
            return new EnumerableThat<T>(new EnumerableExpression<T>(actual, modifier));
        }
    }
}
