using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace NewellClark.Collections
{
    public class Multidictionary<TKey, TValue> 
        : IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>>
    {
        private readonly Dictionary<TKey, List<TValue>> dictionary;

        public Multidictionary(IEqualityComparer<TKey> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            dictionary = new Dictionary<TKey, List<TValue>>(comparer);
        }
        public Multidictionary() : this(EqualityComparer<TKey>.Default) { }

        public int Count { get; private set; }

        public void Add(TKey key, TValue value) => throw new NotImplementedException();

        public bool Remove(TKey key) => throw new NotImplementedException();

        public bool Contains(TKey key) => throw new NotImplementedException();

        public IEnumerable<TValue> this[TKey key] => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>> GetEnumerator()
        {
            foreach (var kvp in dictionary)
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(kvp.Key, kvp.Value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
