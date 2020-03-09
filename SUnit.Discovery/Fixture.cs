using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    /// <summary>
    /// A class that defines unit tests.
    /// </summary>
    public class Fixture
    {
        private readonly Type type;

        /// <summary>
        /// Creates a new <see cref="Fixture"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        public Fixture(Type type)
        {
            Debug.Assert(type != null);

            this.type = type;

            Tests = Finder.FindAllValidTestMethods(type)
                .Select(method => new TestMethod(this, method))
                .ToList()
                .AsReadOnly();

            Factories = FindAllFactories(this);
        }

        /// <summary>
        /// Gets the unqualified name of the class that the fixture represents.
        /// </summary>
        public string Name => type.Name;

        /// <summary>
        /// Gets all the test methods on the fixture.
        /// </summary>
        public IReadOnlyCollection<TestMethod> Tests { get; }

        /// <summary>
        /// Gets all the methods that can be used to instantiate the fixture.
        /// </summary>
        public IReadOnlyCollection<Factory> Factories { get; }

        private static IReadOnlyCollection<Factory> FindAllFactories(Fixture fixture)
        {
            Debug.Assert(fixture != null);

            var results = new List<Factory>();

            var @default = Finder.GetDefaultConstructor(fixture.type);
            if (@default != null)
                results.Add(Factory.FromDefaultCtor(fixture, @default));

            var named = Finder.FindNamedConstructors(fixture.type)
                .Select(method => Factory.FromNamedCtor(fixture, method));
            results.AddRange(named);

            return results.AsReadOnly();
        }
    }
}
