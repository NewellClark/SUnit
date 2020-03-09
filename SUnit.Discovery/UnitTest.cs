using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SUnit.Discovery
{
    /// <summary>
    /// A single unit test that is ready to be executed.
    /// </summary>
    public class UnitTest
    {
        private readonly TestMethod method;
        private readonly Factory factory;

        /// <summary>
        /// Creates a new <see cref="UnitTest"/> from the specified <see cref="TestMethod"/> and
        /// the specified <see cref="Factory"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="factory"></param>
        internal UnitTest(TestMethod method, Factory factory)
        {
            Debug.Assert(method != null);
            Debug.Assert(factory != null);

            this.method = method;
            this.factory = factory;
        }

        /// <summary>
        /// Gets the <see cref="Discovery.Fixture"/> that defines the test.
        /// </summary>
        public Fixture Fixture => factory.Fixture;

        /// <summary>
        /// Gets the name of the test method.
        /// </summary>
        public string Name => method.Name;

        /// <summary>
        /// Overridden to display the name of the method.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;

        /// <summary>
        /// Runs the unit test, returning a <see cref="TestResult"/>.
        /// </summary>
        /// <returns></returns>
        public TestResult Run()
        {
            try
            {
                object fixture = factory.Build();
                Test test = method.Execute(fixture);

                return test.Passed ?
                    (TestResult)new PassResult(method.Name) :
                    new FailResult(method.Name, test);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return TestResult.UnexpectedException(this, ex);
            }
        }

        private sealed class PassResult : TestResult
        {
            private readonly string testName;

            public PassResult(string testName) : base(ResultKind.Pass) => this.testName = testName;

            public override string ToString()
            {
                return $"{testName}";
            }
        }

        private sealed class FailResult : TestResult
        {
            private readonly string testName;
            private readonly Test test;
            private const string indent = "   ";

            public FailResult(string testName, Test test) : base(ResultKind.Fail)
            {
                Debug.Assert(test != null);
                Debug.Assert(!test.Passed);

                this.testName = testName;
                this.test = test;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine(testName);
                var details = test.ToString().Split("\n")
                    .Select(line => $"{indent}{line}");
                foreach (var line in details)
                    sb.AppendLine(line);

                return sb.ToString().TrimEnd();
            }
        }
    }
}
