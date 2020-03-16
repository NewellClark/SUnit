using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using nAssert = NUnit.Framework.Assert;

namespace SUnit.Assertions
{
    [TestFixture]
    public class AssertThrowsTests
    {
        [Test]
        public void NoException_Fails()
        {
            Test test = Assert.Throws<Exception>(() => { });
            nAssert.That(test.Passed, Is.False);
        }

        [Test]
        public void CorrectException_Passes()
        {
            Test test = Assert.Throws<ArgumentNullException>(() => throw new ArgumentNullException());
            nAssert.That(test.Passed, Is.True);
        }

        [Test]
        public void WrongException_Fails()
        {
            Test test = Assert.Throws<ArgumentNullException>(() => throw new NullReferenceException());
            nAssert.That(test.Passed, Is.False);
        }

        [Test]
        public void MoreDerivedException_Fails()
        {
            Test test = Assert.Throws<ArgumentException>(() => throw new ArgumentOutOfRangeException());
        }
    }
}
