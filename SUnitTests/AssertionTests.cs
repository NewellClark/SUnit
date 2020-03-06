using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using assert = NUnit.Framework.Assert;

namespace SUnit
{
    [TestFixture]
    public class AssertionTests
    {
        [Test]
        public void EqualDoubles_Equal_Passes()
        {
            assert.That(Assert.That(0.0).Is.EqualTo(0.0).Passed, Is.True);
        }

        [Test]
        public void UnequalDoubles_Equal_Fails()
        {
            assert.That(Assert.That(0.0).Is.EqualTo(0.1).Passed, Is.False);
        }

        [Test]
        public void EqualDoubles_NotEqual_Fails()
        {
            assert.That(Assert.That(0.0).Is.Not.EqualTo(0.0).Passed, Is.False);
        }

        [Test]
        public void NullReference_IsNull()
        {
            object @null = null;
            assert.That(Assert.That(@null).Is.Null.Passed, Is.True);
        }

        [Test]
        public void NullNullableValue_IsNull()
        {
            double? @null = null;
            assert.That(Assert.That(@null).Is.Null.Passed, Is.True);
        }

        [Test]
        public void NonNullableValue_IsNotNull()
        {
            assert.That(Assert.That(17).Is.Null.Passed, Is.False);
        }

        [Test]
        public void NonNullReference_IsNotNull()
        {
            assert.That(Assert.That(new object()).Is.Null.Passed, Is.False);
        }

        [Test]
        public void True_IsTrue()
        {
            assert.That(Assert.That(true).Is.True.Passed, Is.True);
        }

        [Test]
        public void False_IsNotTrue()
        {
            assert.That(Assert.That(false).Is.True.Passed, Is.False);
        }

        [Test]
        public void True_IsNotFalse()
        {
            assert.That(Assert.That(true).Is.False.Passed, Is.False);
        }

        [Test]
        public void False_IsFalse()
        {
            assert.That(Assert.That(false).Is.False.Passed, Is.True);
        }
    }
}
