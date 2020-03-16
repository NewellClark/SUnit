using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// The return type of all unit tests in <see cref="SUnit"/>.
    /// </summary>
    public abstract partial class Test
    {
        /// <summary>
        /// Indicates whether the test passed.
        /// </summary>
        public abstract bool Passed { get; }

        /// <summary>
        /// Overridden to indicate test status.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Passed ? "PASS" : "FAIL";

        private sealed class CustomMessageTest : Test
        {
            private readonly string message;

            public CustomMessageTest(bool result, string message)
            {
                this.Passed = result;
                this.message = message;
            }

            public override bool Passed { get; }

            public override string ToString() => message;
        }

        internal static Test FromResult(bool result, string message)
        {
            return new CustomMessageTest(result, message);
        }
    }
}
