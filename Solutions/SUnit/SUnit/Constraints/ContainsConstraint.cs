using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUnit.Constraints
{
    internal class ContainsConstraint<T> : IConstraint<IEnumerable<T>>
    {
        private readonly T expected;

        internal ContainsConstraint(T expected)
        {
            this.expected = expected;
        }

        public bool Apply(IEnumerable<T> value)
        {
            if (value is null)
                return false;

            return value.Contains(expected);
        }

        public override string ToString()
        {
            return $"item [{Utilities.DisplayValue(expected)}]";
        }
    }
}
