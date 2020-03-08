﻿using System;
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
    
    /// <summary>
    /// The result of running a <see cref="UnitTest"/>.
    /// </summary>
    public abstract class TestResult
    {
        protected private TestResult(ResultKind kind) => this.Kind = kind;

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

        public static TestResult UnexpectedException(UnitTest test, Exception exception) => new UnexpectedExceptionResult(test, exception);
    }
}
