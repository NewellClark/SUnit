using SUnit.Discovery;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Fixtures
{
    /// <summary>
    /// A class that defines unit tests.
    /// </summary>
    public class Fixture
    {
        private readonly Type type;

        internal Fixture(Type type)
        {
            Debug.Assert(type != null);

            this.type = type;

            Tests = Finder.FindAllValidTestMethods(type)
                .Select(method => new TestMethod(this, method))
                .ToList()
                .AsReadOnly();

            var factories = Finder.FindAllNamedConstructors(type)
                .Select(method => Factory.FromNamedCtor(this, method));
            var ctor = Finder.GetDefaultConstructor(type);
            if (ctor != null)
                factories = factories.Prepend(Factory.FromDefaultCtor(this, ctor));
            this.Factories = factories.ToList().AsReadOnly();
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
    }


}
