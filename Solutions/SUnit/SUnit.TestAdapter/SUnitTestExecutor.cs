using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using SUnit.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VSResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;
using SUResult = SUnit.Discovery.Results.TestResult;
using SUnit.Discovery.Results;
using System.Threading;

namespace SUnit.TestAdapter
{
    [ExtensionUri(ExecutorUri)]
    internal class SUnitTestExecutor : ITestExecutor
    {
        internal const string ExecutorUri = "executor://SUnitTestExecutor";
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            isCancellationRequested = false;

            foreach (var test in tests)
            {
                VSResult result = RunUnitTest(test);
                frameworkHandle.RecordResult(result);
                if (isCancellationRequested)
                {
                    isCancellationRequested = false;
                    return;
                }
            }
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var testCases = SUnitTestDiscoverer.FindAllUnitTests(sources)
                .Select(unitTest => unitTest.ToTestCase());

            RunTests(testCases, runContext, frameworkHandle);
        }

        public void Cancel()
        {
            isCancellationRequested = true;
        }
        private bool isCancellationRequested = false;

        private static VSResult RunUnitTest(TestCase @case)
        {
            return RunUnitTest(@case, @case.ToUnitTest());
        }

        private static VSResult RunUnitTest(TestCase @case, UnitTest unitTest)
        {
            var sunitResult = TestRunner.RunTest(unitTest);
            var result = new VSResult(@case);
            result.DisplayName = unitTest.Name;
            result.Outcome = sunitResult.Kind.ToTestOutcome();

            switch (sunitResult)
            {
                case UnexpectedExceptionResult errorResult:
                    string heading = $"Unexpected {errorResult.Exception.GetType().Name}";
                    string message = IndentLines(errorResult.Exception.Message, "   ");
                    result.ErrorMessage = $"{heading}\n{message}";
                    result.ErrorStackTrace = errorResult.Exception.StackTrace;
                    break;

                case RanSuccessfullyResult ranResult when !ranResult.Result.Passed:
                    result.ErrorMessage = ranResult.Result.ToString();
                    break;

                case RanSuccessfullyResult ranResult when ranResult.Result.Passed:
                    break;

                default:
                    result.ErrorMessage = $"{sunitResult.Kind}: {sunitResult}";
                    break;
            }

            return result;
        }


        private static string IndentLines(string input, string indent)
        {
            var lines = input.Split("\n");
            var sb = new StringBuilder();
            foreach (var line in lines)
                sb.AppendLine($"{indent}{line}");

            return sb.ToString();
        }
    }
}
