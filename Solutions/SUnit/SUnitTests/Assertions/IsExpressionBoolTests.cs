using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static SUnit.Helpers;

namespace SUnit.Assertions
{
    [TestFixture]
    public class IsExpressionBoolTests
    {
        private readonly bool? @null = null;

        [Test]
        public void True_IsTrue()
        {
            AssertPassed(Assert.That(true).Is.True);
        }

        [Test]
        public void True_IsNotFalse()
        {
            AssertPassed(Assert.That(true).Is.Not.False);
        }

        [Test]
        public void False_IsNotTrue()
        {
            AssertPassed(Assert.That(false).Is.Not.True);
        }

        [Test]
        public void False_IsFalse()
        {
            AssertPassed(Assert.That(false).Is.False);
        }

        [Test]
        public void Null_IsNotTrue()
        {
            AssertPassed(Assert.That(@null).Is.Not.True);
        }

        [Test]
        public void Null_IsNotFalse()
        {
            AssertPassed(Assert.That(@null).Is.Not.False);
        }
    }
}
