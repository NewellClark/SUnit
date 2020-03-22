using SUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleConsumer
{
    public class MyTestFixture
    {
        public Test MyTestMethod()
        {
            return Assert.That(2 + 2).Is.EqualTo(4);
        }
    }
}
