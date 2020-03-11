using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// Contains methods for performing assertions. Every unit test framework has a class like  this.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <typeparam name="TActual"></typeparam>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static That<object> That(object actual) => new That<object>(actual);

        public static ThatEnumerable<T> That<T>(IEnumerable<T> actual) => new ThatEnumerable<T>(actual);

        /// <summary>
        /// Used to specify the actual value when writing assertions. For example,
        /// <code>Assert.That(2 + 2).Is.Not.EqualTo(5);</code>.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
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

        public static ThatBool That(bool actual) => new ThatBool(actual);

        public static ThatBool That(bool? actual) => new ThatBool(actual);
    }
}
