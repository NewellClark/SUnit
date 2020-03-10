using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using SUnit.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VSResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;
using SunitResult = SUnit.Discovery.TestResult;
using System.Threading;

namespace SUnit.TestAdapter
{
    internal static class Variables
    {
        public static readonly Regex CompoundNamePattern = new Regex(@"(?<class>[^\|]*)\|\|(?<method>.*)");
        public const string Uri = "executor://SUnitTestExecutor";
    }

    [FileExtension(".dll")]
    [DefaultExecutorUri("executor://SUnitTestExecutor")]
    public class TestDiscoverer : ITestDiscoverer
    {

        public void DiscoverTests(
            IEnumerable<string> sources, 
            IDiscoveryContext discoveryContext, 
            IMessageLogger logger, 
            ITestCaseDiscoverySink discoverySink)
        {
            var testCases = FindAllTestCases(sources);
            foreach (var test in testCases)
                discoverySink.SendTestCase(test);
        }

        internal static IEnumerable<(string source, Fixture fixture)> FindAllFixtures(IEnumerable<string> sources)
        {
            return sources
                .Select(source => (source, Assembly.LoadFrom(source)))
                .Select(t => (t.source, t.Item2.GetTypes().Where(type => type.IsPublic)))
                .SelectMany(t => t.Item2.Select(type => (t.source, type)))
                .Select(t => (t.source, new Fixture(t.type)))
                .Where(t => t.Item2.Factories.Count > 0)
                .Where(t => t.Item2.Tests.Count > 0);
        }

        internal static IEnumerable<TestMethod> FindAllTestMethods(IEnumerable<Fixture> fixtures)
        {
            return fixtures.SelectMany(fixture => fixture.Tests);
        }

        internal static IEnumerable<TestCase> FindAllTestCases(IEnumerable<string> sources)
        {
            var fixtures = FindAllFixtures(sources);
            return fixtures
                .Select(t => (t.source, t.fixture.Tests))
                .SelectMany(t => t.Tests.Select(test => (t.source, test)))
                .Select(t => (t.source, $@"{t.test.Fixture.Type.FullName}||{t.test.Method.Name}"))
                .Select(t => new TestCase(t.Item2, new Uri(Variables.Uri), t.source));
        }
    }

    [ExtensionUri("executor://SUnitTestExecutor")]
    public class TestExecutor : ITestExecutor
    {
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            cancelSource = new CancellationTokenSource();

            try
            {
                var assemblyLookup = new Dictionary<string, Assembly>();
                var fixtureLookup = new Dictionary<(string assembly, string @class), Fixture>();
                var testLookup = new Dictionary<(Fixture fixture, string methodName), UnitTest>();

                static string getLookupName(Fixture fixture, string methodName)
                {
                    return $@"{fixture.Type.FullName}||{methodName}";
                }
                UnitTest getUnitTestFromTestCase(TestCase @case)
                {
                    if (!assemblyLookup.ContainsKey(@case.Source))
                        assemblyLookup.Add(@case.Source, Assembly.LoadFrom(@case.Source));
                    Assembly assembly = assemblyLookup[@case.Source];

                    var match = Variables.CompoundNamePattern.Match(@case.FullyQualifiedName);

                    (string source, string type) tuple = (@case.Source, match.Groups["class"].Value);
                    if (!fixtureLookup.ContainsKey(tuple))
                    {
                        var newFixture = new Fixture(assembly.GetType(tuple.type));
                        fixtureLookup.Add(tuple, newFixture);
                        var factory = newFixture.Factories.First();

                        foreach (var unitTest in factory.CreateTests())
                        {
                            cancelSource.Token.ThrowIfCancellationRequested();
                            string lookupName = getLookupName(newFixture, unitTest.Name);
                            testLookup.Add((newFixture, lookupName), unitTest);
                        }
                    }

                    var fixture = fixtureLookup[tuple];
                    return testLookup[(fixture, @case.FullyQualifiedName)];
                }

                foreach (var @case in tests)
                {
                    cancelSource.Token.ThrowIfCancellationRequested();

                    UnitTest test = getUnitTestFromTestCase(@case);
                    VSResult result = RunUnitTest(@case, test);
                    frameworkHandle.RecordResult(result);
                }
            }
            catch (OperationCanceledException) { }
        }

        private static TestOutcome FromSUnitResultKind(ResultKind kind) => kind switch
        {
            ResultKind.Error => TestOutcome.Failed,
            ResultKind.Fail => TestOutcome.Failed,
            ResultKind.Pass => TestOutcome.Passed,
            _ => TestOutcome.None
        };

        private static VSResult RunUnitTest(TestCase @case, UnitTest test)
        {
            var sunitResult = test.Run();
            var result = new VSResult(@case)
            {
                DisplayName = test.Name,
                Outcome = FromSUnitResultKind(sunitResult.Kind)
            };

            switch (sunitResult.Kind)
            {
                case ResultKind.Fail:
                case ResultKind.Error:
                    result.ErrorMessage = sunitResult.ToString();
                    break;
            }

            return result;
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var testCases = TestDiscoverer.FindAllTestCases(sources);

            RunTests(testCases, runContext, frameworkHandle);
        }

        public void Cancel()
        {
            cancelSource?.Cancel();
        }

        private CancellationTokenSource cancelSource;
    }
}
