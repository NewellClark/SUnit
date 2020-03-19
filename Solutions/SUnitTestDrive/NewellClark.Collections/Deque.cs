using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NewellClark.Collections
{
    public class Deque<T> : IEnumerable<T>
    {

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
