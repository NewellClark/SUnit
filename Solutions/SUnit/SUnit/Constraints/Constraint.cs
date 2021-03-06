﻿using SUnit.Assertions;
using System;
using System.Collections;
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

        /// <summary>
        /// Wraps a <see cref="Predicate{T}"/> in another predicate so that it treats null values as false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate to wrap.</param>
        /// <returns>A new predicate that calls the supplied predicate, but always returns false when given null values.</returns>
        public static Predicate<T> NullIsFalse<T>(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return value => ReferenceEquals(value, null) ? false : predicate(value);
        }

        public static Predicate<T> NullIsTrue<T>(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return value => ReferenceEquals(value, null) ? true : predicate(value);
        }

        /// <summary>
        /// Applies the specified equality function to the two operands ONLY if neither operand is null. If both operands are null,
        /// returns true. If one is null, returns false.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="equalityFunction">The equality function to apply if neither operand is null.</param>
        /// <returns>If both operands are null, returns true. If exactly one is null, returns false. Otherwise,
        /// uses the supplied equality function.</returns>
        public static bool NullFriendlyEquality<TLeft, TRight>(TLeft left, TRight right, Func<TLeft, TRight, bool> equalityFunction)
        {
            if (equalityFunction is null) throw new ArgumentNullException(nameof(equalityFunction));

            bool leftNull = ReferenceEquals(left, null);
            bool rightNull = ReferenceEquals(right, null);

            if (leftNull && rightNull)
                return true;
            if (leftNull ^ rightNull)
                return false;

            return equalityFunction(left, right);
        }

        /// <summary>
        /// Checks if an <see cref="IEnumerable"/> is actually a collection. If so, returns the count. Otherwise, 
        /// returns null.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns>The number of items in the collection if the sequence was actually a collection. Otherwise, null.</returns>
        public static int? TryGetCount(IEnumerable sequence)
        {
            if (sequence is null) throw new ArgumentNullException(nameof(sequence));

            if (sequence is ICollection collection)
                return collection.Count;

            return null;
        }

        /// <summary>
        /// Checks to see if the sequence is really a collection. If it is, returns the count. Otherwise, returns null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns>The number of items in the sequence if it was actually a collection. Otherwise, null.</returns>
        public static int? TryGetCount<T>(IEnumerable<T> sequence)
        {
            if (sequence is null) throw new ArgumentNullException(nameof(sequence));

            if (sequence is ICollection<T> collection)
                return collection.Count;
            if (sequence is IReadOnlyCollection<T> roc)
                return roc.Count;
            if (sequence is ICollection nonGeneric)
                return nonGeneric.Count;

            return null;
        }


    }
}
