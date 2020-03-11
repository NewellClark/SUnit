using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUnit.Assertions
{
    public interface IIsExpressionEnumerable<T> : 
        IIsExpression<IEnumerable<T>, IIsExpressionEnumerable<T>, IsTestEnumerable<T>>
    {
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
        /// <param name="expected"></param>
        /// <returns></returns>
        public IsTestEnumerable<T> SetEqualTo(IEnumerable<T> expected) => SetEqualTo(expected, EqualityComparer<T>.Default);

        
        public IsTestEnumerable<T> SequenceEqualTo(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return ApplyConstraint(new SequenceEqualityConstraint<T>(expected, comparer));
        }


        public IsTestEnumerable<T> SequenceEqualTo(IEnumerable<T> expected)
        {
            return SequenceEqualTo(expected, EqualityComparer<T>.Default);
        }
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
