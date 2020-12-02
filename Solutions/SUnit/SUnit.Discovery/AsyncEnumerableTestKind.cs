using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUnit.Discovery
{
    internal class AsyncEnumerableTestKind : ITestKind
    {
        public bool IsReturnTypeValid(Type returnType)
        {
            if (returnType is null) throw new ArgumentNullException(nameof(returnType));

            return typeof(IAsyncEnumerable<Test>).IsAssignableFrom(returnType);
        }

        public IObservable<TestResult> Run(UnitTest unitTest)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));
            if (!IsReturnTypeValid(unitTest.ReturnType))
                throw new ArgumentException(
                    $"Invalid return type. Got {unitTest.ReturnType.FullName}, expected implimentation of {typeof(IAsyncEnumerable<Test>).Name}", nameof(unitTest));

            async Task subscribeAsync(IObserver<TestResult> observer)
            {
                try
                {
                    object fixture = unitTest.InstantiateFixture();
                    Func<object> method;

                    //  Exceptions caused by attempting to create the delegate for the test method indicate bugs in SUnit, and should not be trapped.
                    try
                    {
                        method = unitTest.CreateDelegate(fixture);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        return;
                    }

                    var outcome = (IAsyncEnumerable<Test>)method();

                    if (outcome is null)
                    {
                        observer.OnNext(new InvalidTestResult(unitTest, "Test methods may not return null async-enumerables."));
                        observer.OnCompleted();
                        return;
                    }

                    await foreach (Test test in outcome)
                    {
                        if (test is null)
                        {
                            observer.OnNext(new InvalidTestResult(unitTest, "Async-enumerable test methods may not return null elements."));
                            continue;
                        }
                        observer.OnNext(new RanSuccessfullyResult(unitTest, test));
                    }
                }
                catch (Exception ex)
                {
                    var errorResult = new UnexpectedExceptionResult(unitTest, ex);
                    observer.OnNext(errorResult);
                }

                observer.OnCompleted();
            }

            return Observable.Create<TestResult>(subscribeAsync);
        }
    }
}
