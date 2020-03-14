using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    internal sealed class Fixture : IEquatable<Fixture>
    {
        public Fixture(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            this.Type = type;
            _Factories = new Lazy<IReadOnlyCollection<Factory>>(() => FindAllFactories(this));
            _Tests = new Lazy<IReadOnlyCollection<MethodInfo>>(
                () => Rules.FindAllValidTestMethods(type)
                    .ToList()
                    .AsReadOnly());
        }

        public string Save()
        {
            var assemblyPair = new TraitPair(nameof(Assembly), Assembly.Location);
            var typePair = new TraitPair(nameof(Type), Type.FullName);

            return TraitPair.SaveAll(assemblyPair, typePair);
        }

        public static Fixture Load(string serializedFixture)
        {
            var lookup = TraitPair.ParseAll(serializedFixture)
                .ToDictionary(pair => pair.Name);

            string getOrThrow(string name)
            {
                if (!lookup.TryGetValue(name, out TraitPair pair))
                    throw new FormatException($"Missing {name} property when loading {nameof(Fixture)} from {serializedFixture}.");
                return pair.Value;
            }

            Assembly assembly = Assembly.LoadFrom(getOrThrow(nameof(Assembly)));
            Type type = assembly.GetType(getOrThrow(nameof(Type)));

            return new Fixture(type);
        }

        public Assembly Assembly => Type.Assembly;
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

        private static IReadOnlyCollection<Factory> FindAllFactories(Fixture fixture)
        {
            Debug.Assert(fixture != null);

            var results = new List<Factory>();

            var @default = Rules.GetDefaultConstructor(fixture.Type);
            if (@default != null)
                results.Add(Factory.FromDefaultCtor(fixture));

            var named = Rules.FindNamedConstructors(fixture.Type)
                .Select(method => Factory.FromNamedCtor(fixture, method));
            results.AddRange(named);

            return results.AsReadOnly();
        }
    }
}
