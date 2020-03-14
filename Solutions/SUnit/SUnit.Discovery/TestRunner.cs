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
    /// <summary>
    /// Contains methods for running <see cref="UnitTest"/>s. 
    /// </summary>
    internal class TestRunner
    {
        /// <summary>
        /// Runs the specified <see cref="UnitTest"/>. 
        /// </summary>
        /// <param name="unitTest">The <see cref="UnitTest"/> to run.</param>
        /// <returns>An <see cref="IObservable{T}"/> that produces the <see cref="TestResult"/>s from running the test. 
        /// The obervable will complete once all test results are finished.</returns>
        /// <remarks>
        /// For single tests, the observable will yield a single result and then complete. If a test throws an exception,
        /// the returned <see cref="TestResult"/> will be an <see cref="UnexpectedExceptionResult"/>, but the 
        /// observable will still complete normally (<see cref="IObserver{T}.OnNext(T)"/> will be called, rather
        /// than <see cref="IObserver{T}.OnError(Exception)"/>).
        /// </remarks>
        public static IObservable<TestResult> RunTest(UnitTest unitTest)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));

            ITestKind testKind = Rules.GetTestKind(unitTest.ReturnType);

            if (testKind is null)
                throw new ArgumentException($"Invalid return type on test method. Got {unitTest.ReturnType.FullName}.",
                    nameof(unitTest));

            return testKind.Run(unitTest);
        }

        /// <summary>
        /// Runs all the <see cref="UnitTest"/>s. 
        /// </summary>
        /// <param name="unitTests">The <see cref="UnitTest"/>s to run.</param>
        /// <returns>
        /// An <see cref="IObservable{T}"/> that produces the results of running all the specified unit tests.
        /// </returns>
        public static IObservable<TestResult> RunTests(IEnumerable<UnitTest> unitTests)
        {
            if (unitTests is null) throw new ArgumentNullException(nameof(unitTests));

            return unitTests
                .Select(test => RunTest(test))
                .Aggregate((x, y) => x.Merge(y));
        }
    }
}
