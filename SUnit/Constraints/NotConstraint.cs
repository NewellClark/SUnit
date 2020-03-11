using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal sealed class NotConstraint<T> : IConstraint<T>
    {
        private readonly IConstraint<T> inner;
        public NotConstraint(IConstraint<T> inner)
        {
            this.inner = inner;
        }

        public bool Apply(T value) => !inner.Apply(value);

        public override string ToString() => $"NOT {inner}";
    }
}
