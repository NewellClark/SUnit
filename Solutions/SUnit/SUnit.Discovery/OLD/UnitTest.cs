using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
//    /// <summary>
//    /// A single unit test that is ready to be executed.
//    /// </summary>
//    internal class UnitTest
//    {
//        private readonly TestMethod method;

//        /// <summary>
//        /// Creates a new <see cref="UnitTest"/> from the specified <see cref="TestMethod"/> and
//        /// the specified <see cref="DiscoveryOLD.Factory"/>.
//        /// </summary>
//        /// <param name="method"></param>
//        /// <param name="factory"></param>
//        internal UnitTest(TestMethod method, Factory factory)
//        {
//            Debug.Assert(method != null);
//            Debug.Assert(factory != null);

//            this.method = method;
//            this.Factory = factory;
//        }

//        /// <summary>
//        /// Gets the <see cref="DiscoveryOLD.Fixture"/> that defines the test.
//        /// </summary>
//        public Fixture Fixture => Factory.Fixture;

//        /// <summary>
//        /// Gets the <see cref="DiscoveryOLD.Factory"/> that will be used to instantiate the test fixture.
//        /// </summary>
//        public Factory Factory { get; }

//        /// <summary>
//        /// Gets the name of the test method.
//        /// </summary>
//        public string Name => method.Name;

//        /// <summary>
//        /// Overridden to display the name of the method.
//        /// </summary>
//        /// <returns></returns>
//        public override string ToString() => Name;

//        /// <summary>
//        /// Runs the unit test, returning a <see cref="TestResult"/>.
//        /// </summary>
//        /// <returns></returns>
//        public TestResult Run()
//        {
//            try
//            {
//                object fixture = Factory.Build();
//                Test test = method.Execute(fixture);

//                return new RanSuccessfullyResult(this.Name, test);
//            }
//#pragma warning disable CA1031 // Do not catch general exception types
//            catch (Exception ex)
//#pragma warning restore CA1031 // Do not catch general exception types
//            {
//                return new UnexpectedExceptionResult(this.Name, ex);
//            }
//        }

//        public static TestResult Run(object fixture, MethodInfo method)
//        {
//            var func = (Func<Test>)method.CreateDelegate(typeof(Func<Test>), fixture);
//            try
//            {
//                Test test = func();

//                return new RanSuccessfullyResult(method.Name, test);
//            }
//            catch (Exception ex)
//            {
//                return new UnexpectedExceptionResult(method.Name, ex);
//            }
//        }
//    }
}
