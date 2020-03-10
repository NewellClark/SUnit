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
    internal class Fixture
    {

        /// <summary>
        /// Creates a new <see cref="Fixture"/> for the specified <see cref="System.Type"/>.
        /// </summary>
        /// <param name="type"></param>
        public Fixture(Type type)
        {
            Debug.Assert(type != null);

            this.Type = type;

            Tests = Finder.FindAllValidTestMethods(type)
                .Select(method => new TestMethod(this, method))
                .ToList()
                .AsReadOnly();

            Factories = FindAllFactories(this);
        }

        public Type Type { get; }

        /// <summary>
        /// Gets the unqualified name of the class that the fixture represents.
        /// </summary>
        public string Name => Type.Name;

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

            var @default = Finder.GetDefaultConstructor(fixture.Type);
            if (@default != null)
                results.Add(Factory.FromDefaultCtor(fixture, @default));

            var named = Finder.FindNamedConstructors(fixture.Type)
                .Select(method => Factory.FromNamedCtor(fixture, method));
            results.AddRange(named);

            return results.AsReadOnly();
        }
    }
}
