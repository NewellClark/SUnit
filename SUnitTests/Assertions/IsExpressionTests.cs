using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static NUnit.Framework.Assert;
using System.Linq;

namespace SUnit.Assertions
{
    [TestFixture]
    public class IsExpressionTests
    {
        [DatapointSource]
        public IEnumerable<(Test normal, Test inverted)> InvertedTestPairs
        {
            get
            {
                yield return (Assert.That(5).Is.EqualTo(5), Assert.That(5).Is.Not.EqualTo(5));
                yield return (Assert.That(5).Is.EqualTo(4), Assert.That(5).Is.Not.EqualTo(4));
                yield return (Assert.That(5).Is.LessThan(4), Assert.That(5).Is.Not.LessThan(4));
                yield return (Assert.That(17).Is.GreaterThan(5), Assert.That(17).Is.Not.GreaterThan(5));
            }
        }

        [DatapointSource]
        public IEnumerable<(int left, int right)> IntegerPair
        {
            get
            {
                return Enumerable.Range(0, 10)
                    .SelectMany(left => Enumerable.Range(0, 10).Select(right => (left, right)));
            }
        }

        [TestFixture]
        public class EqualTo : IsExpressionTests
        {
            [Test]
            public void EqualValues_Passes()
            {
                That(Assert.That(5).Is.EqualTo(5).Passed, Is.True);
            }

            [Test]
            public void UnequalValues_Fails()
            {
                That(Assert.That(5).Is.EqualTo(4).Passed, Is.False);
            }

            [Test]
            public void EqualReferences_Passes()
            {
                var subject = new object();
                That(Assert.That(subject).Is.EqualTo(subject).Passed, Is.True);
            }

            [Test]
            public void UnequalReferences_Fails()
            {
                That(Assert.That(new object()).Is.EqualTo(new object()).Passed, Is.False);
            }

            [Theory]
            public void AnyInvertedTestHasOppositeResult((Test normal, Test inverted) tuple)
            {
                That(tuple.normal.Passed, Is.EqualTo(!tuple.inverted.Passed));
            }
        }

        [TestFixture]
        public class Null : IsExpressionTests
        {
            [Test]
            public void ValueType_IsNotNull()
            {
                That(Assert.That(17).Is.Null.Passed, Is.False);
            }

            [Test]
            public void NullNullable_IsNull()
            {
                int? value = null;
                That(Assert.That(value).Is.Null.Passed, Is.True);
            }

            [Test]
            public void NullReference_IsNull()
            {
                object value = null;
                That(Assert.That(value).Is.Null.Passed, Is.True);
            }
        }

        [TestFixture]
        public class LessThan : IsExpressionTests
        {
            [Test]
            public void ActualSmaller_Passes()
            {
                That(Assert.That(5).Is.LessThan(6).Passed, Is.True);
            }

            [Test]
            public void ActualEqual_Fails()
            {
                That(Assert.That(5).Is.LessThan(5).Passed, Is.False);
            }

            [Test]
            public void ActualLarger_Fails()
            {
                That(Assert.That(5).Is.LessThan(4).Passed, Is.False);
            }

            [Theory]
            public void IsAlwaysOpposite_GreaterThanOrEqualTo((int left, int right) tuple)
            {
                bool lessThan = Assert.That(tuple.left).Is.LessThan(tuple.right).Passed;
                bool greaterThanOrEqualTo = Assert.That(tuple.left).Is.GreaterThanOrEqualTo(tuple.right).Passed;

                That(lessThan, Is.EqualTo(!greaterThanOrEqualTo));
            }
        }

        [TestFixture]
        public class GreaterThan : IsExpressionTests
        {
            [Test]
            public void ActualSmaller_Fails()
            {
                That(Assert.That(5).Is.GreaterThan(6).Passed, Is.False);
            }

            [Test]
            public void ActualEqual_Fails()
            {
                That(Assert.That(5).Is.GreaterThan(5).Passed, Is.False);
            }

            [Test]
            public void ActualLarger_Passes()
            {
                That(Assert.That(5).Is.GreaterThan(4).Passed, Is.True);
            }

            [Theory]
            public void IsAlwaysOppositeLessThanOrEqualTo((int left, int right) tuple)
            {
                bool greater = Assert.That(tuple.left).Is.GreaterThan(tuple.right).Passed;
                bool lessOrEqual = Assert.That(tuple.left).Is.LessThanOrEqualTo(tuple.right).Passed;

                That(greater, Is.EqualTo(!lessOrEqual));
            }
        }
    }
}
