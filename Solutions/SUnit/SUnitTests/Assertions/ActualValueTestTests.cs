using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using assert = NUnit.Framework.Assert;

namespace SUnit.Assertions
{
    [TestFixture]
    public class ActualValueTestTests
    {
        [Test]
        public void PassAndPass_Passes()
        {
            assert.That(Assert.That(5).Is.EqualTo(5).And.Is.EqualTo(5).Passed, Is.True);
        }

        [Test]
        public void PassAndFail_Fails()
        {
            assert.That(Assert.That(5).Is.EqualTo(5).And.Is.EqualTo(4).Passed, Is.False);
        }

        [Test]
        public void FailAndPass_Fails()
        {
            assert.That(Assert.That(5).Is.EqualTo(4).And.Is.EqualTo(5).Passed, Is.False);
        }

        [Test]
        public void FailAndFail_Fails()
        {
            assert.That(Assert.That(5).Is.EqualTo(4).And.Is.EqualTo(6).Passed, Is.False);
        }

        [Test]
        public void PassOrPass_Passes()
        {
            assert.That(Assert.That(9).Is.EqualTo(9).Or.Is.EqualTo(9).Passed, Is.True);
        }

        [Test]
        public void PassOrFail_Passes()
        {
            assert.That(Assert.That(9).Is.EqualTo(9).Or.Is.EqualTo(8).Passed, Is.True);
        }

        [Test]
        public void FailOrPass_Passes()
        {
            assert.That(Assert.That(9).Is.EqualTo(8).Or.Is.EqualTo(9).Passed, Is.True);
        }

        [Test]
        public void FailOrFail_Fails()
        {
            assert.That(Assert.That(9).Is.EqualTo(8).Or.Is.EqualTo(10).Passed, Is.False);
        }

        [Test]
        public void PassXorPass_Fails()
        {
            assert.That(Assert.That(7).Is.EqualTo(7).Xor.Is.EqualTo(7).Passed, Is.False);
        }

        [Test]
        public void PassXorFail_Passes()
        {
            assert.That(Assert.That(7).Is.EqualTo(7).Xor.Is.EqualTo(6).Passed, Is.True);
        }

        [Test]
        public void FailXorPass_Passes()
        {
            assert.That(Assert.That(7).Is.EqualTo(6).Xor.Is.EqualTo(7).Passed, Is.True);
        }

        [Test]
        public void FailXorFail_Fails()
        {
            assert.That(Assert.That(7).Is.EqualTo(6).Xor.Is.EqualTo(8).Passed, Is.False);
        }
    }
}
