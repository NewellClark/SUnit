using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Constraints
{
    internal class EquivalentToConstraint<T> : IConstraint<IEnumerable<T>> 
    {
        private readonly IEnumerable<T> expected;
        private readonly IEqualityComparer<T> comparer;

        internal EquivalentToConstraint(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            Debug.Assert(comparer != null);

            this.expected = expected;
            this.comparer = comparer;
        }

        public bool Apply(IEnumerable<T> actual)
        {
            return Constraint.NullFriendlyEquality(actual, expected, CompareNonNull);
        }

        private bool CompareNonNull(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Debug.Assert(actual != null);
            Debug.Assert(expected != null);

            var actualCount = new CountedSet(actual, comparer);
            var expectedCount = new CountedSet(expected, comparer);

            if (actualCount.Count != expectedCount.Count)
                return false;
            if (actualCount.NullCount != expectedCount.NullCount)
                return false;

            foreach (T item in actualCount)
            {
                if (actualCount.HowMany(item) != expectedCount.HowMany(item))
                    return false;
            }

            return true;
        }

        private class CountedSet : IEnumerable<T>
        {
            private readonly Dictionary<T, int> lookup;
            public int NullCount { get; private set; } = 0;

            public CountedSet(IEnumerable<T> items, IEqualityComparer<T> comparer)
            {
                Debug.Assert(comparer != null);

                lookup = new Dictionary<T, int>(comparer);

                foreach (T item in items)
                    Add(item);
            }

            public void Add(T item)
            {
                Count += 1;

                if (item == null)
                {
                    NullCount += 1;
                    return;
                }
                if (!lookup.ContainsKey(item))
                    lookup.Add(item, 1);
                else
                    lookup[item] = lookup[item] + 1;
            }

            public int HowMany(T item)
            {
                if (item == null)
                    return NullCount;
                return lookup.TryGetValue(item, out int count) ? count : 0;
            }

            public int Count { get; private set; }

            public IEnumerator<T> GetEnumerator()
            {
                foreach (T key in lookup.Keys)
                    yield return key;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
