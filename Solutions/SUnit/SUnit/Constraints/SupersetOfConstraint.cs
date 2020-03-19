using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq.Extensions;
using SUnit.Collections;

namespace SUnit.Constraints
{
    internal class SupersetOfConstraint<T> : IConstraint<IEnumerable<T>>
    {
        private readonly IEnumerable<T> expected;
        private readonly IEqualityComparer<T> comparer;
        
        public SupersetOfConstraint(IEnumerable<T> expected)
            : this(expected, EqualityComparer<T>.Default) { }

        public SupersetOfConstraint(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            this.expected = expected;
            this.comparer = comparer;
        }

        public bool Apply(IEnumerable<T> actual)
        {
            var expectedSet = new CountedSet<T>(expected, comparer);
            var actualSet = new CountedSet<T>(actual, comparer);

            if (actualSet.TotalCount < expectedSet.TotalCount)
                return false;

            foreach (var expectedKvp in expectedSet)
            {
                int expectedCount = expectedKvp.Value;
                int actualCount = actualSet[expectedKvp.Key];

                if (expectedCount > actualCount)
                    return false;
            }

            return true;
        }
    }
}
