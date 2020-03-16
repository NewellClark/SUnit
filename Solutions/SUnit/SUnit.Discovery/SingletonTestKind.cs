using SUnit.Discovery.Results;
using System;
using System.Reactive.Linq;

namespace SUnit.Discovery
{
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

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                object fixture = unitTest.InstantiateFixture();
                Func<object> method;

                try
                {
                    method = unitTest.CreateDelegate(fixture);
                }
                catch (Exception ex)
                {
                    return Observable.Throw<TestResult>(ex);
                }

                Test outcome = (Test)method();
                TestResult testResult = outcome is null ?
                    new InvalidTestResult(unitTest, $"Test methods may not return null.") as TestResult :
                    new RanSuccessfullyResult(unitTest, outcome);

                return Observable.Return(testResult);
            }
            catch (Exception ex)
            {
                return Observable.Return(new UnexpectedExceptionResult(unitTest, ex));
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
