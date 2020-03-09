using NUnit.Framework;
using static SUnit.Helpers;
using assert = NUnit.Framework.Assert;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
