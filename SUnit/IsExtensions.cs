using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// Extension methods for <see cref="IIsExpression{TActual}"/>.
    /// </summary>
    public static class IsExtensions
    {
        /// <summary>
        /// Tests whether the actual value is less than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value that we should be less than.</param>
        /// <returns></returns>
        public static IsTest<T> LessThan<T>(this IIsExpression<T> @this, T expected)
            where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new LessThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is greater than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value that we should be greater than.</param>
        /// <returns>A test that tests whether the actual value is greater than the expected value.</returns>
        public static IsTest<T> GreaterThan<T>(this IIsExpression<T> @this, T expected)
            where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new GreaterThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is less than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected"></param>
        /// <returns>A test that tests whether the actual value is less than or equal to the specified expected value.</returns>
        public static IsTest<T> LessThanOrEqualTo<T>(this IIsExpression<T> @this, T expected)
            where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.GreaterThan(expected);
        }

        /// <summary>
        /// Tests whether the actual value is greater than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static IsTest<T> GreaterThanOrEqualTo<T>(this IIsExpression<T> @this, T expected)
            where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.LessThan(expected);
        }
    }
}
