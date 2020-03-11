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
    [ExtensionUri(SUnitTestDiscoverer.ExecutorUri)]
    public class SUnitTestExecutor : ITestExecutor
    {
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            tests = SUnitTestDiscoverer.FindAllTestCases(Enumerable.Repeat(tests.First().Source, 1));

            try
            {
                foreach (var test in tests)
                {
                    var result = RunUnitTest(test, GetUnitTestFromTestCase(test));
                    frameworkHandle.RecordResult(result);
                }
            }
            catch (OperationCanceledException)
            {
                cancelSource = new CancellationTokenSource();
            }

        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var cases = SUnitTestDiscoverer.FindAllTestCases(sources);

            RunTests(cases, runContext, frameworkHandle);
        }

        public void Cancel()
        {
            cancelSource.Cancel();
        }

        private static UnitTest GetUnitTestFromTestCase(TestCase @case) => (UnitTest)@case.LocalExtensionData;
        private static VSResult RunUnitTest(TestCase @case, UnitTest test)
        {
            var sunitResult = test.Run();
            var result = new VSResult(@case);
            result.DisplayName = test.Name;
            result.Outcome = sunitResult.Kind switch
            {
                ResultKind.Pass => TestOutcome.Passed,
                ResultKind.Fail => TestOutcome.Failed,
                ResultKind.Error => TestOutcome.Failed,
                _ => TestOutcome.None
            };

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

        private CancellationTokenSource cancelSource = new CancellationTokenSource();
    }
}
