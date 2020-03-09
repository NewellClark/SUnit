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
using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace SUnit.TestAdapter
{
    internal static class Variables
    {
        public static readonly Regex CompoundNamePattern = new Regex(@"(?<class>[^\|]*)\|\|(?<method>[^\|]*)");
        public const string Uri = @"executor://SUnitTestExecutor";
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
                .Select(source => (source, Assembly.Load(source)))
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

    [DefaultExecutorUri("executor://SUnitTestExecutor")]
    public class TestExecutor : ITestExecutor
    {
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            foreach (var testCase in tests)
            {
                var result = new TestResult(testCase);

                frameworkHandle.RecordResult(result);
            }
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var testCases = TestDiscoverer.FindAllTestCases(sources);

            RunTests(testCases, runContext, frameworkHandle);
        }

        public void Cancel()
        {
            //  Nope!
        }
    }
}
