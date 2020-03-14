using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace SUnit.Discovery
{
    internal class AsyncTestKind : ITestKind
    {
        public bool IsReturnTypeValid(Type returnType)
        {
            if (returnType is null) throw new ArgumentNullException(nameof(returnType));

            return typeof(Task<Test>).IsAssignableFrom(returnType);
        }
        
        public IObservable<TestResult> Run(UnitTest unitTest)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));
            if (!IsReturnTypeValid(unitTest.ReturnType))
                throw new ArgumentException($"Invalid return type for {nameof(AsyncTestKind)}. Got {unitTest.ReturnType.FullName}.");

            async Task<TestResult> runAsync()
            {
                Func<object> method = unitTest.CreateDelegate();

#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    var task = (Task<Test>)method();
                    Test outcome = await task.ConfigureAwait(false);
                    return new RanSuccessfullyResult(unitTest, outcome);
                }

                catch (Exception ex)

                {
                    return new UnexpectedExceptionResult(unitTest, ex);
                }
#pragma warning restore CA1031 // Do not catch general exception types

            }

            return Observable.FromAsync(runAsync);
        }
    }
}
