using SUnit.DiscoveryOLD.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Discovery
{
    internal static class TestRunner
    {
        public static TestResult RunTest(UnitTest unitTest)
        {
            if (unitTest is null) throw new ArgumentNullException(nameof(unitTest));

            try
            {
                Test outcome = unitTest.Execute();
                return new RanSuccessfullyResult(unitTest.Name, outcome);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new UnexpectedExceptionResult(unitTest.Name, ex);
            }
        }
    }
}
