using SUnit.Discovery;
using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Runners
{
    internal class Runner
    {
        private const string indent = "   ";
        private readonly IEnumerable<Fixture> fixtures;
        private readonly ConsoleColor passColor = ConsoleColor.Green;
        private readonly ConsoleColor failColor = ConsoleColor.Red;
        private readonly ConsoleColor errorColor = ConsoleColor.Magenta;

        public Runner(string path)
        {
            Assembly assembly = Assembly.LoadFrom(path);
            fixtures = assembly.GetExportedTypes()
                .Select(type => new Fixture(type))
                .Where(fixture => fixture.Tests.Count > 0)
                .Where(fixture => fixture.Factories.Count > 0);
        }

        public void Run()
        {
            foreach (var fixture in fixtures)
            {
                PrintFixtureName(fixture);
                RunAndPrintFixtureTests(fixture);
            }
        }

        private void PrintFixtureName(Fixture fixture)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(fixture.Type.Name);

        }

        private void RunAndPrintFixtureTests(Fixture fixture)
        {
            if (fixture.Factories.Count == 1)
                PrintFactoryGroup(fixture.Factories.Single(), string.Empty);
            else
            {
                foreach (var factory in fixture.Factories)
                    PrintFactoryGroup(factory, indent);
            }
        }

        private void PrintFactoryGroup(Factory factory, string margin)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{margin}{factory.ToString()}");

            foreach (var test in factory.CreateTests())
            {
                var result = TestRunner.RunTest(test);
                Console.ForegroundColor = GetColorForResult(result.Kind);
                var resultLines = $"{result.Kind.ToString().ToUpper()} {result}".Split("\n");
                foreach (var line in resultLines)
                    Console.WriteLine($"{indent}{margin}{line}");
            }
        }

        private ConsoleColor GetColorForResult(ResultKind result)
        {
            return result switch
            {
                ResultKind.Pass => passColor,
                ResultKind.Fail => failColor,
                ResultKind.Error => errorColor,
                _ => throw new ArgumentOutOfRangeException(nameof(result), "Unexpected enum value"),
            };
        }
    }
}
