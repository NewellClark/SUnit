using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SUnit.Fixtures
{
    /// <summary>
    /// A single unit test that can be executed.
    /// </summary>
    public class UnitTest
    {
        private readonly TestMethod method;
        private readonly Factory factory;

        internal UnitTest(TestMethod method, Factory factory)
        {
            Debug.Assert(method != null);
            Debug.Assert(factory != null);

            this.method = method;
            this.factory = factory;
        }

        /// <summary>
        /// Gets the <see cref="SUnit.Fixtures.Fixture"/> that defines the test.
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
                return TestResult.UnexpectedException(ex);
            }
        }


        private sealed class PassResult : TestResult
        {
            private readonly string testName;

            public PassResult(string testName) : base(ResultKind.Pass) => this.testName = testName;

            public override string ToString()
            {
                return $"PASS {testName}";
            }
        }

        private sealed class FailResult : TestResult
        {
            private readonly string testName;
            private readonly Test test;
            private static readonly string indent = "   ";

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
                sb.AppendJoin(string.Empty, details);

                return sb.ToString();
            }
        }
    }
}
