using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using nAssert = NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    [TestFixture]
    public class SerializationTests
    {
        private class Mock
        {
            public Mock() : this("ctor") { }

            private Mock(string builder) => Builder = builder;

            public static Mock NamedConstructor() => new Mock(nameof(NamedConstructor));

            public string Builder { get; }

            public Test Test() => Assert.That(2 + 2).Is.EqualTo(4);
        }

        [TestFixture]
        public class FactorySerialization
        {
            [DatapointSource]
            private IEnumerable<Type> Types
            {
                get
                {
                    return typeof(Factory).Assembly.GetTypes()
                        .Where(type => typeof(Factory).IsAssignableFrom(type))
                        .Where(type => !type.IsAbstract)
                        .Where(type => !type.ContainsGenericParameters);
                }
            }

            [Theory]
            public void AllInstantiableFactorySubclasses_HaveLoadMethod(Type subtype)
            {
                var candidates = GetLoadMethodCandidates(subtype);

                nAssert.That(candidates, Has.Exactly(1).Items);
            }

            private static IEnumerable<MethodInfo> GetLoadMethodCandidates(Type factorySubtype)
            {
                return factorySubtype.GetRuntimeMethods()
                    .Where(m => m.IsStatic)
                    .Where(m => m.GetParameters().Length == 2)
                    .Where(m => m.Name == "Load");
            }

            [Test]
            public void DefaultCtorFactory_IsRoundTripSerializableAsText()
            {
                var factory = new Fixture(typeof(Mock))
                    .Factories.Where(f => f.IsDefaultConstructor)
                    .Single();

                string serialized = factory.Save();
                var roundTripped = Factory.Load(serialized);

                Mock mock = (Mock)roundTripped.Build();

                nAssert.That(mock.Builder, Is.EqualTo("ctor"));
            }

            [Test]
            public void NamedCtorFactory_IsRoundTripSerializableAsText()
            {
                var factory = new Fixture(typeof(Mock))
                    .Factories.Where(f => f.IsNamedConstructor)
                    .Single();

                string serialized = factory.Save();
                var roundTripped = Factory.Load(serialized);

                Mock mock = (Mock)roundTripped.Build();

                nAssert.That(mock.Builder, Is.EqualTo(nameof(mock.NamedConstructor)));
            }
        }

        [TestFixture]
        public class UnitTestSerialization
        {
            [Test]
            public void UnitTest_IsRoundTripSerializable()
            {
                UnitTest original = new Fixture(typeof(Mock))
                    .Factories.First()
                    .CreateTests().Single();
                string serialized = original.Save();
                UnitTest roundTripped = UnitTest.Load(serialized);

                nAssert.That(roundTripped.Execute(), Is.InstanceOf<Test>());
            }
        }
    }
}
