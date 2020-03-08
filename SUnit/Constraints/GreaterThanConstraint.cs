using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal sealed class GreaterThanConstraint<T> : IConstraint<T> where T : IComparable<T>
    {
        private readonly T expected;

        public GreaterThanConstraint(T expected) => this.expected = expected;

        public bool Apply(T actual) => Comparer<T>.Default.Compare(actual, expected) > 0;
    }
}
