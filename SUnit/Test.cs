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
        public abstract bool Passed { get; }

        /// <summary>
        /// Overridden to display the result.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Passed ? "Pass" : "Fail";

        private class ResultTest : Test
        {
            private readonly bool passed;

            public ResultTest(bool passed) => this.passed = passed;

            public override bool Passed => passed;
        }

        internal static Test Fail() => new ResultTest(false);

        internal static Test Pass() => new ResultTest(true);

        internal class InvertedTest : Test
        {
            protected private readonly Test inner;

            public InvertedTest(Test inner)
            {
                this.inner = inner;
            }

            public override bool Passed => !inner.Passed;
        }


        public Test Inverted => new InvertedTest(this);
    }
}
