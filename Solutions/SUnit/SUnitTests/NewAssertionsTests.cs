using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static SUnit.Helpers;
using Assert = SUnit.NewAssertions.Assert;

namespace SUnit
{
    [TestFixture]
    public class NewAssertionsTests
    {
        [Test]
        public void TheClassic()
        {
            AssertPassed(Assert.That(2 + 2).Is.EqualTo(4));

            AssertPassed(Assert.That(2 + 2).Is.Not.EqualTo(5));
        }

        [Test]
        public void TheClassicFail()
        {
            AssertFailed(Assert.That(2 + 2).Is.EqualTo(5));

            AssertFailed(Assert.That(2 + 2).Is.Not.EqualTo(4));
        }

        
    }
}
