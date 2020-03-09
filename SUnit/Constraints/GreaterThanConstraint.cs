using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal sealed class GreaterThanConstraint<T> : IConstraint<T> 
        where T : IComparable<T>
    {
        private readonly T expected;

        public GreaterThanConstraint(T expected) => this.expected = expected;

        public bool Apply(T actual) => Comparer<T>.Default.Compare(actual, expected) > 0;

        public override string ToString() => $"> {expected}";
    }

    internal sealed class NullableGreaterThanConstraint<T> : IConstraint<T?> 
        where T : struct, IComparable<T>
    {
        private readonly T? expected;

        public NullableGreaterThanConstraint(T? expected) => this.expected = expected;

        public bool Apply(T? actual) => Comparer<T?>.Default.Compare(actual, expected) > 0;
    }

    internal sealed class NullableGreaterThanOrEqualToConstraint<T> : IConstraint<T?>
        where T : struct, IComparable<T>
    {
        private readonly T? expected;

        public NullableGreaterThanOrEqualToConstraint(T? expected) => this.expected = expected;

        public bool Apply(T? actual) => Comparer<T?>.Default.Compare(actual, expected) >= 0;
    }
}
