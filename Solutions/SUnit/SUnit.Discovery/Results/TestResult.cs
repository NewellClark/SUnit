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
        /// The test did not run due to an exception, and we don't know whether the code under test or
        /// the test itself is to blame.
        /// </summary>
        Error,

        /// <summary>
        /// The test ran successfully, and returned a failing <see cref="Test"/>.
        /// </summary>
        Fail,

        /// <summary>
        /// The test ran successfully, and returned a passing <see cref="Test"/>.
        /// </summary>
        Pass
    }

    /// <summary>
    /// The result of running a test.
    /// </summary>
    internal abstract class TestResult
    {
        protected private TestResult(UnitTest unitTest, ResultKind kind)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));

            this.UnitTest = unitTest;
            this.Kind = kind;
        }

        public UnitTest UnitTest { get; }
        public string Name => UnitTest.Name;

        /// <summary>
        /// Whether the result was a pass, fail, or error.
        /// </summary>
        public ResultKind Kind { get; }
    }
}
