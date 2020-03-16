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

        /// <summary>
        /// Tests that the specified <see cref="Action"/> throws the specified exception. Does not
        /// include subclasses.
        /// </summary>
        /// <typeparam name="TException">The type of exception that should be thrown.</typeparam>
        /// <param name="methodThatShouldThrow">An <see cref="Action"/> delegate that should throw.</param>
        /// <returns>A <see cref="Test"/> that passes if the specified <see cref="Action"/> threw 
        /// an exception of exactly the specified type (no subclasses). </returns>
        public static Test Throws<TException>(Action methodThatShouldThrow)
            where TException : Exception
        {
            if (methodThatShouldThrow is null)
                throw new ArgumentNullException(nameof(methodThatShouldThrow));

            string exceptionName = typeof(TException).Name;

            try
            {
                methodThatShouldThrow();
            }
            catch (TException)
            {
                string message = $"Got expected {exceptionName}.";
                return Test.FromResult(true, message);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)

            {
                string message = $"Expected {exceptionName}\n" +
                    $"But was {ex.GetType().Name}: {ex.Message}";
                return Test.FromResult(false, message);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return Test.FromResult(false, $"Expected {exceptionName}\nBut caught nothing.");
        }
    }
}
