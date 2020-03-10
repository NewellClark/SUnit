using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Discovery.Results
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
        protected private TestResult(UnitTest unitTest, ResultKind kind)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));

            this.UnitTest = unitTest;
            this.Kind = kind;
        }
        
        /// <summary>
        /// The <see cref="SUnit.Discovery.UnitTest"/> that ran. 
        /// </summary>
        public UnitTest UnitTest { get; }

        /// <summary>
        /// Whether the result was a pass, fail, or error.
        /// </summary>
        public ResultKind Kind { get; }
    }
}
