using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using System.Linq;

namespace SUnit.Discovery
{
    internal interface ITestKind
    {
        public bool IsReturnTypeValid(Type returnType);

        public IObservable<TestResult> Run(UnitTest unitTest);
    }

    internal class SingletonTestKind : ITestKind
    {
        public bool IsReturnTypeValid(Type returnType)
        {
            if (returnType is null) throw new ArgumentNullException(nameof(returnType));

            return typeof(Test).IsAssignableFrom(returnType);
        }

        public IObservable<TestResult> Run(UnitTest unitTest)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));
            if (!IsReturnTypeValid(unitTest.ReturnType))
                throw new ArgumentException(
                    $"Invalid type passed to {nameof(SingletonTestKind)}. Got {unitTest.ReturnType.FullName}.",
                    nameof(unitTest));

            Func<object> method;
            
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                method = unitTest.CreateDelegate();
            }
            catch (Exception ex)
            {
                return Observable.Throw<TestResult>(ex);
            }

            Test outcome;

            try
            {
                outcome = (Test)method();
            }
            catch (Exception ex)
            {
                var errorResult = new UnexpectedExceptionResult(unitTest, ex);
                return Observable.Return(errorResult);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            var ranResult = new RanSuccessfullyResult(unitTest, outcome);
            return Observable.Return(ranResult);
        }
    }

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
                    foreach (Test test in outcome)
                        observer.OnNext(new RanSuccessfullyResult(unitTest, test));
                }
                catch (Exception ex)
                {
                    var errorResult = new UnexpectedExceptionResult(unitTest, ex);
                    observer.OnNext(errorResult);
                    return emptyAction;
                }

                observer.OnCompleted();
                return emptyAction;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return Observable.Create<TestResult>(onSubscribe);
        }
    }
}
