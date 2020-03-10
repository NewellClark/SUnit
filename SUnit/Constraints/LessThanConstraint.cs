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

        public override string ToString() => $"<{expected}";
    }

    internal sealed class NullableLessThanConstraint<T> : IConstraint<T?>
        where T : struct, IComparable<T>
    {
        private readonly T? expected;

        public NullableLessThanConstraint(T? expected) => this.expected = expected;

        public bool Apply(T? actual)
        {
            if (actual is null)
                return false;
            return Comparer<T?>.Default.Compare(actual, expected) < 0;
        }

        public override string ToString() => $"<{expected}";
    }

    internal sealed class NullableLessThanOrEqualToConstraint<T> : IConstraint<T?>
        where T : struct, IComparable<T>
    {
        private readonly T? expected;

        public NullableLessThanOrEqualToConstraint(T? expected) => this.expected = expected;

        public bool Apply(T? actual) => Comparer<T?>.Default.Compare(actual, expected) <= 0;

        public override string ToString() => $"<={expected}";
    }
}
