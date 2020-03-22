using SUnit.Collections;
using System;
using System.Collections.Generic;

namespace SUnit.Constraints
{
    internal class ProperSubsetOfConstraint<T> : IConstraint<IEnumerable<T>>
    {
        private readonly IEnumerable<T> expected;
        private readonly IEqualityComparer<T> comparer;

        public ProperSubsetOfConstraint(IEnumerable<T> expected)
            : this(expected, EqualityComparer<T>.Default) { }

        public ProperSubsetOfConstraint(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            this.expected = expected;
            this.comparer = comparer;
        }

        public bool Apply(IEnumerable<T> actual)
        {
            var expectedSet = new CountedSet<T>(expected, comparer);
            var actualSet = new CountedSet<T>(actual, comparer);

            if (actualSet.TotalCount >= expectedSet.TotalCount)
                return false;

            foreach (var kvp in actualSet)
            {
                int actualCount = kvp.Value;
                int expectedCount = expectedSet[kvp.Key];

                if (actualCount > expectedCount)
                    return false;
            }

            return true;
        }
    }
}
