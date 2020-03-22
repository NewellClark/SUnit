using System;
using System.Collections.Generic;
using System.Linq;
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
                ReturnType = method.ReturnType;
            }

            //  Used via reflection.
#pragma warning disable IDE0051 // Remove unused private members
            private static NamedCtorFactory Load(Fixture fixture, string subclassData)
#pragma warning restore IDE0051 // Remove unused private members
            {
                var pairs = TraitPair.ParseAll(subclassData).ToDictionary(pair => pair.Name);

                string returnTypeName = pairs[nameof(ReturnType)].Value;
                Type returnType = Type.GetType(returnTypeName);

                string methodName = pairs[nameof(method)].Value;
                var methodInfo = returnType.GetMethod(methodName, Type.EmptyTypes);

                return new NamedCtorFactory(fixture, methodInfo);
            }

            protected private override string SaveSubclassData()
            {
                return TraitPair.SaveAll(
                    new TraitPair(nameof(method), method.Name),
                    new TraitPair(nameof(ReturnType), ReturnType.AssemblyQualifiedName));
            }

            public override Type ReturnType { get; }

            public override object Build()
            {
                try
                {
                    return method.Invoke(null, Array.Empty<object>());
                }
                catch (TargetInvocationException ex) when (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
            }
            public override string Name => method.Name;
            public override bool IsDefaultConstructor => false;
            public override bool IsNamedConstructor => true;
        }
    }
}
