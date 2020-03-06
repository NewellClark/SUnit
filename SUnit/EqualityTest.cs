using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    internal sealed class EqualityTest<T> : Test 
    {
        private readonly T expected;
        private readonly T actual;

        public EqualityTest(T expected, T actual)
        {
            this.expected = expected;
            this.actual = actual;
        }

        public override bool Passed => EqualityComparer<T>.Default.Equals(expected, actual);
    }
}
