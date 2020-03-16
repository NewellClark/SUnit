using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;

namespace SUnit.Discovery
{
    internal class MultiTestKind : ITestKind
    {
        public bool IsReturnTypeValid(Type returnType)
        {
            if (returnType is null) throw new ArgumentNullException(nameof(returnType));

            return typeof(IEnumerable<Test>).IsAssignableFrom(returnType);
        }
        
        public IObservable<TestResult> Run(UnitTest unitTest)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));
            if (!IsReturnTypeValid(unitTest.ReturnType))
                throw new ArgumentException($"Invalid return type. Got {unitTest.ReturnType.FullName}", nameof(unitTest));

            static void emptyAction() { }

#pragma warning disable CA1031 // Do not catch general exception types
            Action onSubscribe(IObserver<TestResult> observer)
            {
                Func<object> method;

                try
                {
                    method = unitTest.CreateDelegate();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return emptyAction;
                }

                try
                {
                    var outcome = (IEnumerable<Test>)method();
                    if (outcome is null)
                    {
                        observer.OnNext(new InvalidTestResult(unitTest, "Multi-test methods may not return null enumerables."));
                        observer.OnCompleted();

                        return emptyAction;
                    }

                    foreach (Test test in outcome)
                    {
                        if (test is null)
                        {
                            observer.OnNext(new InvalidTestResult(unitTest, $"Multi-test methods may not return null elements."));
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
                return emptyAction;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return Observable.Create<TestResult>(onSubscribe);
        }
    }
}
