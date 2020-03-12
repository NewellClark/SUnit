using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Discovery
{
    internal static class TestRunner
    {
        public static TestResult RunTest(Func<Test> testMethod, string testName)
        {
            if (testMethod is null) throw new ArgumentNullException(nameof(testMethod));

            try
            {
                Test outcome = testMethod();
                return new RanSuccessfullyResult(testName, outcome);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new UnexpectedExceptionResult(testName, ex);
            }
        }

        public static TestResult RunTest(UnitTest unitTest)
        {
            return RunTest(unitTest.Execute, unitTest.Name);
        }
    }
}
