using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    public class TestCase
    {
        private readonly FixtureFactory fixtureFactory;
        private readonly MethodInfo method;
        internal TestCase(MethodInfo method, FixtureFactory fixtureFactory, Type fixture)
        {
            Debug.Assert(method != null);
            Debug.Assert(fixtureFactory != null);
            
            this.method = method;
            this.fixtureFactory = fixtureFactory;
            this.Fixture = fixture;
        }

        /// <summary>
        /// Gets the name of the test method.
        /// </summary>
        public string Name => method.Name;
        /// <summary>
        /// Gets the type that the test method was declared on.
        /// </summary>
        public Type Fixture { get; }
        /// <summary>
        /// Gets the name of the factory method that will instantiate the fixture that the test will be called on.
        /// </summary>
        public string Factory => fixtureFactory.Name;

        public override string ToString() => $"{Fixture.Name}${Factory}${Name}";

        public Test Run()
        {
            object fixture = fixtureFactory.Build();
            return (Test)method.Invoke(fixture, Array.Empty<object>());
        }
    }
}
