using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    /// <summary>
    /// A constraint that compares two sequences for set-equality. Order doesn't matter, and duplicates are ignored.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SetEqualityConstraint<T> : IConstraint<IEnumerable<T>>  
    {
        private readonly HashSet<T> expected;
        public SetEqualityConstraint(IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            if (expected is null)
                return;
            this.expected = new HashSet<T>(expected, comparer);
        }

        public bool Apply(IEnumerable<T> value)
        {
            return Constraint.NullFriendlyEquality(expected, value, (left, right) => left.SetEquals(right));
        }
    }
}
