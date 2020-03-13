using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    partial class Factory
    {
        private sealed class NamedCtorFactory : Factory
        {
            private readonly MethodInfo method;

            public NamedCtorFactory(Fixture fixture, MethodInfo method)
                : base(fixture)
            {
                if (method is null) throw new ArgumentNullException(nameof(method));
                if (!method.IsStatic)
                    throw new ArgumentException($"{nameof(method)} must be static.", nameof(method));
                if (method.GetParameters().Length > 0)
                    throw new ArgumentException($"{nameof(method)} must have no parameters.", nameof(method));

                this.method = method;
            }

            //  Used via reflection.
#pragma warning disable IDE0051 // Remove unused private members
            private static NamedCtorFactory Load(Fixture fixture, string subclassData)
#pragma warning restore IDE0051 // Remove unused private members
            {
                string name = TraitPair.Parse(subclassData).Value;
                var method = fixture.Type.GetMethod(name, Type.EmptyTypes);

                return new NamedCtorFactory(fixture, method);
            }

            private protected override string SaveSubclassData()
            {
                var namePair = new TraitPair(nameof(Name), Name);
                return namePair.Save();
            }

            public override object Build() => method.Invoke(null, Array.Empty<object>());
            public override string Name => method.Name;
            public override bool IsDefaultConstructor => false;
            public override bool IsNamedConstructor => true;
        }
    }
}
