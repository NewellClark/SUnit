using NUnit.Framework;
using static SUnit.Helpers;
using assert = NUnit.Framework.Assert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SUnit.Assertions
{
    [TestFixture]
    public class IsComparableExtensionsTests
    {
        [TestFixture]
        public class NullStruct
        {
            private readonly int? none = null;

            [Test]
            public void IsNotLessThanAnything()
            {
                AssertFailed(Assert.That(none).Is.LessThan(int.MaxValue));
            }

            [Test]
            public void IsNotLessThanItself()
            {
                AssertFailed(Assert.That(none).Is.LessThan(none));
            }

            [Test]
            public void IsNotGreaterThanAnything()
            {
                AssertFailed(Assert.That(none).Is.GreaterThan(int.MinValue));
            }

            [Test]
            public void IsNotGreaterThanItself()
            {
                AssertFailed(Assert.That(none).Is.GreaterThan(none));
            }

            [Test]
            public void IsLessThanOrEqualToItself()
            {
                AssertPassed(Assert.That(none).Is.LessThanOrEqualTo(none));
            }

            [Test]
            public void IsGreaterThanOrEqualToItself()
            {
                AssertPassed(Assert.That(none).Is.GreaterThanOrEqualTo(none));
            }
        }

        [TestFixture]
        public class AnyNonNullValue
        {
            [DatapointSource]
            private IEnumerable<int> Values => Enumerable.Range(-2, 5);

            [Theory]
            public void IsNotLessThanItself(int value)
            {
                AssertPassed(Assert.That(value).Is.Not.LessThan(value));
            }

            [Theory]
            public void IsNotGreaterThanItself(int value)
            {
                AssertPassed(Assert.That(value).Is.Not.GreaterThan(value));
            }

            [Theory]
            public void IsNotLessThanValueBelowIt(int value)
            {
                AssertPassed(Assert.That(value).Is.Not.LessThan(value - 1));
            }

            [Theory]
            public void IsGreaterThanValueBelowIt(int value)
            {
                AssertPassed(Assert.That(value).Is.GreaterThan(value - 1));
            }

            [Theory]
            public void IsLessThanValueAboveIt(int value)
            {
                AssertPassed(Assert.That(value).Is.LessThan(value + 1));
            }

            [Theory]
            public void IsNotGreaterThanValueAboveIt(int value)
            {
                AssertPassed(Assert.That(value).Is.Not.GreaterThan(value + 1));
            }

            [Theory]
            public void IsLessThanOrEqualToItself(int value)
            {
                AssertPassed(Assert.That(value).Is.LessThanOrEqualTo(value));
            }

            [Theory]
            public void IsNotLessThanOrEqualToValueBelow(int value)
            {
                AssertPassed(Assert.That(value).Is.Not.LessThanOrEqualTo(value - 1));
            }

            [Theory]
            public void IsLessThanOrEqualToValueAbove(int value)
            {
                AssertPassed(Assert.That(value).Is.LessThanOrEqualTo(value + 1));
            }

            [Theory]
            public void IsGreaterThanOrEqualToItself(int value)
            {
                AssertPassed(Assert.That(value).Is.GreaterThanOrEqualTo(value));
            }

            [Theory]
            public void IsGreaterThanOrEqualToValueBelow(int value)
            {
                AssertPassed(Assert.That(value).Is.GreaterThanOrEqualTo(value - 1));
            }

            [Theory]
            public void IsNotGreaterThanOrEqualToValueAbove(int value)
            {
                AssertPassed(Assert.That(value).Is.Not.GreaterThanOrEqualTo(value + 1));
            }
        }
    }
}
