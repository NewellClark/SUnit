using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SUnit.Fixtures
{
    /// <summary>
    /// A unit test method.
    /// </summary>
    public class TestMethod
    {
        private readonly MethodInfo method;

        internal TestMethod(Fixture fixture, MethodInfo method)
        {
            Debug.Assert(fixture != null);
            Debug.Assert(method != null);

            this.Fixture = fixture;
            this.method = method;
        }

        /// <summary>
        /// Gets the <see cref="SUnit.Fixtures.Fixture"/> that defines the <see cref="TestMethod"/>.
        /// </summary>
        public Fixture Fixture { get; }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string Name => method.Name;

        /// <summary>
        /// Executes the <see cref="TestMethod"/> for the specified fixture instance.
        /// </summary>
        /// <param name="fixture">An instance of the class that defines the <see cref="TestMethod"/>.</param>
        /// <returns>The result of executing the test.</returns>
        public Test Execute(object fixture)
        {
            return (Test)method.Invoke(fixture, Array.Empty<object>());
        }

        /// <summary>
        /// Overridden to display the method name.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => method.Name;
    }
}
