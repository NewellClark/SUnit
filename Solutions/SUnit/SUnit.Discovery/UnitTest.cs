using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    internal class UnitTest
    {
        private readonly MethodInfo method;
        private readonly Factory factory;

        internal UnitTest(MethodInfo method, Factory factory)
        {
            if (method is null) throw new ArgumentNullException(nameof(method));
            if (factory is null) throw new ArgumentNullException(nameof(factory));

            this.method = method;
            this.factory = factory;
        }

        public string Name => method.Name;

        public Test Execute()
        {
            object fixture = factory.Build();
            Func<Test> func = (Func<Test>)method.CreateDelegate(typeof(Func<Test>), fixture);

            return func();
        }
    }
}
