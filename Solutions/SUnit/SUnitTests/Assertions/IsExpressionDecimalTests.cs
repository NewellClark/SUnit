using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static SUnit.Helpers;

namespace SUnit.Assertions
{
    [TestFixture]
    public class IsExpressionDecimalTests
    {
        [Test]
        public void Zero_IsZero()
        {
            AssertPassed(Assert.That(decimal.Zero).Is.Zero);
        }

        [Test]
        public void Zero_IsNotPositive()
        {
            AssertFailed(Assert.That(decimal.Zero).Is.Positive);
        }

        [Test]
        public void Zero_IsNotNegative()
        {
            AssertFailed(Assert.That(decimal.Zero).Is.Negative);
        }

        [Test]
        public void Positive_IsNotNegative()
        {
            AssertFailed(Assert.That(decimal.One).Is.Negative);
        }

        [Test]
        public void Positive_IsPositive()
        {
            AssertPassed(Assert.That(decimal.One).Is.Positive);
        }

        [Test]
        public void NotPass_Fails()
        {
            AssertFailed(Assert.That(1m).Is.Not.LessThan(decimal.MaxValue));
        }

        [Test]
        public void NotFail_Passes()
        {
            AssertPassed(Assert.That(1m).Is.Not.Negative);
        }

        [Test]
        public void Or_Works()
        {
            AssertPassed(Assert.That(-1m).Is.Zero.Or.Is.EqualTo(7m).Or.Is.LessThan(decimal.MaxValue));
        }
    }
}
