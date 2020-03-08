using SUnit.Discovery;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Fixtures
{

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
        
        public IReadOnlyCollection<TestMethod> Tests { get; }
        public IReadOnlyCollection<Factory> Factories { get; }
    }

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

        public Fixture Fixture { get; }
        public Test Execute(object fixture)
        {
            return (Test)method.Invoke(fixture, Array.Empty<object>());
        }
    }

    public abstract class Factory
    {
        protected private Factory(Fixture fixture)
        {
            Debug.Assert(fixture != null);

            this.Fixture = fixture;
        }

        public Fixture Fixture { get; }

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
