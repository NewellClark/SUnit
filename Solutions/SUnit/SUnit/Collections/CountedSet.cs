using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq.Extensions;

namespace SUnit.Collections
{
    internal class CountedSet<T> : IEnumerable<KeyValuePair<T, int>>
    {
        private readonly Dictionary<Box, int> countLookup;
        private readonly BoxComparer comparer;

        public CountedSet(IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            this.comparer = new BoxComparer(comparer);

            var countedItems = items.CountBy(t => new Box(t), this.comparer);
            TotalCount = countedItems.Aggregate(0, (sum, kvp) => sum + kvp.Value);
            countLookup = countedItems.ToDictionary(this.comparer);
        }

        public int this[T item]
        {
            get
            {
                if (!countLookup.TryGetValue(new Box(item), out int count))
                    return 0;

                return count;
            }
        }

        public int TotalCount { get; }

        public int UniqueCount => countLookup.Count;

        public bool Contains(T item)
        {
            return countLookup.ContainsKey(new Box(item));
        }

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
        {
            foreach (var kvp in countLookup)
            {
                yield return new KeyValuePair<T, int>(kvp.Key.Item, kvp.Value);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();


        private struct Box
        {
            public Box(T item)
            {
                this.Item = item;
            }

            public T Item { get; }
        }

        private class BoxComparer : IEqualityComparer<Box>
        {
            private readonly IEqualityComparer<T> itemComparer;

            public BoxComparer(IEqualityComparer<T> itemComparer)
            {
                if (itemComparer is null) throw new ArgumentNullException(nameof(itemComparer));

                this.itemComparer = itemComparer;
            }

            public bool Equals(Box x, Box y)
            {
                return itemComparer.Equals(x.Item, y.Item);
            }

            public int GetHashCode(Box obj)
            {
                if (obj.Item is null)
                    return 0;

                return itemComparer.GetHashCode(obj.Item);
            }
        }

    }
}
