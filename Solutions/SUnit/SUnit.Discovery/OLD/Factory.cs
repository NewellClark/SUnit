using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.DiscoveryOLD
{
    /// <summary>
    /// An object that can be used to instantiate a <see cref="DiscoveryOLD.Fixture"/>.
    /// </summary>
    internal abstract class Factory
    {
        protected private Factory(Fixture fixture)
        {
            Debug.Assert(fixture != null);

            Fixture = fixture;
        }

        /// <summary>
        /// Gets the <see cref="DiscoveryOLD.Fixture"/> that the <see cref="Factory"/> instantiates.
        /// </summary>
        public Fixture Fixture { get; }

        /// <summary>
        /// Instantiates the <see cref="DiscoveryOLD.Fixture"/>.
        /// </summary>
        /// <returns>An instantiated <see cref="DiscoveryOLD.Fixture"/>.</returns>
        public abstract object Build();

        /// <summary>
        /// Indicates whether the current <see cref="Factory"/> is a default constructor.
        /// </summary>
        public abstract bool IsDefaultConstructor { get; }

        /// <summary>
        /// Indicates whether the current <see cref="Factory"/> is a named constructor.
        /// </summary>
        public abstract bool IsNamedConstructor { get; }

        /// <summary>
        /// Gets the name of the constructor. For named constructors, this will be the name of the method.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Uses the current <see cref="Factory"/> to instantiate all the <see cref="UnitTest"/>s
        /// on the <see cref="SUnit.DiscoveryOLD.Fixture"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UnitTest> CreateTests()
        {
            return Fixture.Tests.Select(method => new UnitTest(method, this));
        }

        /// <summary>
        /// Overridden to display the name.
        /// </summary>
        /// <returns>The name.</returns>
        public override string ToString() => Name;

        private sealed class DefaultConstructorFactory : Factory
        {
            private readonly ConstructorInfo ctor;

            public DefaultConstructorFactory(Fixture fixture, ConstructorInfo ctor) : base(fixture)
            {
                this.ctor = ctor;
            }

            public override object Build() => ctor.Invoke(Array.Empty<object>());
            public override bool IsDefaultConstructor => true;
            public override bool IsNamedConstructor => false;
            public override string Name => "ctor";
        }

        internal static Factory FromDefaultCtor(Fixture fixture, ConstructorInfo ctor)
        {
            Debug.Assert(fixture != null);
            Debug.Assert(ctor != null);
            Debug.Assert(ctor.GetParameters().Length == 0);

            return new DefaultConstructorFactory(fixture, ctor);
        }

        private sealed class NamedConstructorFactory : Factory
        {
            private readonly MethodInfo method;

            public NamedConstructorFactory(Fixture fixture, MethodInfo method) : base(fixture)
            {
                this.method = method;
            }

            public override bool IsDefaultConstructor => false;
            public override bool IsNamedConstructor => true;
            public override object Build() => method.Invoke(null, Array.Empty<object>());
            public override string Name => method.Name;
        }

        internal static Factory FromNamedCtor(Fixture fixture, MethodInfo method)
        {
            Debug.Assert(fixture != null);
            Debug.Assert(method != null);
            Debug.Assert(method.GetParameters().Length == 0);

            return new NamedConstructorFactory(fixture, method);
        }
    }
}
