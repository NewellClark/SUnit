using System;
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
            var method = factory.ReturnType.GetMethod(name, Type.EmptyTypes);

            return new UnitTest(factory, method);
        }

        public Factory Factory { get; }

        public string Name => method.Name;

        /// <summary>
        /// Gets the return type of the test method.
        /// </summary>
        public Type ReturnType => method.ReturnType;

        /// <summary>
        /// Creates a new fixture instance and binds a delegate for the test method to it.
        /// </summary>
        /// <returns>A newly-created delegate with a newly-created test fixture as the receiver.</returns>
        public Func<object> CreateDelegate()
        {
            object fixture = InstantiateFixture();

            return CreateDelegate(fixture);
        }

        /// <summary>
        /// Creates a new test method delegate with the specified fixture instance as the receiver.
        /// </summary>
        /// <param name="fixture">An instance of the test fixture to bind t</param>
        /// <returns>A delegate created by binding the test method to the specified fixture instance.</returns>
        public Func<object> CreateDelegate(object fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            return (Func<object>)method.CreateDelegate(typeof(Func<object>), fixture);
        }
        
        public object InstantiateFixture() => Factory.Build();

        public object Execute() => CreateDelegate()();
    }
}
