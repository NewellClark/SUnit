using System;
using System.Collections.Generic;
using System.Text;
using SUnit;

namespace AdapterDebugging
{
    public class SUnitTest1
    {
        public Test WrittenTest()
        {
            return Assert.That(2 + 2).Is.EqualTo(4)
                .And.EqualTo(5);
        }

        public Test HelloWorks() => Assert.That(91).Is.Negative.Or.Hello.Positive
            .Or.Goodbye.Negative;
    }
}
