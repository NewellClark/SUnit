using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    internal abstract class Factory
    {
        protected private Factory(Type fixtureType)
        {
            if (fixtureType is null) throw new ArgumentNullException(nameof(fixtureType));

            this.FixtureType = fixtureType;
        }

        public Type FixtureType { get; }
        public abstract object Build();
        public abstract string Name { get; }
        public abstract bool IsDefaultConstructor { get; }
        public abstract bool IsNamedConstructor { get; }
    }

    internal class DefaultCtorFactory : Factory
    {
        private readonly ConstructorInfo ctor;

        public DefaultCtorFactory(Type fixtureType, ConstructorInfo ctor)
            : base(fixtureType)
        {
            if (ctor is null) throw new ArgumentNullException(nameof(ctor));
            if (ctor.GetParameters().Length > 0)
                throw new ArgumentException($"{nameof(ctor)} must have no parameters.");

            this.ctor = ctor;
        }

        public override object Build()
        {
            return ctor.Invoke(Array.Empty<object>());
        }

        public override string Name => "ctor";
        public override bool IsDefaultConstructor => true;
        public override bool IsNamedConstructor => false;
    }

    internal class NamedCtorFactory : Factory
    {
        private readonly MethodInfo method;

        public NamedCtorFactory(Type fixtureType, MethodInfo method)
            : base(fixtureType)
        {
            if (method is null) throw new ArgumentNullException(nameof(method));
            if (!method.IsStatic)
                throw new ArgumentException($"{nameof(method)} must be static.", nameof(method));
            if (method.GetParameters().Length > 0)
                throw new ArgumentException($"{nameof(method)} must have no parameters.", nameof(method));

            this.method = method;
        }

        public override object Build() => method.Invoke(null, Array.Empty<object>());
        public override string Name => method.Name;
        public override bool IsDefaultConstructor => false;
        public override bool IsNamedConstructor => true;
    }
}
