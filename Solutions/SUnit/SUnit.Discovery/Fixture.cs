using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

/*  Everything here must be:
 *  -   Not concerned with actually RUNNING tests.
 *  -   Easily serializable to text.
 *  
 *  A Fixture can:
 *  -   Tell you its Assembly.
 *  -   Tell you its Type.
 *  -   Tell you its TestMethods.
 *  -   Tell you all its Factories.
 *  -   Be round-trip serialized to text.
 *  -   Use value-equality semantics.
 *  
 *  A Factory can:
 *  -   Tell you its' Fixture's type (not its fixture!).
 *  -   Instantiate an instance of the Fixture type (not an instance of the Fixture class!). 
 *  -   Be round-trip serialized to text.
 *  -   Use value-equality semantics.
 *  
 *  A TestMethod can:
 *  -   Tell you its' Fixture's type. 
 *  -   Tell you what kind of test it is (once we add multi-tests and async tests). 
 *  -   Give you the MethodInfo for the test method it encapsulates.
 *  -   Be round-trip serialized to text.
 *  -   Use value-equality semantics.
 *  
 *  A UnitTest can:
 *  -   Tell you its' Fixture's Type.
 *  -   Tell you its' Factory.
 *  -   Tell you its' TestMethod.
 * */
namespace SUnit.Discovery
{
    internal class Fixture
    {
        public Fixture(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            this.Type = type;
            _Factories = new Lazy<IReadOnlyCollection<Factory>>(() => FindAllFactories(type));
            _Tests = new Lazy<IReadOnlyCollection<MethodInfo>>(
                () => Finder.FindAllValidTestMethods(type)
                    .ToList()
                    .AsReadOnly());
        }

        public Type Type { get; }

        private readonly Lazy<IReadOnlyCollection<MethodInfo>> _Tests;
        public IReadOnlyCollection<MethodInfo> Tests => _Tests.Value;

        private readonly Lazy<IReadOnlyCollection<Factory>> _Factories;
        public IReadOnlyCollection<Factory> Factories => _Factories.Value;
        
        private static IReadOnlyCollection<Factory> FindAllFactories(Type fixtureType)
        {
            Debug.Assert(fixtureType != null);

            var results = new List<Factory>();

            var @default = Finder.GetDefaultConstructor(fixtureType);
            if (@default != null)
                results.Add(new DefaultCtorFactory(fixtureType, @default));

            var named = Finder.FindNamedConstructors(fixtureType)
                .Select(method => new NamedCtorFactory(fixtureType, method));
            results.AddRange(named);

            return results.AsReadOnly();
        }
    }
}
