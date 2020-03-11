using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Constraints
{
    internal class PredicateConstraint<T> : IConstraint<T>
    {
        private readonly Predicate<T> predicate;

        public PredicateConstraint(Predicate<T> predicate)
        {
            Debug.Assert(predicate != null);

            this.predicate = predicate;
        }

        public bool Apply(T value) => predicate(value);

        public override string ToString() => $"A value satisfying a predicate";
    }
}
