using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SUnit.Fixtures
{
    /// <summary>
    /// An object that can be used to instantiate a <see cref="SUnit.Fixtures.Fixture"/>.
    /// </summary>
    public abstract class Factory
    {
        protected private Factory(Fixture fixture)
        {
            Debug.Assert(fixture != null);

            this.Fixture = fixture;
        }

        /// <summary>
        /// Gets the <see cref="SUnit.Fixtures.Fixture"/> that the <see cref="Factory"/> instantiates.
        /// </summary>
        public Fixture Fixture { get; }

        /// <summary>
        /// Instantiates the <see cref="SUnit.Fixtures.Fixture"/>.
        /// </summary>
        /// <returns>An instantiated <see cref="SUnit.Fixtures.Fixture"/>.</returns>
        public abstract object Build();


        private sealed class DefaultConstructorFactory : Factory
        {
            private readonly ConstructorInfo ctor;

            public DefaultConstructorFactory(Fixture fixture, ConstructorInfo ctor) : base(fixture)
            {
                this.ctor = ctor;
            }

            public override object Build() => ctor.Invoke(Array.Empty<object>());
            public override string ToString() => "<default ctor>";
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

            public override object Build() => method.Invoke(null, Array.Empty<object>());
            public override string ToString() => method.Name;
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
