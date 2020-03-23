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
    [TestFixture]
    partial class TestRunnerTests
    {
        [TestFixture]
        public class AsyncTest
        {
            private class Mock
            {
                private readonly TimeSpan longAsyncDuration = TimeSpan.FromMilliseconds(300);

                public Task<Test> SyncPass() => Task.FromResult(Test.Pass);

                public Task<Test> SyncFail() => Task.FromResult(Test.Fail);

                public Task<Test> SyncError() => Task.FromException<Test>(new ExpectedException());

                public async Task<Test> ShortAsyncPass()
                {
                    await Task.Yield();
                    return Test.Pass;
                }

                public async Task<Test> ShortAsyncFail()
                {
                    await Task.Yield();
                    return Test.Fail;
                }

                public async Task<Test> ShortAsyncError()
                {
                    await Task.Yield();
                    throw new ExpectedException();
                }

                public async Task<Test> LongAsyncPass()
                {
                    await Task.Delay(longAsyncDuration);
                    return Test.Pass;
                }

                public async Task<Test> LongAsyncFail()
                {
                    await Task.Delay(longAsyncDuration);
                    return Test.Fail;
                }

                public async Task<Test> LongAsyncError()
                {
                    await Task.Delay(longAsyncDuration);
                    throw new ExpectedException();
                }

                public Task<Test> SyncReturnNullTask() => null;
                public Task<Test> SyncReturnTaskWithNullTest() => Task.FromResult<Test>(null);
                public async Task<Test> AsyncReturnTaskWithNullTest()
                {
                    await Task.Yield();

                    return null;
                }
            }


            private async Task<TestResult> RunAsync(string methodName)
            {
                var fixture = new Fixture(typeof(Mock));
                var factory = fixture.Factories.Single();
                var unitTest = factory.CreateTests().Where(test => test.Name == methodName).Single();

                return await TestRunner.RunTest(unitTest);
            }

            [TestCase(nameof(Mock.SyncPass))]
            [TestCase(nameof(Mock.ShortAsyncPass))]
            [TestCase(nameof(Mock.LongAsyncPass))]
            public async Task PassResults(string methodName)
            {
                var result = await RunAsync(methodName);

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Pass));
            }

            [TestCase(nameof(Mock.SyncFail))]
            [TestCase(nameof(Mock.ShortAsyncFail))]
            [TestCase(nameof(Mock.LongAsyncFail))]
            public async Task FailResults(string methodName)
            {
                var result = await RunAsync(methodName);

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Fail));
            }

            [TestCase(nameof(Mock.SyncError))]
            [TestCase(nameof(Mock.ShortAsyncError))]
            [TestCase(nameof(Mock.LongAsyncError))]
            public async Task ErrorResults(string methodName)
            {
                var result = await RunAsync(methodName);

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Error));
            }

            [Test]
            public async Task NullTaskReturningMethod_YieldsInvalidTestResult()
            {
                var result = await RunAsync(nameof(Mock.SyncReturnNullTask));

                nAssert.That(result, Is.InstanceOf<InvalidTestResult>());
            }

            [TestCase(nameof(Mock.SyncReturnTaskWithNullTest))]
            [TestCase(nameof(Mock.AsyncReturnTaskWithNullTest))]
            public async Task MethodReturnsTaskThatReturnsNullTest_YieldsInvalidTestResult(string name)
            {
                var result = await RunAsync(name);

                nAssert.That(result, Is.InstanceOf<InvalidTestResult>());
            }
        }
    }
}
