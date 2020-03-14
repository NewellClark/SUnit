using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using nAssert = NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    partial class UnitTestTests
    {
        [TestFixture]
        public class SingleDefaultCtor : UnitTestTests
        {
            private class Mock
            {
                public Mock() { }

                [Pass]
                public Test Null_IsNull() => Assert.That((object)null).Is.Null;
                
                [Fail]
                public Test Null_IsNotNull() => Assert.That((object)null).Is.Not.Null;
            }

            private protected override Type FixtureType => typeof(Mock);
        }
    }
}
