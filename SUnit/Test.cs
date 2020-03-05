using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// Unit tests report their results by returning <see cref="Test"/> instances.
    /// </summary>
    public abstract class Test
    {
        protected private Test() { }

        /// <summary>
        /// Gets the result of the test.
        /// </summary>
        public abstract TestResult Result { get; }

        /// <summary>
        /// Overridden to display the result.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Result.ToString();

        private sealed class FailTest : Test
        {
            public override TestResult Result => TestResult.Fail;
        }

        /// <summary>
        /// Produces a failed <see cref="Test"/>.
        /// </summary>
        /// <returns>A <see cref="Test"/> with a <see cref="Result"/> of <see cref="TestResult.Fail"/>.</returns>
        internal static Test Fail() => new FailTest();
    }
}
