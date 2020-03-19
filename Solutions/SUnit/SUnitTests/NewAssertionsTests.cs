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

            var s = Assert.That("hello, world").Is.LessThan("Hello, World!");
        }
    }
}
