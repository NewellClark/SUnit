using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.DiscoveryOLD.Results
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
    /// The result of running a test.
    /// </summary>
    internal abstract class TestResult
    {
        protected private TestResult(string name, ResultKind kind)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));

            this.Name = name;
            this.Kind = kind;
        }

        public string Name { get; }

        /// <summary>
        /// Whether the result was a pass, fail, or error.
        /// </summary>
        public ResultKind Kind { get; }
    }
}
