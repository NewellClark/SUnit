using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using SUnit.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.TestAdapter
{
    [FileExtension(".dll")]
    [DefaultExecutorUri(ExecutorUri)]
    public class SUnitTestDiscoverer : ITestDiscoverer
    {
        internal const string ExecutorUri = "executor://SUnitTestExecutor";

        public void DiscoverTests(
            IEnumerable<string> sources, 
            IDiscoveryContext discoveryContext, 
            IMessageLogger logger, 
            ITestCaseDiscoverySink discoverySink)
        {
            IEnumerable<TestCase> cases = FindAllTestCases(sources);

            foreach (var @case in cases)
                discoverySink.SendTestCase(@case);
        }

        internal static IEnumerable<TestCase> FindAllTestCases(IEnumerable<string> sources)
        {
            return sources
                .Select(source => (source, unitTests: FindAllUnitTestsInAssembly(source)))
                .SelectMany(t => t.unitTests.Select(unitTest => CreateTestCaseFromUnitTest(t.source, unitTest)));
        }

        private static IEnumerable<UnitTest> FindAllUnitTestsInAssembly(string source)
        {
            Assembly assembly = Assembly.LoadFrom(source);
            var fixtures = assembly.GetTypes()
                .Where(type => type.IsPublic)
                .Select(type => new Fixture(type))
                .Where(fixture => fixture.Factories.Count > 0)
                .Where(fixture => fixture.Tests.Count > 0);

            return fixtures
                .SelectMany(fixture => fixture.Factories)
                .SelectMany(factory => factory.CreateTests());
        }

        private static TestCase CreateTestCaseFromUnitTest(string source, UnitTest unitTest)
        {
            TestCase @case = new TestCase();
            @case.ExecutorUri = new Uri(ExecutorUri);
            @case.Source = source;
            @case.LocalExtensionData = unitTest;
            @case.DisplayName = unitTest.Name;
            @case.FullyQualifiedName = $@"{unitTest.Fixture.Type.FullName}+{unitTest.Factory.Name}.{unitTest.Name}";
            
            return @case;
        }
    }
}
