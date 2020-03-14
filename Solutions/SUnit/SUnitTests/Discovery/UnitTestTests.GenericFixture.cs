using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using nAssert = NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    partial class UnitTestTests
    {
        [TestFixture]
        public class GenericFixture : UnitTestTests
        {
            private class Mock<T>
            {
                private T valueEqualA, valueEqualB, notEqual;

                public Mock(T valueEqualA, T valueEqualB, T notEqual)
                {
                    this.valueEqualA = valueEqualA;
                    this.valueEqualB = valueEqualB;
                    this.notEqual = notEqual;
                }

                public static Mock<int> Integers() => new Mock<int>(61, 61, -1992);

                public static Mock<string> Strings()
                {
                    var now = DateTime.UtcNow;

                    return new Mock<string>(now.ToString(), now.ToString(), "Freak the Freak Out");
                }

                public static Mock<object> Objects()
                {
                    object ab = new object();
                    object not = new object();

                    return new Mock<object>(ab, ab, not);
                }

                public static Mock<List<T>> StillNotConstructed()
                {
                    var ab = new List<T>();
                    var not = new List<T>();

                    return new Mock<List<T>>(ab, ab, not);
                }

                public static Mock<TimeSpan> AlsoNotConstructed<U>()
                {
                    return new Mock<TimeSpan>(
                        TimeSpan.FromSeconds(49),
                        TimeSpan.FromSeconds(49),
                        TimeSpan.FromMilliseconds(7));
                }

                [Pass]
                public Test ReferenceEquals() => Assert.That(valueEqualA).Is.EqualTo(valueEqualA);

                [Fail]
                public Test NotReferenceEquals() => Assert.That(valueEqualA).Is.Not.EqualTo(valueEqualA);

                //[Pass]
                //public Test ValueEquals() => Assert.That(valueEqualA).Is.EqualTo(valueEqualB);

                //[Fail]
                //public Test NotValueEquals() => Assert.That(valueEqualA).Is.Not.EqualTo(valueEqualB);

                //[Fail]
                //public Test EqualTo_NotEqual() => Assert.That(valueEqualA).Is.EqualTo(notEqual);

                //[Pass]
                //public Test NotEqualTo_NotEqual() => Assert.That(valueEqualA).Is.Not.EqualTo(notEqual);
            }

            private protected override Type FixtureType => typeof(Mock<>);
        }
    }
}
