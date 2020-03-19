using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.NewAssertions
{
    public static class ComparableExtensions
    {
        public static TTest LessThan<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ValueTest<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new LessThanConstraint<T>(expected));
        }

        public static TTest LessThan<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ValueTest<T?>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableLessThanConstraint<T>(expected));
        }

        public static TTest GreaterThan<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ValueTest<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new GreaterThanConstraint<T>(expected));
        }

        public static TTest GreaterThan<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ValueTest<T?>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableGreaterThanConstraint<T>(expected));
        }

        public static TTest LessThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ValueTest<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.GreaterThan(expected);
        }

        public static TTest LessThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T?, TIs, TTest> @this, T? expected)
            where T : struct, IComparable<T>
            where TIs : IIsExpression<T?, TIs, TTest>
            where TTest : ValueTest<T?>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new NullableLessThanOrEqualToConstraint<T>(expected));
        }

        public static TTest GreaterThanOrEqualTo<T, TIs, TTest>(this IIsExpression<T, TIs, TTest> @this, T expected)
            where T : IComparable<T>
            where TIs : IIsExpression<T, TIs, TTest>
            where TTest : ValueTest<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.LessThan(expected);
        }

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
