using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUnit.Assertions
{
    /// <inheritdoc/>
    public interface IIsExpressionEnumerable<T> : 
        IIsExpression<IEnumerable<T>, IIsExpressionEnumerable<T>, IsTestEnumerable<T>>
    {
        /// <summary>
        /// Tests whether the sequence is empty. Null is NOT empty!
        /// </summary>
        public IsTestEnumerable<T> Empty
        {
            get
            {
                var predicate = Constraint.NullIsFalse<IEnumerable<T>>(items => !items.Any());
                
                return ApplyConstraint(predicate);
            }
        }
        
        /// <summary>
        /// Tests whether the actual sequence has all the same elements as the expected sequence. Order does not matter,
        /// and duplicates do not matter.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="comparer">The equality comparer to use to compare items.</param>
        /// <returns></returns>
        public IsTestEnumerable<T> SetEqualTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new SetEqualityConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Tests whether the actual sequence has all the same elements as the expected sequence. Order does not matter, and
        /// duplicates do not matter.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>A test that passes if the actual collection has all the same distinct items as
        /// the expected value.</returns>
        public IsTestEnumerable<T> SetEqualTo(IEnumerable<T> expected) => SetEqualTo(expected, EqualityComparer<T>.Default);

        /// <summary>
        /// Tests whether the actual sequence has all the same elements as the expected sequence. Order does not matter, and
        /// duplicates do not matter.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>A test that passes if the actual collection has all the same distinct items as
        /// the expected value.</returns>
        public IsTestEnumerable<T> SetEqualTo(params T[] expected) => SetEqualTo(expected.AsEnumerable());

        /// <summary>
        /// Tests whether the actual sequence has the same elements in the same order.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="comparer">The equality comparer to use to compare elements.</param>
        /// <returns>A test that tests whether the actual sequence has the same elements in the same order
        /// as the expected sequence.</returns>
        public IsTestEnumerable<T> SequenceEqualTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new SequenceEqualityConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Tests whether the current sequence has all the same items in the same order as the expected sequence.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns>A test that verifies that the actual sequence and the expected sequence have 
        /// the same items in the same order.</returns>
        public IsTestEnumerable<T> SequenceEqualTo(IEnumerable<T> expected)
        {
            return SequenceEqualTo(expected, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Tests whether the current sequence has all the same items in the same order as the expected sequence.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns>A test that verifies that the actual sequence and the expected sequence have 
        /// the same items in the same order.</returns>
        public IsTestEnumerable<T> SequenceEqualTo(params T[] expected) => SequenceEqualTo(expected.AsEnumerable());

        /// <summary>
        /// Tests whether the current sequence has the same number of all the same items, in any old order.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public IsTestEnumerable<T> EquivalentTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new EquivalentToConstraint<T>(expected, comparer));
        }

        /// <summary>
        /// Tests whether the current sequence has the same number of all the same items as the expected sequence.
        /// Order does not matter.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public IsTestEnumerable<T> EquivalentTo(IEnumerable<T> expected)
        {
            return EquivalentTo(expected, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Tests whether the current sequence has the same number of all the same items as the expected sequence.
        /// Order does not matter.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public IsTestEnumerable<T> EquivalentTo(params T[] expected) => EquivalentTo(expected.AsEnumerable());
    }

    internal class IsExpressionEnumerable<T> : 
        ActualValueExpression<IEnumerable<T>, IIsExpressionEnumerable<T>, IsTestEnumerable<T>>,
        IIsExpressionEnumerable<T>
    {
        internal IsExpressionEnumerable(IEnumerable<T> actual, ConstraintModifier<IEnumerable<T>> constraintModifier)
            : base(actual, constraintModifier) { }
        internal IsExpressionEnumerable(IEnumerable<T> actual) : base(actual, c => c) { }

        private protected override IsTestEnumerable<T> CreateTest(IEnumerable<T> actual, IConstraint<IEnumerable<T>> constraint)
        {
            return new IsTestEnumerable<T>(actual, constraint);
        }

        private protected override IIsExpressionEnumerable<T> CreateExpression(
            IEnumerable<T> actual, ConstraintModifier<IEnumerable<T>> constraintModifier)
        {
            return new IsExpressionEnumerable<T>(actual, constraintModifier);
        }
    }
}
