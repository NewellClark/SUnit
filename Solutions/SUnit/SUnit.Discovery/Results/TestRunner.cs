using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace SUnit.Discovery
{
    internal class TestRunner
    {
        public static IObservable<TestResult> RunTest(UnitTest unitTest)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));

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
                return Observable.Return(new UnexpectedExceptionResult(unitTest, ex));
            }

            Debug.Assert(outcome != null);

            switch (outcome)
            {
                case IEnumerable<Test> multi:
                    return RunMultiTest(unitTest, multi);
                case Test single:
                    return HandleSingleOutcome(unitTest, single);
                default:
                    Debug.Fail($"Unexpected test outcome type. Got {outcome.GetType().FullName}");
                    return Observable.Empty<TestResult>();
            }
        }

        public static IObservable<TestResult> RunTests(IEnumerable<UnitTest> unitTests)
        {
            if (unitTests is null) throw new ArgumentNullException(nameof(unitTests));

            return unitTests
                .Select(test => RunTest(test))
                .Aggregate((x, y) => x.Merge(y));
        }

        private static IObservable<TestResult> HandleSingleOutcome(UnitTest unitTest, Test outcome)
        {
            return Observable.Return(new RanSuccessfullyResult(unitTest, outcome));
        }
        
        private static IObservable<TestResult> RunMultiTest(UnitTest unitTest, IEnumerable<Test> outcome)
        {
            Debug.Assert(unitTest != null);
            Debug.Assert(outcome != null);

            return Observable.Create<TestResult>(o =>
            {
                try
                {
                    foreach (Test test in outcome)
                        o.OnNext(new RanSuccessfullyResult(unitTest, test));
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    o.OnNext(new UnexpectedExceptionResult(unitTest, ex));
                }

                o.OnCompleted();

                return () => { };
            });
        }
    }
}
