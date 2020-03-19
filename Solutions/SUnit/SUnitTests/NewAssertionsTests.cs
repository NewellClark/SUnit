using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static SUnit.Helpers;

namespace SUnit.NewAssertions
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

        [Test]
        public void DoublesWork()
        {
            AssertPassed(Assert.That(0.0).Is.Zero);

            AssertFailed(Assert.That(1.2).Is.Zero);

            AssertPassed(Assert.That((2.0 + 2.0)).Is.EqualTo(4.0));
        }

        [Test]
        public void CompareablesWork()
        {
            AssertPassed(Assert.That(7.0).Is.LessThan(8).And.Is.Not.Zero);

            var d = Assert.That(17.9).Is.LessThan(17.91).And.Is.Zero;
        }

        [Test]
        public void IntegersWork()
        {
            AssertPassed(Assert.That(17).Is.Not.Zero.And.Is.LessThan(18));

            AssertFailed(Assert.That(-1).Is.Positive.And.Is.Negative);
        }

        [Test]
        public void DecimalsWork()
        {
            AssertPassed(Assert.That(17.98970m).Is.EqualTo(1798.970m / 100m));

            AssertFailed(Assert.That(.0000000000001m).Is.Zero);
        }

        [Test]
        public void EnumerablesWork()
        {
            int[] nums = { 5, 19, 47, 19 };

            AssertPassed(Assert.That(nums).Is.SetEqualTo(19, 5, 47));

            AssertFailed(Assert.That(nums).Is.SequenceEqualTo(19, 5, 47));

            AssertPassed(Assert.That(nums).Is.EquivalentTo(19, 19, 5, 47));
        }
    }
}
