using System;
using System.Collections.Generic;
using System.Text;
using SUnit.Assertions;

namespace SUnit
{
    public abstract class BaseAssert
    {
        protected private BaseAssert() { }

        public static That<T> That<T>(T actual) => new That<T>(actual);
    }

    public class Assert : BaseAssert
    {
        private Assert() : base() { }

        public static DoubleThat That(double? actual) => new DoubleThat(actual);

        public static LongThat That(long? actual) => new LongThat(actual);

        public static DecimalThat That(decimal? actual) => new DecimalThat(actual);

        public static BoolThat That(bool? actual) => new BoolThat(actual);

        public static EnumerableThat<T> That<T>(IEnumerable<T> actual)
        {
            return new EnumerableThat<T>(actual);
        }


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
        

        public static Test Throws<TException>(Action codeThatShouldThrow)
            where TException : Exception
        {
            return Throws<TException>(codeThatShouldThrow, false);
        }
    }
}
