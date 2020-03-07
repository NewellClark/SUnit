using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    internal delegate IsTest<TActual> TestBuilder<TActual>(IConstraint<TActual> constraint);

    public class Is<TActual>
    {
        private readonly TestBuilder<TActual> builder;

        internal Is(TActual actual)
        {
            builder = constraint => new IsTest<TActual>(actual, constraint);
        }
        
        internal Is(TestBuilder<TActual> builder)
        {
            Debug.Assert(builder != null);

            this.builder = builder;
        }
        internal Is(TActual actual, Func<IConstraint<TActual>, IConstraint<TActual>> constraintModifier)
        {
            Debug.Assert(constraintModifier != null);

            this.builder = constraint => new IsTest<TActual>(actual, constraintModifier(constraint));
        }

        internal IsTest<TActual> ApplyConstraint(IConstraint<TActual> constraint)
        {
            Debug.Assert(constraint != null);

            return builder(constraint);
        }

        /// <summary>
        /// Inverts any <see cref="Test"/> that is applied to the current actual value.
        /// </summary>
        public Is<TActual> Not => new Is<TActual>(constraint => ApplyConstraint(!constraint));

        /// <summary>
        /// Tests that the expression is equal to the specified actual value.
        /// </summary>
        /// <param name="expected">The</param>
        /// <returns></returns>
        public IsTest<TActual> EqualTo(TActual expected)
        {
            return ApplyConstraint(new EqualToConstraint<TActual>(expected));
        }

        public IsTest<TActual> Null => ApplyConstraint(new NullConstraint<TActual>());
    }

    public static class IsExtensions
    {
        public static IsTest<T> LessThan<T>(this Is<T> @this, T expected) 
            where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new LessThanConstraint<T>(expected));
        }

        public static IsTest<T> GreaterThan<T>(this Is<T> @this, T expected)
            where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.ApplyConstraint(new GreaterThanConstraint<T>(expected));
        }

        public static IsTest<T> LessThanOrEqualTo<T>(this Is<T> @this, T expected)
            where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.GreaterThan(expected);
        }

        public static IsTest<T> GreaterThanOrEqualTo<T>(this Is<T> @this, T expected)
            where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.LessThan(expected);
        }
    }
}
