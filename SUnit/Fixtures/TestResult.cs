using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Fixtures
{
    /// <summary>
    /// The kind of result.
    /// </summary>
    public enum ResultKind
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
    
    public abstract class TestResult
    {
        protected private TestResult(ResultKind kind) => this.Kind = kind;

        public ResultKind Kind { get; }

        private sealed class UnexpectedExceptionResult : TestResult
        {
            private readonly Exception exception;
            public UnexpectedExceptionResult(Exception exception) : base(ResultKind.Error)
            {
                Debug.Assert(exception != null);

                this.exception = exception;
            }

            public override string ToString()
            {
                return $"Unexpected {exception.GetType().Name}";
            }
        }

        public static TestResult UnexpectedException(Exception exception) => new UnexpectedExceptionResult(exception);
    }
}
