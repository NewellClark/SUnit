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
        
        public int Count { get; private set; }

        public void Add(TKey key, TValue value) => throw new NotImplementedException();

        public bool Remove(TKey key) => throw new NotImplementedException();

        public bool Contains(TKey key) => throw new NotImplementedException();

        public IEnumerable<TValue> this[TKey key] => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
