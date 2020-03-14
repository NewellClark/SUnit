using NUnit.Framework;
using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using nAssert = NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    partial class TestRunnerTests
    {
        [TestFixture]
        public class SingleTest
        {
            private class Mock
            {
                public Test Throws() => throw new ExpectedException();

                public Test Passes() => Test.Pass;

                public Test Fails() => Test.Fail;
            }

            private IObservable<TestResult> Run(string name)
            {
                var fixture = new Fixture(typeof(Mock));
                var factory = fixture.Factories.Single();
                var unitTest = factory.CreateTests()
                    .Where(test => test.Name == name)
                    .Single();

                return TestRunner.RunTest(unitTest);
            }

            [Test]
            public async Task ThrowingTest_YieldsErrorResult()
            {
                var result = await Run(nameof(Mock.Throws));

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Error));
            }

            [Test]
            public async Task PassingTest_YieldsPassingResult()
            {
                var result = await Run(nameof(Mock.Passes));

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Pass));
            }

            [Test]
            public async Task FailingTest_YieldsFailingResult()
            {
                var result = await Run(nameof(Mock.Fails));

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Fail));
            }
        }
    }
}
