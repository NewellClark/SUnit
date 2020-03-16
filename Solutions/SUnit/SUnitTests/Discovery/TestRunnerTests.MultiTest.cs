using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nAssert = NUnit.Framework.Assert;
using System.Reactive.Linq;
using SUnit.Discovery.Results;
using System.Reactive;

namespace SUnit.Discovery
{
    [TestFixture]
    public partial class TestRunnerTests
    {
        [TestFixture]
        public class MultiTests
        {
            private class Mock
            {
                public IEnumerable<Test> EmptyMultiTest() => Enumerable.Empty<Test>();

                public IEnumerable<Test> ThrowsImmediately() => throw new ExpectedException();

                public IEnumerable<Test> ThrowsOnFirstMoveNext()
                {
                    //  We want to defer throwing until MoveNext() is called for the first time.
                    //  Using a non-constant value avoids compiler warnings.
                    if (DateTime.Now.Second * 0 == 0) 
                        throw new ExpectedException();

                    yield return Test.Pass;
                }

                public IEnumerable<Test> SingletonPass()
                {
                    yield return Test.Pass;
                }

                public IEnumerable<Test> ThrowsAfterThree()
                {
                    yield return Test.Fail;
                    yield return Test.Pass;
                    yield return Test.Fail;

                    throw new ExpectedException();
                }

                public IEnumerable<Test> CompletesNormallyAfterThree()
                {
                    yield return Test.Pass;
                    yield return Test.Fail;
                    yield return Test.Pass;
                }

                public IEnumerable<Test> ReturnsNullSequence() => null;

                public IEnumerable<Test> ReturnsNullFirstElement()
                {
                    yield return null;
                    yield return Test.Pass;
                    yield return Test.Fail;
                }

                public IEnumerable<Test> ReturnsNullMiddleElement()
                {
                    yield return Test.Fail;
                    yield return null;
                    yield return Test.Pass;
                }
            }

            private UnitTest Get(string name)
            {
                var fixture = new Fixture(typeof(Mock));
                var factory = fixture.Factories.Single();

                return factory.CreateTests()
                    .Where(test => test.Name == name)
                    .Single();
            }

            private IObservable<TestResult> Run(string name) => TestRunner.RunTest(Get(name));


            [Test]
            public async Task EmptySequence_producesNoResults()
            {
                bool empty = await Run(nameof(Mock.EmptyMultiTest)).IsEmpty();

                nAssert.That(empty, Is.True);
            }

            [Test]
            public async Task ThrowsImmediately_ReturnsSingleErrorResult()
            {
                var result = await Run(nameof(Mock.ThrowsImmediately)).SingleAsync();

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Error));
            }

            [Test]
            public async Task ThrowsOnFirstMoveNext_ReturnsSingleErrorResult()
            {
                var result = await Run(nameof(Mock.ThrowsOnFirstMoveNext));

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Error));
            }

            [Test]
            public async Task SingletonPass_ReturnsSinglePassingResult()
            {
                var result = await Run(nameof(Mock.SingletonPass));

                nAssert.That(result.Kind, Is.EqualTo(ResultKind.Pass));
            }

            [Test]
            public async Task ThrowsAfterThree_ReturnsFirstThreeElementsBeforeThrowing()
            {
                var result = await Run(nameof(Mock.ThrowsAfterThree))
                    .Scan(new List<TestResult>(), (list, testResult) =>
                    {
                        list.Add(testResult);
                        return list;
                    });

                bool[] wasErrorExpected = { false, false, false, true };
                var wasErrorActual = result
                    .Select(tr => tr.Kind == ResultKind.Error);

                CollectionAssert.AreEqual(wasErrorExpected, wasErrorActual);
            }

            [Test]
            public async Task CompletesNormallyAfterThree_ReturnsThreeThenCompletes()
            {
                var tcs = new TaskCompletionSource<Unit>();
                int count = 0;
                Run(nameof(Mock.CompletesNormallyAfterThree)).Subscribe(
                    _ => count++,
                    error => throw error,
                    () => tcs.SetResult(Unit.Default));

                await tcs.Task;

                nAssert.That(count, Is.EqualTo(3));
            }

            [Test]
            public async Task ReturnsNullSequence_YieldsSingleInvalidTestResult()
            {
                var result = await Run(nameof(Mock.ReturnsNullSequence)).SingleAsync();

                nAssert.That(result, Is.InstanceOf<InvalidTestResult>());
            }

            [Test]
            public async Task FirstElementIsNull_YieldsNullResultForNullElement()
            {
                var resultTypes = await Run(nameof(Mock.ReturnsNullFirstElement))
                    .Select(tr => tr.GetType())
                    .ToList()
                    .SingleAsync();

                var invalid = typeof(InvalidTestResult);
                var successful = typeof(RanSuccessfullyResult);
                var expected = new[] { invalid, successful, successful };

                CollectionAssert.AreEqual(expected, resultTypes);
            }

            [Test]
            public async Task MiddleElementIsNull_YieldsInvalidMiddleResult()
            {
                var resultTypes = await Run(nameof(Mock.ReturnsNullMiddleElement))
                    .Select(tr => tr.GetType())
                    .ToList()
                    .SingleAsync();

                var invalid = typeof(InvalidTestResult);
                var successful = typeof(RanSuccessfullyResult);
                var expected = new[] { successful, invalid, successful };

                CollectionAssert.AreEqual(expected, resultTypes);
            }
        }
    }
}
