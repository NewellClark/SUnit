using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    internal abstract partial class Factory
    {
        private Factory(Fixture fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            this.Fixture = fixture;
        }

        protected private abstract string SaveSubclassData();

        public string Save()
        {
            var ourTypePair = new TraitPair(nameof(Type), GetType().FullName);
            var fixturePair = new TraitPair(nameof(Fixture), Fixture.Save());
            var subclassDataPair = new TraitPair("SubclassData", SaveSubclassData());
            return TraitPair.SaveAll(ourTypePair, fixturePair, subclassDataPair);
        }

        public static Factory Load(string serializedFactory)
        {
            var lookup = TraitPair.ParseAll(serializedFactory)
                .ToDictionary(pair => pair.Name);

            Fixture fixture = Fixture.Load(lookup[nameof(Fixture)].Value);
            string subclassData = lookup["SubclassData"].Value;
            Type subclassType = Type.GetType(lookup[nameof(Type)].Value);

            Debug.Assert(typeof(Factory).IsAssignableFrom(subclassType));

            var loadMethod = subclassType.GetMethod(
                "Load", BindingFlags.Static | BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            return (Factory)loadMethod.Invoke(null, new object[] { fixture, subclassData });
        }

        public Fixture Fixture { get; }

        public abstract object Build();

        public abstract string Name { get; }

        public abstract bool IsDefaultConstructor { get; }

        public abstract bool IsNamedConstructor { get; }

        public IEnumerable<UnitTest> CreateTests()
        {
            return Fixture.Tests
                .Select(method => new UnitTest(this, method));
        }

        public static Factory FromDefaultCtor(Fixture fixture) => new DefaultCtorFactory(fixture);

        public static Factory FromNamedCtor(Fixture fixture, MethodInfo method) => new NamedCtorFactory(fixture, method);
    }
}
