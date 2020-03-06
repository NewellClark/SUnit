using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    /// <summary>
    /// A method that is responsible for instantiating a Fixture.
    /// </summary>
    internal abstract class FixtureFactory
    {
        protected private FixtureFactory() { }

        /// <summary>
        /// Instantiates the fixture.
        /// </summary>
        /// <returns>A newly-instantiated fixture.</returns>
        public abstract object Build();
        public abstract Type Fixture { get; }
        public abstract string Name { get; }

        private sealed class DefaultConstructorFixtureFactory : FixtureFactory
        {
            private readonly ConstructorInfo ctor;

            public DefaultConstructorFixtureFactory(ConstructorInfo ctor)
            {
                Debug.Assert(ctor != null);
                Debug.Assert(ctor.IsPublic);
                Debug.Assert(!ctor.IsStatic);
                Debug.Assert(ctor.GetParameters().Length == 0);

                this.ctor = ctor;
            }
            public override object Build() => ctor.Invoke(Array.Empty<object>());
            public override Type Fixture => ctor.DeclaringType;
            public override string Name => "<default ctor>";
        }

        internal static FixtureFactory FromDefaultConstructor(ConstructorInfo ctor)
        {
            return new DefaultConstructorFixtureFactory(ctor);
        }

        private sealed class NamedConstructorFixtureFactory : FixtureFactory
        {
            private readonly Func<object> factory;
            private readonly MethodInfo method;

            public NamedConstructorFixtureFactory(MethodInfo method)
            {
                Debug.Assert(method != null);

                factory = (Func<object>)method.CreateDelegate(typeof(Func<object>));
                this.method = method;
            }

            public override object Build() => factory();
            public override Type Fixture => method.DeclaringType;
            public override string Name => method.Name;
        }
        internal static FixtureFactory FromNamedConstructor(MethodInfo method)
        {
            return new NamedConstructorFixtureFactory(method);
        }
    }
}
