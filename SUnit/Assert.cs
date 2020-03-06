using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// Contains methods for performing assertions.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Just like every other unit test framework. You say <code>Assert.That(actual).Is.EqualTo(expected);</code>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static That<T> That<T>(T actual) => new That<T>(actual);

        public static ThatBool That(bool actual) => new ThatBool(actual);
    }
}
