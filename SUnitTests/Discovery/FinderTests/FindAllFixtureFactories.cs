using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SUnit.Discovery.FinderTests
{
    public abstract class FindAllFixtureFactories
    {
        private IEnumerable<FixtureFactory> Factories => Finder.FindAllFactories(MockType);
        protected abstract Type MockType { get; }

        [TestFixture]
        public class PublicDefaultConstructor : FindAllFixtureFactories
        {
            private class Mock { public Mock() { } }
            protected override Type MockType => typeof(Mock);

            [Test]
            public void IsIncluded()
            {
                Assert.That(Factories.Count(), Is.EqualTo(1));
            }

            [Test]
            public void IsExecutedByBuildMethod()
            {
                var result = Factories.Single().Build();

                Assert.That(result, Is.InstanceOf<Mock>());
            }
        }

        [TestFixture]
        public class PrivateDefaultConstructor : FindAllFixtureFactories
        {
            private class Mock { private Mock() { } };
            protected override Type MockType => typeof(Mock);

            [Test]
            public void IsNotIncluded()
            {
                Assert.That(Factories, Is.Empty);
            }
        }

        [TestFixture]
        public class MultipleFactoryMethods : FindAllFixtureFactories
        {
            private class Mock
            {
                private Mock([CallerMemberName] string name = "")
                {
                    NamedConstructor = name;
                }

                public static Mock Alpha() => new Mock();
                public static Mock Bravo() => new Mock();
                public static Mock Charlie() => new Mock();

                public string NamedConstructor { get; }
                public override string ToString()
                {
                    return NamedConstructor;
                }
            }

            protected override Type MockType => typeof(Mock);

            [Test]
            public void AreAllIncluded()
            {
                var result = Factories.Select(f => f.Build().ToString());
                string[] expected = { nameof(Mock.Alpha), nameof(Mock.Bravo), nameof(Mock.Charlie) };

                Assert.That(result, Is.EquivalentTo(expected));
            }
        }

        [TestFixture]
        public class StaticMethodWithArguments : FindAllFixtureFactories
        {
            private class Mock
            {
                private Mock() { }
                public static Mock HasArguments(int whatever) { return new Mock(); }
            }
            protected override Type MockType => typeof(Mock);

            [Test]
            public void IsNotIncluded()
            {
                Assert.That(Factories, Is.Empty);
            }
        }

        [TestFixture]
        public class NonPublicMethod : FindAllFixtureFactories
        {
            private class Mock
            {
                private Mock() { }
                internal static Mock NonPublic() => new Mock();
            }
            protected override Type MockType => typeof(Mock);

            [Test]
            public void IsNotIncluded()
            {
                Assert.That(Factories, Is.Empty);
            }
        }
    }
}
