using System;
using System.Collections.Generic;
using System.Text;
using SUnit;

namespace CreatedFromTemplate
{
    public class SUnitTestTemplate
    {
        public Test WrittenTest()
        {
            return Assert.That(2 + 2).Is.EqualTo(4);
        }
    }
}
