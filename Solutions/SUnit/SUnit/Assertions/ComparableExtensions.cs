using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;
using SUnit.Assertions;

namespace SUnit
{
    /// <summary>
    /// Contains extension methods for objects that implement <see cref="IComparable{T}"/>.
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// Tests whether the actual value is less than the expected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we expect to be less than.</param>
        /// <returns></returns>
        public static TTest LessThan<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ValueTest<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new LessThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is less than the expected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we expect to be less than.</param>
        /// <returns></returns>
        public static TTest LessThan<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ValueTest<T?>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableLessThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is greater than the expected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we expect to be greater than.</param>
        /// <returns></returns>
        public static TTest GreaterThan<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ValueTest<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new GreaterThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is greater than the expected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we expect to be greater than.</param>
        /// <returns></returns>
        public static TTest GreaterThan<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ValueTest<T?>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableGreaterThanConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is less than or equal to the expected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we expect to be less than or equal to.</param>
        /// <returns></returns>
        public static TTest LessThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ValueTest<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.GreaterThan(expected);
        }

        /// <summary>
        /// Tests whether the actual value is less than or equal to the expected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we expect to be less than or equal to.</param>
        /// <returns></returns>
        public static TTest LessThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ValueTest<T?>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableLessThanOrEqualToConstraint<T>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is greater than or equal to the expected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we expect to be greater than or equal to.</param>
        /// <returns></returns>
        public static TTest GreaterThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ValueTest<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.LessThan(expected);
        }

        /// <summary>
        /// Tests whether the actual value is greater than or equal to the expected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIs"></typeparam>
        /// <typeparam name="TTest"></typeparam>
        /// <param name="this"></param>
        /// <param name="expected">The value we expect to be greater than or equal to.</param>
        /// <returns></returns>
        public static TTest GreaterThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ValueTest<T?>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableGreaterThanOrEqualToConstraint<T>(expected));
        }
    }
}
