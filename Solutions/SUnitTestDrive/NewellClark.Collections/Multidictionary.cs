using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace NewellClark.Collections
{
    /// <summary>
    /// Maps a single key to multiple values. Duplicates are allowed.
    /// </summary>
    /// <typeparam name="TKey">The key type. </typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class Multidictionary<TKey, TValue> 
        : IEnumerable<Multidictionary<TKey, TValue>.Grouping>
    {
        private readonly Dictionary<TKey, List<TValue>> dictionary;

        /// <summary>
        /// Creates a new <see cref="Multidictionary{TKey, TValue}"/> that uses the specified
        /// <see cref="IEqualityComparer{T}"/> to compare keys for equality.
        /// </summary>
        /// <param name="keyComparer">The <see cref="IEqualityComparer{T}"/> to use for containment checks of keys.</param>
        public Multidictionary(IEqualityComparer<TKey> keyComparer)
        {
            if (keyComparer is null) throw new ArgumentNullException(nameof(keyComparer));

            dictionary = new Dictionary<TKey, List<TValue>>(keyComparer);
            Keys = new KeyCollection(this);
        }

        /// <summary>
        /// Creates a new <see cref="Multidictionary{TKey, TValue}"/>.
        /// </summary>
        public Multidictionary() 
            : this(EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Gets the number of entries in the <see cref="Multidictionary{TKey, TValue}"/>.
        /// </summary>
        public int Count => dictionary.Count;

        /// <summary>
        /// Adds the specified value to the group with the specified key. If the key does not exist,
        /// a new group is created. 
        /// </summary>
        /// <param name="key">The key of the group to add the value to.</param>
        /// <param name="value">The value to add.</param>
        public void Add(TKey key, TValue value)
        {
            if (!dictionary.TryGetValue(key, out List<TValue> list))
                dictionary.Add(key, list = new List<TValue>());
            list.Add(value);
        }

        /// <summary>
        /// Adds all the values in the collection to the gorup with the specified key.
        /// </summary>
        /// <param name="key">The key of the group to add to.</param>
        /// <param name="values">The values to add.</param>
        public void AddRange(TKey key, IEnumerable<TValue> values)
        {
            if (values is null) throw new ArgumentNullException(nameof(values));

            if (!dictionary.TryGetValue(key, out List<TValue> list))
                dictionary.Add(key, list = new List<TValue>());

            list.AddRange(values);
        }

        /// <summary>
        /// Removes the entry with the specified key, if it exists.
        /// </summary>
        /// <param name="key">The key of the entry to remove.</param>
        /// <returns>True if the key was found and removed, false otherwise.</returns>
        public bool Remove(TKey key)
        {
            bool removed = dictionary.Remove(key);

            return removed;
        }

        /// <summary>
        /// Removes the specified value from the group with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to remove.</param>
        /// <returns>True if the value was successfully removed.</returns>
        public bool Remove(TKey key, TValue value)
        {
            if (!dictionary.TryGetValue(key, out List<TValue> list))
                return false;

            bool wasRemoved = list.Remove(value);
            if (list.Count == 0)
                dictionary.Remove(key);

            return wasRemoved;
        }

        /// <summary>
        /// Indicates whether there is at least one value in the <see cref="Multidictionary{TKey, TValue}"/> 
        /// associated with the specified key.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>True if the <see cref="Multidictionary{TKey, TValue}"/> contains the specified key.</returns>
        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        /// <summary>
        /// Gets all the values associated with the specified key. If the key is not in the 
        /// <see cref="Multidictionary{TKey, TValue}"/>, an empty group is returned.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>A group containing all the values associated with the key, or an empty group
        /// if the key is not in the <see cref="Multidictionary{TKey, TValue}"/>.</returns>
        public Grouping this[TKey key]
        {
            get => dictionary.ContainsKey(key) ?
                new Grouping(key, dictionary[key]) :
                default;
        }

        /// <summary>
        /// Gets a collection of all the keys in the <see cref="Multidictionary{TKey, TValue}"/>.
        /// </summary>
        public KeyCollection Keys { get; }

        /// <summary>
        /// Gets an <see cref="IEnumerator{T}"/> that enumerates all the key-value groups in the 
        /// <see cref="Multidictionary{TKey, TValue}"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Grouping> GetEnumerator()
        {
            foreach (var kvp in dictionary)
                yield return new Grouping(kvp.Key, kvp.Value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        /// <summary>
        /// A read-only collection of all the keys in a <see cref="Multidictionary{TKey, TValue}"/>.
        /// </summary>
        public sealed class KeyCollection : IReadOnlyCollection<TKey>
        {
            private readonly Multidictionary<TKey, TValue> owner;

            /// <summary>
            /// Creates a new <see cref="KeyCollection"/> for the specified <see cref="Multidictionary{TKey, TValue}"/>.
            /// </summary>
            /// <param name="owner">The <see cref="Multidictionary{TKey, TValue}"/> that owns the 
            /// newly-created <see cref="KeyCollection"/>.</param>
            internal KeyCollection(Multidictionary<TKey, TValue> owner)
            {
                if (owner is null) throw new ArgumentNullException(nameof(owner));

                this.owner = owner;
            }

            /// <summary>
            /// Gets the number of keys in the <see cref="KeyCollection"/>.
            /// </summary>
            public int Count => owner.Count;

            /// <summary>
            /// Gets an <see cref="IEnumerator{T}"/> that enumerates the <see cref="KeyCollection"/>.
            /// </summary>
            /// <returns>An <see cref="IEnumerator{T}"/> for the current <see cref="KeyCollection"/>.</returns>
            public IEnumerator<TKey> GetEnumerator()
            {
                foreach (var entry in owner)
                    yield return entry.Key;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// A group containing all the values associated with a particular key in a <see cref="Multidictionary{TKey, TValue}"/>.
        /// </summary>
        public struct Grouping : IGrouping<TKey, TValue>, IReadOnlyCollection<TValue>
        {
            private readonly IReadOnlyCollection<TValue> items;

            /// <summary>
            /// Creates an empty <see cref="Grouping"/> for the specified key.
            /// </summary>
            /// <param name="key">The key of the <see cref="Grouping"/>.</param>
            internal Grouping(TKey key)
            {
                this.Key = key;
                this.items = null;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <param name="items"></param>
            internal Grouping(TKey key, IReadOnlyCollection<TValue> items)
            {
                Debug.Assert(items != null);

                this.Key = key;
                this.items = items;
            }

            /// <summary>
            /// Gets the number values in the <see cref="Grouping"/>.
            /// </summary>
            public int Count => items?.Count ?? 0;

            /// <summary>
            /// Gets the key associated with the <see cref="Grouping"/>.
            /// </summary>
            public TKey Key { get; }

            /// <summary>
            /// Gets an enumerator for the <see cref="Grouping"/>.
            /// </summary>
            /// <returns></returns>
            public IEnumerator<TValue> GetEnumerator()
            {
                return items?.GetEnumerator() ??
                    Enumerable.Empty<TValue>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
