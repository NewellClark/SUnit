using SUnit.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal class ProperSupersetOfConstraint<T> : IConstraint<IEnumerable<T>>
    {
        private readonly IEnumerable<T> expected;
        private readonly IEqualityComparer<T> comparer;

        public ProperSupersetOfConstraint(IEnumerable<T> expected)
            : this(expected, EqualityComparer<T>.Default) { }

        public ProperSupersetOfConstraint(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            this.expected = expected;
            this.comparer = comparer;
        }

        public bool Apply(IEnumerable<T> actual)
        {
            var actualSet = new CountedSet<T>(actual, comparer);
            var expectedSet = new CountedSet<T>(expected, comparer);

            if (actualSet.TotalCount <= expectedSet.TotalCount)
                return false;

            foreach (var kvp in expectedSet)
            {
                int expectedCount = kvp.Value;
                int actualCount = actualSet[kvp.Key];

                if (expectedCount > actualCount)
                    return false;
            }

            return true;
        }
    }
}
