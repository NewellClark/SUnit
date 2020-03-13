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
    public class FixtureFactoryTests
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
    }
}
