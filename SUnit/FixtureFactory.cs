using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// A method that is responsible for instantiating a Fixture.
    /// </summary>
    public abstract class FixtureFactory
    {
        protected private FixtureFactory() { }

        /// <summary>
        /// Instantiates the fixture.
        /// </summary>
        /// <returns>A newly-instantiated fixture.</returns>
        public abstract object Build();

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
        }

        internal static FixtureFactory FromDefaultConstructor(ConstructorInfo ctor)
        {
            return new DefaultConstructorFixtureFactory(ctor);
        }

        private sealed class NamedConstructorFixtureFactory : FixtureFactory
        {
            private readonly Func<object> factory;

            public NamedConstructorFixtureFactory(MethodInfo method)
            {
                Debug.Assert(method != null);

                factory = (Func<object>)method.CreateDelegate(typeof(Func<object>));
            }

            public override object Build() => factory();
        }
        internal static FixtureFactory FromNamedConstructor(MethodInfo method)
        {
            return new NamedConstructorFixtureFactory(method);
        }
    }
}
