using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Analyzers.TestConsumer
{
#if false
    public class MyViolatingTestFixture
    {
        public Test Violator()
        {
            Assert.That(2 + 2).Is.EqualTo(5);

            return Assert.That(2 + 2).Is.EqualTo(5);
        }
    }
#endif
}
