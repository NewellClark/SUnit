using NUnit.Framework;
using static SUnit.Helpers;

using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    [TestFixture]
    public class IsExpressionLongTests
    {
        [Test]
        public void Zero_IsZero()
        {
            AssertPassed(Assert.That(0L).Is.Zero);
        }

        [Test]
        public void Zero_IsNotPositive()
        {
            AssertFailed(Assert.That(0L).Is.Positive);
        }

        [Test]
        public void Zero_IsNotNegative()
        {
            AssertFailed(Assert.That(0L).Is.Negative);
        }

        [Test]
        public void Positive_IsPositive()
        {
            AssertPassed(Assert.That(79).Is.Positive);
            AssertFailed(Assert.That(69).Is.Not.Positive);
        }

        [Test]
        public void Positive_IsNotZero()
        {
            AssertPassed(Assert.That(1).Is.Not.Zero);
        }

        [Test]
        public void Negative_IsNotPositive()
        {
            AssertFailed(Assert.That(-123).Is.Positive);
            AssertPassed(Assert.That(-123).Is.Not.Positive);
        }
    }
}
