using NUnit.Framework;
using SUnit.Discovery;
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
        public class AsyncEnumerableTests
        {
            private class Mock
            {
                private static readonly TimeSpan longDelay = TimeSpan.FromMilliseconds(700);

                public IAsyncEnumerable<Test> ThrowsWithoutIteratingSync()
                {
                    throw new ExpectedException();
                }

                public async IAsyncEnumerable<Test> ThrowsWithoutIteratingAsync()
                {
                    await Task.Yield();
                    throw new ExpectedException();
                    yield return null;
                }

                public async IAsyncEnumerable<Test> ThrowsWithoutIteratingLongAsync()
                {
                    await Task.Delay(longDelay);
                    throw new ExpectedException();
                    yield return null;
                }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                public async IAsyncEnumerable<Test> NoThrowSync()
                {
                    yield return Test.Pass;
                    yield return Test.Fail;
                    yield return Test.Pass;
                }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

                public async IAsyncEnumerable<Test> NoThrowAsync()
                {
                    await Task.Yield();
                    yield return Test.Pass;

                    await Task.Yield();
                    yield return Test.Fail;

                    await Task.Yield();
                    yield return Test.Pass;
                }

                public async IAsyncEnumerable<Test> NoThrowLongAsync()
                {
                    await Task.Delay(longDelay);
                    yield return Test.Pass;

                    await Task.Delay(longDelay);
                    yield return Test.Fail;

                    await Task.Delay(longDelay);
                    yield return Test.Pass;
                }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                public async IAsyncEnumerable<Test> ThrowsAfterThreeSync()
                {
                    yield return Test.Pass;
                    yield return Test.Fail;
                    yield return Test.Pass;

                    throw new ExpectedException();
                }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

                public async IAsyncEnumerable<Test> ThrowsAfterThreeAsync()
                {
                    await Task.Yield();
                    yield return Test.Pass;

                    await Task.Yield();
                    yield return Test.Fail;

                    await Task.Yield();
                    yield return Test.Pass;

                    throw new ExpectedException();
                }
            }

            private IObservable<ResultKind> RunAsync(string name)
            {
                var fixture = new Fixture(typeof(Mock));
                var factory = fixture.Factories.Single();
                var unitTest = factory.CreateTests().Single(u => u.Name == name);

                return TestRunner.RunTest(unitTest)
                    .Select(r => r.Kind);
            }

            [TestCase(nameof(Mock.ThrowsWithoutIteratingSync))]
            [TestCase(nameof(Mock.ThrowsWithoutIteratingAsync))]
            [TestCase(nameof(Mock.ThrowsWithoutIteratingLongAsync))]
            public async Task ThrowsWithoutIterating(string name)
            {
                var result = await RunAsync(name)
                    .ToList();

                CollectionAssert.AreEqual(Enumerable.Repeat(ResultKind.Error, 1), result);
            }

            [TestCase(nameof(Mock.NoThrowSync))]
            [TestCase(nameof(Mock.NoThrowAsync))]
            [TestCase(nameof(Mock.NoThrowLongAsync))]
            public async Task DoesNotThrow(string name)
            {
                var actual = await RunAsync(name)
                    .ToList();

                var expected = new[] { ResultKind.Pass, ResultKind.Fail, ResultKind.Pass };

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestCase(nameof(Mock.ThrowsAfterThreeSync))]
            [TestCase(nameof(Mock.ThrowsAfterThreeAsync))]
            public async Task ThrowsAfterThree(string name)
            {
                var actual = await RunAsync(name)
                    .ToList();

                var expected = new[] { ResultKind.Pass, ResultKind.Fail, ResultKind.Pass, ResultKind.Error };

                CollectionAssert.AreEqual(expected, actual);
            }

            public Test DoesAnalyzerWork()
            {
                Assert.That(2 + 2).Is.EqualTo(4);
                return Test.Pass;
            }
        }
    }
}
