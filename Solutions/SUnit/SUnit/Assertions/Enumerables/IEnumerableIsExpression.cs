using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SUnit.Assertions
{
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

        public EnumerableTest<T> Empty
        {
            get
            {
                bool predicate(IEnumerable<T> items)
                {
                    return !items?.Any() ?? false;
                }

                return ApplyConstraint(Constraint.FromPredicate<IEnumerable<T>>(predicate));
            }
        }

        public EnumerableTest<T> SupersetOf(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            return ApplyConstraint(new SupersetOfConstraint<T>(expected, comparer));
        }

        public EnumerableTest<T> SupersetOf(IEnumerable<T> expected)
        {
            return ApplyConstraint(new SupersetOfConstraint<T>(expected));
        }

        public EnumerableTest<T> SubsetOf(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            return ApplyConstraint(new SubsetOfConstraint<T>(expected, comparer));
        }

        public EnumerableTest<T> SubsetOf(IEnumerable<T> expected)
        {
            return ApplyConstraint(new SubsetOfConstraint<T>(expected));
        }

        public EnumerableTest<T> ProperSupersetOf(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            return ApplyConstraint(new ProperSupersetOfConstraint<T>(expected, comparer));
        }

        public EnumerableTest<T> ProperSupersetOf(IEnumerable<T> expected)
        {
            return ApplyConstraint(new ProperSupersetOfConstraint<T>(expected));
        }

        public EnumerableTest<T> ProperSubsetOf(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            return ApplyConstraint(new ProperSubsetOfConstraint<T>(expected, comparer));
        }

        public EnumerableTest<T> ProperSubsetOf(IEnumerable<T> expected)
        {
            return ApplyConstraint(new ProperSubsetOfConstraint<T>(expected));
        }
    }
}
