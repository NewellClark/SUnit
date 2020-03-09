using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Constraints
{
    internal static class Constraint
    {
        private sealed class PredicateConstraint<T> : IConstraint<T>
        {
            private readonly Predicate<T> predicate;

            public PredicateConstraint(Predicate<T> predicate)
            {
                Debug.Assert(predicate != null);

                this.predicate = predicate;
            }

            public bool Apply(T value) => predicate(value);
        }

        /// <summary>
        /// Creates a new <see cref="IConstraint{T}"/> from the specified <see cref="Predicate{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of value that the constraint applies to.</typeparam>
        /// <param name="predicate">The predicate to apply to values that the constraint is applied to.</param>
        /// <returns>A new <see cref="IConstraint{T}"/> that uses the specified <see cref="Predicate{T}"/>.</returns>
        public static IConstraint<T> Create<T>(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return new PredicateConstraint<T>(predicate);
        }
    }
}
