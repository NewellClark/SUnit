using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// Contains methods for performing assertions. Every unit test framework has a class like this.
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
        public static That<TActual> That<TActual>(TActual actual) => new That<TActual>(actual);

        public static ThatDouble That(double actual) => new ThatDouble(actual);
    }
}
