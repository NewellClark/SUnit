using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// The only purpose of this class is to ensure that more-specific overloads of
    /// <see cref="That{T}(T)"/> are always prioritized.
    /// </summary>
    public abstract class BaseAssert
    {
        protected private BaseAssert() { }

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static That<T> That<T>(T actual) => new That<T>(actual);
    }

    /// <summary>
    /// Contains methods for performing assertions. Every unit test framework has a class like  this.
    /// </summary>
    public sealed class Assert : BaseAssert
    {
        private Assert() { }

        /// <summary>
        /// Used to specify the actual value when writing assertions for sequences.
        /// <code>Assert.That(errorCollection).Is.Empty;</code>
        /// </summary>
        /// <typeparam name="T">The type of element in the sequence.</typeparam>
        /// <param name="actual">The actual value that is under test.</param>
        /// <returns>An object that lets you say "Is" (unless you're using VB). </returns>
        public static ThatEnumerable<T> That<T>(IEnumerable<T> actual) => new ThatEnumerable<T>(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns>An object that lets you say "Is".</returns>
        public static ThatDouble That(double actual) => new ThatDouble(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatDouble That(double? actual) => new ThatDouble(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatDouble That(float actual) => new ThatDouble(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatDouble That(float? actual) => new ThatDouble(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatDecimal That(decimal actual) => new ThatDecimal(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatDecimal That(decimal? actual) => new ThatDecimal(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatLong That(long actual) => new ThatLong(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatLong That(long? actual) => new ThatLong(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatLong That(int actual) => new ThatLong(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatLong That(int? actual) => new ThatLong(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatLong That(short actual) => new ThatLong(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static ThatLong That(short? actual) => new ThatLong(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns>An object that lets you say "Is".</returns>
        public static ThatBool That(bool actual) => new ThatBool(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns>An object that lets you say "Is".</returns>
        public static ThatBool That(bool? actual) => new ThatBool(actual);
    }
}
