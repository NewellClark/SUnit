using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    public class IsBase
    {        
        private readonly Func<Test, Test> modifier;
        protected private IsBase(Func<Test, Test> modifier)
        {
            this.modifier = modifier;
        }
        protected private IsBase() : this(NonInverted) { }

        private class EqualityTestClass<T> : Test
        {
            private readonly T expected;
            private readonly T actual;

            public EqualityTestClass(T expected, T actual)
            {
                this.expected = expected;
                this.actual = actual;
            }

            public override bool Passed => EqualityComparer<T>.Default.Equals(expected, actual);
        }

        /// <summary>
        /// Creates an equality test for the two specified arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected private Test EqualityTest<T>(T expected, T actual)
        {
            return modifier(new EqualityTestClass<T>(expected, actual));
        }
        
        protected private static Test NonInverted(Test inner) => inner;
        protected private static Test Inverted(Test inner) => inner.Inverted;

        internal Test Create(Test inner) => modifier(inner);
    }

    //  The only purpose of this class is to provide the type of the Is property 
    //  on the That class, as in Assert.That(actual).Is.EqualTo(expected);
    //  Since the constructor is internal, and there are no useful static methods on this class, 
    //  the fact that Visual Basic programmers can't speak the name of this class should
    //  not be an issue.
    //  Sure, NUnit had to make a class called "Iz", but the way they have things set up, you actually have 
    //  to say "Is" directly, rather than just saying "Is" as a property. 
    /// <summary>
    /// It's the return type of <code>Assert.That(whatever).Is</code>. It has methods like <c>EqualTo(...)</c>, 
    /// <c>Null</c>, etc. 
    /// </summary>
    /// <typeparam name="TActual"></typeparam>
#pragma warning disable CA1716 // Identifiers should not match keywords
    public class Is<TActual> : IsBase
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        protected private Is(TActual actual, Func<Test, Test> modifier) : base(modifier)
        {
            this.Actual = actual;
        }
        internal Is(TActual actual) : this(actual, NonInverted) { }

        internal TActual Actual { get; }

        /// <summary>
        /// Inverts the test.
        /// </summary>
        public Is<TActual> Not => new Is<TActual>(Actual, Inverted);

        /// <summary>
        /// Tests if the actual value is equal to the specified expected value.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public Test EqualTo(TActual expected) => EqualityTest(expected, Actual);

        /// <summary>
        /// Tests if the actual value is null.
        /// </summary>
        public Test Null => EqualityTest<object>(null, Actual);
    }

    /// <summary>
    /// Allows code like <code>Assert.That(boolean expression).Is.True;</code>.
    /// </summary>
    public class IsBool : Is<bool>
    {
        internal IsBool(bool actual) : base(actual) { }
        protected private IsBool(bool actual, Func<Test, Test> modifier) : base(actual, modifier) { }

        /// <summary>
        /// Inverts the test.
        /// </summary>
        public new IsBool Not => new IsBool(Actual, Inverted);

        /// <summary>
        /// Test if the actual value is <see langword="true"/>.
        /// </summary>
        public Test True => EqualityTest(true, Actual);

        /// <summary>
        /// Test if the actual value is <see langword="false"/>.
        /// </summary>
        public Test False => EqualityTest(false, Actual);
    }

    public static class IsExtensions
    {
        private class CompareTest<T> : Test
        {
            private readonly T actual;
            private readonly T expected;
            private readonly int sign;

            public CompareTest(T actual, T expected, int sign)
            {
                this.actual = actual;
                this.expected = expected;
                this.sign = sign;
            }

            public override bool Passed => Math.Sign(Comparer<T>.Default.Compare(actual, expected)) == Math.Sign(sign);
        }

        public static Test LessThan<T>(this Is<T> @this, T expected) where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Create(new CompareTest<T>(@this.Actual, expected, -1));
        }

        public static Test GreaterThan<T>(this Is<T> @this, T expected) where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Create(new CompareTest<T>(@this.Actual, expected, 1));
        }

        public static Test LessThanOrEqualTo<T>(this Is<T> @this, T expected) where T : IComparable<T>
        {
            return @this.Not.GreaterThan(expected);
        }

        public static Test GreaterThanOrEqualTo<T>(this Is<T> @this, T expected) where T : IComparable<T>
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            return @this.Not.LessThan(expected);
        }
    }
}
