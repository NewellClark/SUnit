using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal sealed class LessThanConstraint<T> : IConstraint<T> where T: IComparable<T> 
    {
        private readonly T expected;

        public LessThanConstraint(T expected) => this.expected = expected;

        public bool Apply(T actual) => Comparer<T>.Default.Compare(actual, expected) < 0;
    }
}
