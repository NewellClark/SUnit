using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace SUnit.Discovery
{
    internal class TestRunner
    {

        public static IObservable<TestResult> RunTest(UnitTest unitTest)
        {
            var testMethod = unitTest.CreateDelegate();
            object outcome;

            try
            {
                outcome = testMethod();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return Observable.Return(new UnexpectedExceptionResult(unitTest.Name, ex));
            }

            switch (outcome)
            {
                case IEnumerable<Test> multi:
                    return HandleMultiTest(multi);
                case Test single:
                    return HandleSingleOutcome(unitTest, single);
                default:
                    Debug.Fail($"Unexpected test outcome type. Got {outcome.GetType().FullName}");
                    return Observable.Empty<TestResult>();
            }
        }

        private static IObservable<TestResult> HandleSingleOutcome(UnitTest unitTest, Test outcome)
        {
            return Observable.Return(new RanSuccessfullyResult(unitTest.Name, outcome));
        }

        private static IObservable<TestResult> HandleMultiTest(IEnumerable<Test> outcome)
        {
            throw new NotImplementedException();
        }
    }
}
