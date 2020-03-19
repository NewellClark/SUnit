using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SUnit.Assertions
{
    /// <inheritdoc/>
    public interface IEnumerableIsExpression<T>
        : IIsExpression<IEnumerable<T>, IEnumerableIsExpression<T>, EnumerableTest<T>>
    {
        /// <summary>
        /// Determines if two sequences have the same elements. Order does not matter, and 
        /// duplicates do not matter.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="comparer">The equality comparer to use to compare elements.</param>
        /// <returns></returns>
        public EnumerableTest<T> SetEqualTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new SetEqualityConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Determines if two sequences have the same elements. Order does not matter, and 
        /// duplicates do not matter.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns></returns>
        public EnumerableTest<T> SetEqualTo(IEnumerable<T> expected)
        {
            return SetEqualTo(expected, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Determines if two sequences have the same elements. Order does not matter, and 
        /// duplicates do not matter.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns></returns>
        public EnumerableTest<T> SetEqualTo(params T[] expected)
        {
            return SetEqualTo(expected?.AsEnumerable());
        }
        
        /// <summary>
        /// Determines whether two sequences have the same elements in the same order.
        /// </summary>
        /// <param name="expected">The sequence we expect.</param>
        /// <param name="comparer">The equality comparer to use to compare elements.</param>
        /// <returns></returns>
        public EnumerableTest<T> SequenceEqualTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new SequenceEqualityConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Determines whether two sequences have the same elements in the same order.
        /// </summary>
        /// <param name="expected">The sequence we expect.</param>
        /// <returns></returns>
        public EnumerableTest<T> SequenceEqualTo(IEnumerable<T> expected)
        {
            return SequenceEqualTo(expected, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Tests whether two sequences have the same elements in the same order.
        /// </summary>
        /// <param name="expected">The sequence we expect.</param>
        /// <returns></returns>
        public EnumerableTest<T> SequenceEqualTo(params T[] expected)
        {
            return SequenceEqualTo(expected?.AsEnumerable());
        }
        
        /// <summary>
        /// Tests whether two sequences have all the same elements, but in any order. 
        /// Duplicates do matter, but order does not.
        /// </summary>
        /// <param name="expected">The sequence we expect.</param>
        /// <param name="comparer">The equality comparer to use to compare elements.</param>
        /// <returns></returns>
        public EnumerableTest<T> EquivalentTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new EquivalentToConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Tests whether two sequences have all the same elements, but in any order. 
        /// Duplicates do matter, but order does not.
        /// </summary>
        /// <param name="expected">The sequence we expect.</param>
        /// <returns></returns>
        public EnumerableTest<T> EquivalentTo(IEnumerable<T> expected)
        {
            return EquivalentTo(expected, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Tests whether two sequences have all the same elements, but in any order. 
        /// Duplicates do matter, but order does not.
        /// </summary>
        /// <param name="expected">The sequence we expect.</param>
        /// <returns></returns>
        public EnumerableTest<T> EquivalentTo(params T[] expected)
        {
            return EquivalentTo(expected?.AsEnumerable());
        }

        /// <summary>
        /// Tests whether the sequence is empty.
        /// </summary>
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

        /// <summary>
        /// Tests whether the sequence is a superset of another sequence.
        /// </summary>
        /// <param name="expected">The sequence we expect to be a superset of.</param>
        /// <param name="comparer">The equality comparer to use for comparing items.</param>
        /// <returns></returns>
        public EnumerableTest<T> SupersetOf(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            return ApplyConstraint(new SupersetOfConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Tests whether the sequence is a superset of another sequence.
        /// </summary>
        /// <param name="expected">The sequence we expect to be a superset of.</param>
        /// <returns></returns>
        public EnumerableTest<T> SupersetOf(IEnumerable<T> expected)
        {
            return ApplyConstraint(new SupersetOfConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the sequence is a subset of another sequence.
        /// </summary>
        /// <param name="expected">The sequence we expect to be a subset of.</param>
        /// <param name="comparer">The comparer to use to compare elements.</param>
        /// <returns></returns>
        public EnumerableTest<T> SubsetOf(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            return ApplyConstraint(new SubsetOfConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Tests whether the sequence is a subset of another sequence.
        /// </summary>
        /// <param name="expected">The sequence we expect to be a subset of.</param>
        /// <returns></returns>
        public EnumerableTest<T> SubsetOf(IEnumerable<T> expected)
        {
            return ApplyConstraint(new SubsetOfConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the sequence is a proper superset of another sequence.
        /// </summary>
        /// <param name="expected">The sequence we expect to be a proper superset of.</param>
        /// <param name="comparer">The equality comparer to use to compare elements.</param>
        /// <returns></returns>
        public EnumerableTest<T> ProperSupersetOf(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            return ApplyConstraint(new ProperSupersetOfConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Tests whether the sequence is a proper superset of another sequence.
        /// </summary>
        /// <param name="expected">The sequence we expect to be a proper superset of.</param>
        /// <returns></returns>
        public EnumerableTest<T> ProperSupersetOf(IEnumerable<T> expected)
        {
            return ApplyConstraint(new ProperSupersetOfConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the sequence is a proper subset of another sequence.
        /// </summary>
        /// <param name="expected">The sequence we expect to be a proper subset of.</param>
        /// <param name="comparer">The equality comparer to use to compare elements.</param>
        /// <returns></returns>
        public EnumerableTest<T> ProperSubsetOf(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            return ApplyConstraint(new ProperSubsetOfConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Tests whether the sequence is a proper subset of another sequence.
        /// </summary>
        /// <param name="expected">The sequence we expect to be a proper subset of.</param>
        /// <returns></returns>
        public EnumerableTest<T> ProperSubsetOf(IEnumerable<T> expected)
        {
            return ApplyConstraint(new ProperSubsetOfConstraint<T>(expected));
        }
    }
}
