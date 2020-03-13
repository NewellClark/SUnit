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
    [DefaultExecutorUri(SUnitTestExecutor.ExecutorUri)]
    internal class SUnitTestDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(
            IEnumerable<string> sources, 
            IDiscoveryContext discoveryContext, 
            IMessageLogger logger, 
            ITestCaseDiscoverySink discoverySink)
        {
            var unitTests = FindAllUnitTests(sources);
            var cases = unitTests.Select(u => u.ToTestCase());

            foreach (var @case in cases)
                discoverySink.SendTestCase(@case);
        }

        internal static IEnumerable<UnitTest> FindAllUnitTests(IEnumerable<string> sources)
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));

            return sources.Select(source => Assembly.LoadFrom(source))
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Select(type => new Fixture(type))
                .SelectMany(fixture => fixture.Factories)
                .SelectMany(factory => factory.CreateTests());
        }
    }
}
