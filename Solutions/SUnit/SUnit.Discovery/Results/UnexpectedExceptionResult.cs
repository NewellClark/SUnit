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
}
