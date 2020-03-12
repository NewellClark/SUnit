using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    internal class UnitTest
    {
        private readonly MethodInfo method;


        internal UnitTest(Factory factory, MethodInfo method)
        {
            if (method is null) throw new ArgumentNullException(nameof(method));
            if (factory is null) throw new ArgumentNullException(nameof(factory));

            this.method = method;
            this.Factory = factory;
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
