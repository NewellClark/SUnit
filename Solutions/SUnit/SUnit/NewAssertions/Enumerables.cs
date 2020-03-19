using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SUnit.NewAssertions
{
    public interface IEnumerableExpression<T> 
        : IValueExpression<IEnumerable<T>, IEnumerableExpression<T>, EnumerableTest<T>> { }


    public interface IEnumerableIsExpression<T>
        : IIsExpression<IEnumerable<T>, IEnumerableIsExpression<T>, EnumerableTest<T>>
    {
        public EnumerableTest<T> SetEqualTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new SetEqualityConstraint<T>(expected, comparer));
        }

        public EnumerableTest<T> SetEqualTo(IEnumerable<T> expected)
        {
            return SetEqualTo(expected, EqualityComparer<T>.Default);
        }

        public EnumerableTest<T> SetEqualTo(params T[] expected)
        {
            return SetEqualTo(expected?.AsEnumerable());
        }
        
        public EnumerableTest<T> SequenceEqualTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new SequenceEqualityConstraint<T>(expected, comparer));
        }

        public EnumerableTest<T> SequenceEqualTo(IEnumerable<T> expected)
        {
            return SequenceEqualTo(expected, EqualityComparer<T>.Default);
        }

        public EnumerableTest<T> SequenceEqualTo(params T[] expected)
        {
            return SequenceEqualTo(expected?.AsEnumerable());
        }
        
        public EnumerableTest<T> EquivalentTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new EquivalentToConstraint<T>(expected, comparer));
        }

        public EnumerableTest<T> EquivalentTo(IEnumerable<T> expected)
        {
            return EquivalentTo(expected, EqualityComparer<T>.Default);
        }

        public EnumerableTest<T> EquivalentTo(params T[] expected)
        {
            return EquivalentTo(expected?.AsEnumerable());
        }
    }


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


    public class EnumerableThat<T> : That<IEnumerable<T>>
    {
        internal EnumerableThat(IEnumerable<T> actual) 
            : this(new EnumerableExpression<T>(actual, c => c)) { }

        internal EnumerableThat(IEnumerableExpression<T> expression)
            : base(expression) { }

        protected private new IEnumerableExpression<T> Expression => (IEnumerableExpression<T>)base.Expression;

        public new IEnumerableIsExpression<T> Is => new EnumerableIsExpression<T>(Expression);
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
