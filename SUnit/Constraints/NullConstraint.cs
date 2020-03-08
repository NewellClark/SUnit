using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal class NullConstraint<T> : IConstraint<T>
    {
        public bool Apply(T value) => ReferenceEquals(null, value);
    }
}
