using SUnit.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal class SubsetOfConstraint<T> : IConstraint<IEnumerable<T>>
    {
        private readonly IEnumerable<T> expected;
        private readonly IEqualityComparer<T> comparer;

        public SubsetOfConstraint(IEnumerable<T> expected)
            : this(expected, EqualityComparer<T>.Default) { }

        public SubsetOfConstraint(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            this.expected = expected;
            this.comparer = comparer;
        }

        public bool Apply(IEnumerable<T> actual)
        {
            var expectedSet = new CountedSet<T>(expected, comparer);
            var actualSet = new CountedSet<T>(actual, comparer);

            if (actualSet.TotalCount > expectedSet.TotalCount)
                return false;

            foreach (var actualKvp in actualSet)
            {
                int actualCount = actualKvp.Value;
                int expectedCount = expectedSet[actualKvp.Key];

                if (actualCount > expectedCount)
                    return false;
            }

            return true;
        }
    }
}
