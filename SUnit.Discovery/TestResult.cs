using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Discovery
{
    /// <summary>
    /// The kind of result.
    /// </summary>
    internal enum ResultKind
    {
        /// <summary>
        /// The test did not run due to an error.
        /// </summary>
        Error,

        /// <summary>
        /// The test ran, and it failed.
        /// </summary>
        Fail,

        /// <summary>
        /// The test ran, and it passed.
        /// </summary>
        Pass
    }

    /// <summary>
    /// The result of running a <see cref="UnitTest"/>.
    /// </summary>
    internal abstract class TestResult
    {
        protected private TestResult(ResultKind kind) => Kind = kind;

        /// <summary>
        /// Whether the result was a pass, fail, or error.
        /// </summary>
        public ResultKind Kind { get; }

        private sealed class UnexpectedExceptionResult : TestResult
        {
            private readonly Exception exception;
            private readonly UnitTest test;

            public UnexpectedExceptionResult(UnitTest test, Exception exception) : base(ResultKind.Error)
            {
                Debug.Assert(test != null);
                Debug.Assert(exception != null);

                this.exception = exception;
                this.test = test;
            }

            public override string ToString()
            {
                return $"{test.Name}\n   Unexpected {exception.GetType().Name}";
            }
        }

        /// <summary>
        /// Creates a <see cref="TestResult"/> for when an unexpected exception is thrown while running the test.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static TestResult UnexpectedException(UnitTest test, Exception exception) => new UnexpectedExceptionResult(test, exception);
    }
}
