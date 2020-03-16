using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SUnit.Helpers;

namespace SUnit.Assertions
{
    [TestFixture]
    public class IsExpressionEnumerableTests
    {
        [Test]
        public void AnEmptyArray_IsEmpty() => AssertPassed(Assert.That(Array.Empty<object>()).Is.Empty);

        [Test]
        public void NullSequence_IsNotEmpty()
        {
            AssertPassed(Assert.That((int[])null).Is.Not.Empty);
        }

        [Test]
        public void EmptySequences_AreSetEqual()
        {
            AssertPassed(Assert.That(Enumerable.Empty<object>()).Is.SetEqualTo(Array.Empty<object>()));
        }

        [Test]
        public void OrderDoesNotMatter_ForSetEquality()
        {
            var actual = new string[] { "Hello", "World", "Everyone" };

            AssertPassed(Assert.That(actual).Is.SetEqualTo("Everyone", "Hello", "World"));
        }

        [Test]
        public void DuplicatesDoNotMatter_ForSetEquality()
        {
            var actual = new string[] { "baby", "baby", "goats", "carrots" };

            AssertPassed(Assert.That(actual)
                .Is.SetEqualTo("carrots", "carrots", "carrots", "baby", "goats", "goats", "goats"));
        }

        [Test]
        public void NonEqualSets_AreNotEqual()
        {
            var actual = new string[] { "C#", "Java", "C++" };
            var expected = new string[] { "C#", "C++" };

            AssertPassed(Assert.That(actual).Is.Not.SetEqualTo(expected));
        }

        [Test]
        public void NullIsSetEqualToNull()
        {
            IEnumerable<string> @null = null;
            AssertPassed(Assert.That(@null).Is.SetEqualTo(@null));
        }
    }
}
