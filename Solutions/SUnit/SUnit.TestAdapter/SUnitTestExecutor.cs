using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using SUnit.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VSResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;
using SUResult = SUnit.Discovery.Results.TestResult;
using SUnit.Discovery.Results;
using System.Threading;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Threading.Tasks;

namespace SUnit.TestAdapter
{
    [ExtensionUri(ExecutorUri)]
    internal class SUnitTestExecutor : ITestExecutor
    {
        internal const string ExecutorUri = "executor://SUnitTestExecutor";

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var testCaseLookup = tests
                .ToDictionary(test => test.ToUnitTest(), test => test);
            cts = new CancellationTokenSource();

            try
            {
                var resultObservable = TestRunner.RunTests(testCaseLookup.Select(kvp => kvp.Key));
                resultObservable.Do(sunitResult =>
                {
                    UnitTest unitTest = sunitResult.UnitTest;
                    TestCase testCase = testCaseLookup[unitTest];
                    VSResult vsResult = ConvertToVsResult(testCase, unitTest, sunitResult);
                    frameworkHandle.RecordResult(vsResult);
                }).ToTask()
                .Wait(cts.Token);
            }
            catch (Exception error)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, $"Unexpected {error.GetType().FullName}");
                frameworkHandle.SendMessage(TestMessageLevel.Error, error.Message);
                frameworkHandle.SendMessage(TestMessageLevel.Error, error.StackTrace);
            }
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var testCases = SUnitTestDiscoverer.FindAllUnitTests(sources)
                .Select(unitTest => unitTest.ToTestCase());

            RunTests(testCases, runContext, frameworkHandle);
        }

        public void Cancel() => cts?.Cancel();
        private CancellationTokenSource cts;

        private static VSResult ConvertToVsResult(TestCase @case, UnitTest unitTest, SUResult sunitResult)
        {
            var result = new VSResult(@case)
            {
                DisplayName = unitTest.Name,
                Outcome = sunitResult.Kind.ToTestOutcome()
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
    }
}
