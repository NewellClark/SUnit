using SUnit.Assertions;
using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// Extension methods for <see cref="IIsExpression{T, TIs, TTest}"/>.
    /// </summary>
    public static class IsComparableExtensions
    {
        public static TTest LessThan<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ActualValueTest<T, TIs, TTest>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new LessThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is less than the specified expected value. <see langword="null"/> is not 
        /// less than anything.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value that we should be less than.</param>
        /// <returns>A test that checks whether the actual value is less than <paramref name="expected"/>.</returns>
        public static TTest LessThan<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ActualValueTest<T?, TIs, TTest>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableLessThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is greater than the specified <paramref name="expected"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value that we should be greater than.</param>
        /// <returns>A test that tests whether the actual value is greater than the expected value.</returns>
        public static TTest GreaterThan<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ActualValueTest<T, TIs, TTest>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new GreaterThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is greater than the specified expected value. <see langword="null"/> is not
        /// greater than anything.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected"></param>
        /// <returns>A test that tests whether the actual value is greater than <paramref name="expected"/>.</returns>
        public static TTest GreaterThan<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ActualValueTest<T?, TIs, TTest>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableGreaterThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is less than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected"></param>
        /// <returns>A test that tests whether the actual value is less than or equal to the specified expected value.</returns>
        public static TTest LessThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ActualValueTest<T, TIs, TTest>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.GreaterThan(expected);
        }

        /// <summary>
        /// Tests whether the actual value is less than or equal to the specified expected value. <see langword="null"/> is 
        /// not less than anything and only equal to itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we should be less than or equal to.</param>
        /// <returns>A test that indicates if the actual value is less than or equal to the specified expected value.</returns>
        public static TTest LessThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ActualValueTest<T?, TIs, TTest>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableLessThanOrEqualToConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is greater than or equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static TTest GreaterThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ActualValueTest<T, TIs, TTest>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.LessThan(expected);
        }

        /// <summary>
        /// Tests whether the actual value is greater than or equal to the specified expected value. <see langword="null"/> is not
        /// greater than anything and is only equal to itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we should be greater than or equal to.</param>
        /// <returns>A test that indicates whether the actual value is greater than or equal to <paramref name="expected"/>.</returns>
        public static TTest GreaterThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ActualValueTest<T?, TIs, TTest>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableGreaterThanOrEqualToConstraint<T>(expected));
        }
    }
}
