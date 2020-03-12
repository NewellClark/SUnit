using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.DiscoveryOLD.Results
{
    internal sealed class UnexpectedExceptionResult : TestResult
    {
        public Exception Exception { get; }

        public UnexpectedExceptionResult(string name, Exception exception) : base(name, ResultKind.Error)
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
