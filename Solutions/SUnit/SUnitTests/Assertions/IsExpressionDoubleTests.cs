using NUnit.Framework;
using static SUnit.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    [TestFixture]
    public class IsExpressionDoubleTests
    {
        [Test]
        public void Zero_IsNotPositive()
        {
            AssertFailed(Assert.That(0.0).Is.Positive);
            AssertPassed(Assert.That(0.0).Is.Not.Positive);
        }

        [Test]
        public void Zero_IsNotNegative()
        {
            AssertPassed(Assert.That(0.0).Is.Not.Negative);
            AssertFailed(Assert.That(0.0).Is.Negative);
        }

        [Test]
        public void Zero_IsZero()
        {
            AssertPassed(Assert.That(0.0).Is.Zero);
            AssertFailed(Assert.That(0.0).Is.Not.Zero);
        }

        [Test]
        public void Epsilon_IsPositive()
        {
            AssertPassed(Assert.That(double.Epsilon).Is.Positive);
        }

        [Test]
        public void Epsilon_IsNotNegative()
        {
            AssertFailed(Assert.That(double.Epsilon).Is.Negative);
            AssertPassed(Assert.That(double.Epsilon).Is.Not.Negative);
        }

        [Test]
        public void Epsilon_IsNotZero()
        {
            AssertPassed(Assert.That(double.Epsilon).Is.Not.Zero);
        }

        [Test]
        public void NegativeEpsilon_IsNotPositive()
        {
            AssertFailed(Assert.That(-double.Epsilon).Is.Positive);
        }

        [Test]
        public void PositiveInfinity_IsPositive()
        {
            AssertPassed(Assert.That(double.PositiveInfinity).Is.Positive);
            AssertFailed(Assert.That(double.PositiveInfinity).Is.Not.Positive);
        }

        [Test]
        public void PositiveInfinity_IsNotNegative()
        {
            AssertFailed(Assert.That(double.PositiveInfinity).Is.Negative);
        }

        [Test]
        public void NegativeInfinity_IsNegative()
        {
            AssertPassed(Assert.That(double.NegativeInfinity).Is.Negative);
        }

        [Test]
        public void NegativeInfinity_IsNotPositive()
        {
            AssertFailed(Assert.That(double.NegativeInfinity).Is.Positive);
        }

        [Test]
        public void NaN_IsNotPositive()
        {
            AssertFailed(Assert.That(double.NaN).Is.Positive);
            AssertPassed(Assert.That(double.NaN).Is.Not.Positive);
        }

        private readonly double? nothing = null;

        [Test]
        public void Null_IsNotPositive()
        {
            AssertPassed(Assert.That(nothing).Is.Not.Positive);
        }

        [Test]
        public void Null_IsNotNegative()
        {
            AssertFailed(Assert.That(nothing).Is.Negative);
        }

        [Test]
        public void Null_IsNotNaN()
        {
            AssertFailed(Assert.That(nothing).Is.NaN);
        }

        [Test]
        public void FailOrPass_Passes()
        {
            AssertPassed(Assert.That(double.NaN).Is.Negative.Or.NaN);
        }

        [Test]
        public void PassOrFail_Passes()
        {
            AssertPassed(Assert.That(double.PositiveInfinity).Is.Positive.Or.Negative);
        }

        [Test]
        public void PassAndFail_Fails()
        {
            AssertFailed(Assert.That(double.NegativeInfinity).Is.Positive.And.Zero);
        }

        [Test]
        public void ManyChainedAndsAllPass_Passes()
        {
            Assert.That(7.2).Is.LessThan(7.3)
                .And.LessThan(7.4)
                .And.LessThan(7.5)
                .And.LessThan(7.6);
        }
    }
}
