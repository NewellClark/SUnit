using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using assert = NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    [TestFixture]
    public abstract class FixtureFactoryTests
    {
        private class MockBase
        {
            protected MockBase() { }
            protected MockBase(string builder) => Builder = builder;

            public string Builder { get; private set; }
            public string LastExecuted { get; private set; } = "";

            protected void SetBuilder([CallerMemberName]string builderName = "") => Builder = builderName;
            protected void SetLastExecuted([CallerMemberName]string caller = "") => LastExecuted = caller;
        }

        protected abstract Type Type { get; }
        protected private Fixture Fixture { get; }
        protected private Factory Factory => Fixture.Factories.Single();
        private MockBase Build() => (MockBase)Fixture.Factories.Single().Build();

        protected FixtureFactoryTests()
        {
            Fixture = new Fixture(Type);
        }


        [TestFixture]
        public class DefaultCtor : FixtureFactoryTests
        {
            private class Mock : MockBase { public Mock() { SetBuilder("Default"); } }

            protected override Type Type => typeof(Mock);

            [Test]
            public void IsValidFactory()
            {
                assert.That(Fixture.Factories, Has.Count.EqualTo(1));
            }

            [Test]
            public void IsCalledByFactory()
            {
                assert.That(Build().Builder, Is.EqualTo("Default"));
            }

            [Test]
            public void InstantiatesInstanceOfFixture()
            {
                assert.That(Build(), Is.InstanceOf<Mock>());
            }

            [Test]
            public void IsDefaultConstructor()
            {
                assert.That(Factory.IsDefaultConstructor, Is.True);
            }

            [Test]
            public void IsNotNamedConstructor()
            {
                assert.That(Factory.IsNamedConstructor, Is.False);
            }
        }

        [TestFixture]
        public class PublicStaticNoParamMockReturningMethod : FixtureFactoryTests
        {
            private class Mock : MockBase
            {
                private Mock(string builder) : base(builder) { }
                public static Mock NamedCtor() => new Mock(nameof(NamedCtor));
            }

            protected override Type Type => typeof(Mock);

            [Test]
            public void IsValidFactory()
            {
                assert.That(Fixture.Factories, Has.Count.EqualTo(1));
            }

            [Test]
            public void IsCalledByFactory()
            {
                assert.That(Build().Builder, Is.EqualTo(nameof(Mock.NamedCtor)));
            }
        }

        [TestFixture]
        public class NonPublicMethod : FixtureFactoryTests
        {
            private class Mock : MockBase
            {
                private Mock() { }
                internal static Mock NamedCtor() => new Mock();
            }
            protected override Type Type => typeof(Mock);

            [Test]
            public void IsNotValidFactory()
            {
                assert.That(Fixture.Factories, Is.Empty);
            }
        }

        [TestFixture]
        public class InstanceMethod : FixtureFactoryTests
        {
            private class Mock : MockBase
            {
                private Mock() { }
                public Mock InstanceMethod() => new Mock();
            }

            protected override Type Type => typeof(Mock);

            [Test]
            public void IsNotValidFactory()
            {
                assert.That(Fixture.Factories, Is.Empty);
            }
        }

        [TestFixture]
        public class DefaultCtorOnAbstractClass : FixtureFactoryTests
        {
            private abstract class Mock : MockBase
            {
                public Mock() { }
            }

            protected override Type Type => typeof(Mock);

            [Test]
            public void IsNotValidFactory()
            {
                assert.That(Fixture.Factories, Is.Empty);
            }
        }

        [TestFixture]
        public abstract class DefaultCtorOnGeneric : FixtureFactoryTests
        {
            private class Mock<T> : MockBase
            {
                public Mock() { SetBuilder(typeof(T).Name); }
            }

            [TestFixture]
            public class Unconstructed : DefaultCtorOnGeneric
            {
                protected override Type Type => typeof(Mock<>);

                [Test]
                public void IsNotValidFactory()
                {
                    assert.That(Fixture.Factories, Is.Empty);
                }
            }

            [TestFixture]
            public class Constructed : DefaultCtorOnGeneric
            {
                protected override Type Type => typeof(Mock<string>);

                [Test]
                public void IsValidFactory()
                {
                    assert.That(Fixture.Factories, Has.Count.EqualTo(1));
                }

                [Test]
                public void IsCalledByFactory()
                {
                    assert.That(Build().Builder, Is.EqualTo(typeof(string).Name));
                }
            }
        }

        [TestFixture]
        public abstract class NamedCtorOnGeneric : FixtureFactoryTests
        {
            [TestFixture]
            public class ReturnsUnconstructed : NamedCtorOnGeneric
            {
                private class Mock<T> : MockBase
                {
                    private Mock() { }
                    public static Mock<T> Unconstructed() => new Mock<T>();
                }

                protected override Type Type => typeof(Mock<>);

                [Test]
                public void IsNotValidFactory()
                {
                    assert.That(Fixture.Factories, Is.Empty);
                }
            }

            [TestFixture]
            public class ReturnsConstructed : NamedCtorOnGeneric
            {
                private class Mock<T> : MockBase
                {
                    private Mock(string builder) : base(builder) { }
                    public static Mock<TimeSpan> ConstructedWithTimeSpan()
                    {
                        return new Mock<TimeSpan>(nameof(ConstructedWithTimeSpan));
                    }
                }

                protected override Type Type => typeof(Mock<>);

                [Test]
                public void HasOneFactory()
                {
                    assert.That(Fixture.Factories, Has.Count.EqualTo(1));
                }
                [Test]
                public void HasCorrectName()
                {
                    assert.That(Factory.Name, Is.EqualTo(nameof(Mock<TimeSpan>.ConstructedWithTimeSpan)));
                }

                [Test]
                public void IsCalledByFactory()
                {
                    assert.That(Build().Builder, Is.EqualTo(nameof(Mock<TimeSpan>.ConstructedWithTimeSpan)));
                }

                [Test]
                public void BuildsConstructedInstance()
                {
                    assert.That(Build(), Is.InstanceOf<Mock<TimeSpan>>());
                }
            }

            [TestFixture]
            public class ReturnsSubclassOfConstructed : NamedCtorOnGeneric
            {
                private class Mock<T> : MockBase
                {
                    protected Mock(string builder) : base(builder) { }
                    public static Subclass ConstructedSubclass()
                    {
                        return new Subclass(nameof(ConstructedSubclass));
                    }
                }

                private class Subclass : Mock<DateTime>
                {
                    public Subclass(string builder) : base(builder) { }
                }

                protected override Type Type => typeof(Mock<>);

                [Test]
                public void HasOneFactory()
                {
                    assert.That(Fixture.Factories, Has.Count.EqualTo(1));
                }

                [Test]
                public void HasCorrectName()
                {
                    assert.That(Factory.Name, Is.EqualTo(nameof(Mock<DateTime>.ConstructedSubclass)));
                }

                [Test]
                public void IsCalledByFactory()
                {
                    assert.That(Build().Builder, Is.EqualTo(nameof(Mock<DateTime>.ConstructedSubclass)));
                }

                [Test]
                public void BuildsConstructedInstance()
                {
                    assert.That(Build(), Is.InstanceOf<Mock<DateTime>>());
                }
            }
        }
    }
}
