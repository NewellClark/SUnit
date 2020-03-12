using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUnit.DiscoveryOLD.Results
{
    /// <summary>
    /// The <see cref="TestResult"/> that is returned when a test runs to completion successfully.
    /// </summary>
    internal class RanSuccessfullyResult : TestResult
    {
        private const string indent = "   ";

        public RanSuccessfullyResult(string name, Test result) 
            : base(name, result.Passed ? ResultKind.Pass : ResultKind.Fail)
        {
            this.Result = result;
        }

        /// <summary>
        /// The <see cref="Test"/> returned by the test method.
        /// </summary>
        public Test Result { get; }

        public override string ToString()
        {
            return GetFailedDisplayString();
        }

        private string GetFailedDisplayString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Name);

            var lines = Result.ToString().Split("\n")
                .Select(line => $"{indent}{line}");

            foreach (var line in lines)
                sb.AppendLine(line);

            return sb.ToString().TrimEnd();
        }
    }
}
