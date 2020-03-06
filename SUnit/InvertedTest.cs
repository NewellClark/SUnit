using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    internal class InvertedTest : Test
    {
        protected private readonly Test inner;

        public InvertedTest(Test inner)
        {
            Debug.Assert(inner != null);

            this.inner = inner;
        }

        public override bool Passed => !inner.Passed;
    }
}
