using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    internal class Fixture : IEquatable<Fixture>
    {
        public Fixture(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            this.Type = type;
            _Factories = new Lazy<IReadOnlyCollection<Factory>>(() => FindAllFactories(type));
            _Tests = new Lazy<IReadOnlyCollection<MethodInfo>>(
                () => Finder.FindAllValidTestMethods(type)
                    .ToList()
                    .AsReadOnly());
        }

        public Type Type { get; }

        private readonly Lazy<IReadOnlyCollection<MethodInfo>> _Tests;
        public IReadOnlyCollection<MethodInfo> Tests => _Tests.Value;

        private readonly Lazy<IReadOnlyCollection<Factory>> _Factories;
        public IReadOnlyCollection<Factory> Factories => _Factories.Value;

        public static bool Equals(Fixture left, Fixture right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left is null || right is null)
                return false;
            return left.Type == right.Type;
        }
        public static bool operator ==(Fixture left, Fixture right) => Equals(left, right);
        public static bool operator !=(Fixture left, Fixture right) => !Equals(left, right);
        public bool Equals(Fixture other) => Equals(this, other);
        public override bool Equals(object obj)
        {
            return obj is Fixture fixture && Equals(this, fixture);
        }
        public override int GetHashCode() => Type.GetHashCode();

        private static IReadOnlyCollection<Factory> FindAllFactories(Type fixtureType)
        {
            Debug.Assert(fixtureType != null);

            var results = new List<Factory>();

            var @default = Finder.GetDefaultConstructor(fixtureType);
            if (@default != null)
                results.Add(new DefaultCtorFactory(fixtureType, @default));

            var named = Finder.FindNamedConstructors(fixtureType)
                .Select(method => new NamedCtorFactory(fixtureType, method));
            results.AddRange(named);

            return results.AsReadOnly();
        }
    }
}
