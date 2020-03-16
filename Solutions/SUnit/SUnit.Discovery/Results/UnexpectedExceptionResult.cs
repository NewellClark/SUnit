using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Discovery.Results
{
    internal sealed class UnexpectedExceptionResult : TestResult
    {
        public Exception Exception { get; }

        public UnexpectedExceptionResult(UnitTest unitTest, Exception exception) : base(unitTest, ResultKind.Error)
        {
            if (exception is null) throw new ArgumentNullException(nameof(exception));

            this.Exception = exception;
        }

        public override string ToString()
        {
            return $"Unexpected {Exception.GetType().Name}";
        }
    }

    internal sealed class InvalidTestResult : TestResult
    {
        public InvalidTestResult(UnitTest unitTest, string message)
            : base(unitTest, ResultKind.Error)
        {
            this.Message = message;
        }

        public string Message { get; }

        public override string ToString() => Message;
    }
}
