using SUnit.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Runners
{
    internal class Runner
    {
        private readonly IEnumerable<IGrouping<string, TestCase>> groups;
        private const string indent = "   ";

        public Runner(string path)
        {
            Assembly assembly = Assembly.LoadFrom(path);
            var cases = Finder.FindAllTestCases(assembly.GetTypes());
            groups = cases
                .GroupBy(test => $"{test.Fixture.Name}${test.Factory}");
        }


        public void Run()
        {
            static void printTestCase(TestCase test)
            {
                var result = test.Run();
                var color = result.Passed ? ConsoleColor.Green : ConsoleColor.Red;
                string passFailText = result.Passed ? "PASS" : "FAIL";

                Console.ForegroundColor = color;
                Console.WriteLine($"{indent}{passFailText} {test.Name}");
                if (result.Passed)
                    return;
                var details = result.ToString().Split("\n");
                foreach (var line in details)
                    Console.WriteLine($"{indent}{indent}{line}");
            }

            foreach (var group in groups)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(group.Key);
                foreach (var test in group)
                    printTestCase(test);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }
    }
}
