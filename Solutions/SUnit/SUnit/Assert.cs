using System;
using System.Collections.Generic;
using System.Text;
using SUnit.Assertions;

namespace SUnit
{
    /// <summary>
    /// This class exists to get overload resolution to pick more specific types when possible.
    /// </summary>
    public abstract class BaseAssert
    {
        protected private BaseAssert() { }

        /// <summary>
        /// Specifies the actual value that is being tested.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual">The actual value that is being tested.</param>
        /// <returns></returns>
        public static That<T> That<T>(T actual) => new That<T>(actual);
    }

    /// <summary>
    /// Contains methods for performing assertions.
    /// </summary>
    public class Assert : BaseAssert
    {
        private Assert() : base() { }

        /// <summary>
        /// Specifies the actual value that is being tested.
        /// </summary>
        /// <param name="actual">The actual value that is being tested.</param>
        /// <returns></returns>
        public static DoubleThat That(double? actual) => new DoubleThat(actual);

        /// <summary>
        /// Specifies the actual value that is being tested.
        /// </summary>
        /// <param name="actual">The actual value that is being tested.</param>
        /// <returns></returns>
        public static LongThat That(long? actual) => new LongThat(actual);

        /// <summary>
        /// Specifies the actual value that is being tested.
        /// </summary>
        /// <param name="actual">The actual value that is being tested.</param>
        /// <returns></returns>
        public static DecimalThat That(decimal? actual) => new DecimalThat(actual);

        /// <summary>
        /// Specifies the actual value that is being tested.
        /// </summary>
        /// <param name="actual">The actual value that is being tested.</param>
        /// <returns></returns>
        public static BoolThat That(bool? actual) => new BoolThat(actual);

        /// <summary>
        /// Specifies the actual value that is being tested.
        /// </summary>
        /// <param name="actual">The actual value that is being tested.</param>
        /// <returns></returns>
        public static EnumerableThat<T> That<T>(IEnumerable<T> actual)
        {
            return new EnumerableThat<T>(actual);
        }


        /// <summary>
        /// Asserts that a delegate throws the specified exception.
        /// </summary>
        /// <typeparam name="TException">The type of exception that the action is expected to throw.</typeparam>
        /// <param name="codeThatShouldThrow">The delegate that should throw.</param>
        /// <param name="allowSubclasses">Whether to allow subclass exceptions to pass the test.</param>
        /// <returns></returns>
        public static Test Throws<TException>(Action codeThatShouldThrow, bool allowSubclasses)
            where TException : Exception
        {
            if (codeThatShouldThrow is null) throw new ArgumentNullException(nameof(codeThatShouldThrow));

            bool shouldCatch(Exception ex)
            {
                return allowSubclasses ?
                    typeof(TException).IsAssignableFrom(ex.GetType()) :
                    typeof(TException) == ex.GetType();
            }

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                codeThatShouldThrow();
            }
            catch (TException ex) when (shouldCatch(ex))
            {
                return Test.FromResult(true, $"Got expected {ex.GetType().Name}");
            }
            catch (Exception ex)
            {
                return Test.FromResult(false, $"Expected {typeof(TException).Name}\nBut caught {ex.GetType().Name}.");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return Test.FromResult(false, $"Expected {typeof(TException).Name}\nBut caught nothing.");
        }
        

        /// <summary>
        /// Asserts that a delegate throws a specific exception. Subclass exceptions are not allowed.
        /// </summary>
        /// <typeparam name="TException">The exact type of exception that is expected. Subclass exceptions
        /// will not match.</typeparam>
        /// <param name="codeThatShouldThrow">The delegate that is expected to throw.</param>
        /// <returns></returns>
        public static Test Throws<TException>(Action codeThatShouldThrow)
            where TException : Exception
        {
            return Throws<TException>(codeThatShouldThrow, false);
        }
    }
}
