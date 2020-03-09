using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Constraints
{
    internal static class Constraint
    {
        

        /// <summary>
        /// Creates a new <see cref="IConstraint{T}"/> from the specified <see cref="Predicate{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of value that the constraint applies to.</typeparam>
        /// <param name="predicate">The predicate to apply to values that the constraint is applied to.</param>
        /// <returns>A new <see cref="IConstraint{T}"/> that uses the specified <see cref="Predicate{T}"/>.</returns>
        public static IConstraint<T> FromPredicate<T>(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return new PredicateConstraint<T>(predicate);
        }
    }
}
