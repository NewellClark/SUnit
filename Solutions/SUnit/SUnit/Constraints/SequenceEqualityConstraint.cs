using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Constraints
{
    internal class SequenceEqualityConstraint<T> : IConstraint<IEnumerable<T>> 
    {
        private readonly IEnumerable<T> expected;
        private readonly IEqualityComparer<T> comparer;

        internal SequenceEqualityConstraint(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            Debug.Assert(comparer != null);

            this.expected = expected;
            this.comparer = comparer;
        }

        public bool Apply(IEnumerable<T> value)
        {
            return Constraint.NullFriendlyEquality(value, expected, CompareNonNull);
        }

        private bool CompareNonNull(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Debug.Assert(actual != null);
            Debug.Assert(expected != null);

            using var leftIter = actual.GetEnumerator();
            using var rightIter = expected.GetEnumerator();

            bool leftMoved; 
            bool rightMoved;

            while ((leftMoved = leftIter.MoveNext()) & (rightMoved = rightIter.MoveNext()))
            {
                if (!comparer.Equals(leftIter.Current, rightIter.Current))
                    return false;
            }

            Debug.Assert(!(leftMoved & rightMoved));

            return !(leftMoved ^ rightMoved);
        }
    }
}
