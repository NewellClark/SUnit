﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    internal sealed class UnitTest
    {
        private readonly MethodInfo method;
        
        internal UnitTest(Factory factory, MethodInfo method)
        {
            if (method is null) throw new ArgumentNullException(nameof(method));
            if (factory is null) throw new ArgumentNullException(nameof(factory));

            this.method = method;
            this.Factory = factory;
        }

        public string Save()
        {
            var factoryPair = new TraitPair(nameof(Factory), Factory.Save());
            var namePair = new TraitPair(nameof(Name), Name);

            return TraitPair.SaveAll(factoryPair, namePair);
        }

        public static UnitTest Load(string serialized)
        {
            var lookup = TraitPair.ParseAll(serialized).ToDictionary(pair => pair.Name);
            Factory factory = Factory.Load(lookup[nameof(Factory)].Value);
            string name = lookup[nameof(Name)].Value;
            var method = factory.Fixture.Type.GetMethod(name, Type.EmptyTypes);

            return new UnitTest(factory, method);
        }

        public Factory Factory { get; }

        public string Name => method.Name;

        public Test Execute()
        {
            object fixture = Factory.Build();
            Func<Test> func = (Func<Test>)method.CreateDelegate(typeof(Func<Test>), fixture);

            return func();
        }
    }
}
