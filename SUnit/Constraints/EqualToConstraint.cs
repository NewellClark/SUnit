using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    /// <summary>
    /// Constraint that tests whether a value is equal to an expected value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class EqualToConstraint<T> : IConstraint<T>
    {
        private readonly T expected;
        public EqualToConstraint(T expected)
        {
            this.expected = expected;
        }

        public bool Apply(T value)
        {
            return EqualityComparer<T>.Default.Equals(expected, value);
        }

        public override string ToString() => $"{expected}";
    }
}
