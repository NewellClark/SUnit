using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using SUnit.Discovery;
using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SUnit.TestAdapter
{
    /// <summary>
    /// A light-weight version of <see cref="UnitTest"/>. 
    /// </summary>
    internal struct LiteTest
    {
        private static readonly TestProperty LinesProperty =
            TestProperty.Register(
                nameof(LinesProperty), nameof(LinesProperty),
                typeof(string[]), typeof(TestCase));

        public static LiteTest FromTestCase(TestCase testCase)
        {
            if (testCase is null) throw new ArgumentNullException(nameof(testCase));

            string[] lines = (string[])testCase.GetPropertyValue(LinesProperty);

            return new LiteTest(lines);
        }

        /// <summary>
        /// Index 0
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Index 1
        /// </summary>
        private string FullFixtureTypeName { get; }

        /// <summary>
        /// Index 2
        /// </summary>
        private string CompoundFactoryName { get; }

        /// <summary>
        /// Index 3
        /// </summary>
        public string TestMethodName { get; }

        public LiteTest(UnitTest unitTest)
        {
            Source = unitTest.Factory.Fixture.Assembly.Location;
            FullFixtureTypeName = unitTest.Factory.Fixture.Type.FullName;
            CompoundFactoryName = Parsing.EncodeFactoryMethod(unitTest.Factory);
            TestMethodName = unitTest.Name;
        }

        /// <summary>
        /// Loads a <see cref="LiteTest"/> from an array of strings. The array must have been created
        /// by calling <see cref="ToLines"/> on a previous instance of <see cref="LiteTest"/>.
        /// </summary>
        /// <param name="lines">An array with exactly four elements, each of which contains one of the auto-properties
        /// of <see cref="LiteTest"/>, in the order in which they appear in the source code.</param>
        public LiteTest(string[] lines)
        {
            if (lines is null)
                throw new ArgumentNullException(nameof(lines));
            if (lines.Length != 4)
                throw new FormatException($"Wrong number of elements in lines array. Should be four, was {lines.Length}.");

            Source = lines[0];
            FullFixtureTypeName = lines[1];
            CompoundFactoryName = lines[2];
            TestMethodName = lines[3];
        }

        public string[] ToLines()
        {
            return new string[]
            {
                Source,
                FullFixtureTypeName,
                CompoundFactoryName,
                TestMethodName
            };
        }

        /// <summary>
        /// Creates a <see cref="TestCase"/> from the current <see cref="LiteTest"/>, saving the data in the
        /// <see cref="LiteTest"/> in the <see cref="TestCase"/>s properties so it can be retrieved later.
        /// </summary>
        /// <returns></returns>
        public TestCase ToTestCase()
        {
            var @case = new TestCase();

            @case.Source = Source;
            @case.DisplayName = TestMethodName;
            @case.FullyQualifiedName = $"{FullFixtureTypeName}+{Parsing.GetFactoryMethodName(CompoundFactoryName)}.{TestMethodName}";
            @case.ExecutorUri = new Uri(SUnitTestExecutor.ExecutorUri);
            @case.SetPropertyValue(LinesProperty, ToLines());

            return @case;
        }

        public Discovery.Results.TestResult Run()
        {
            Assembly assembly = Assembly.LoadFrom(Source);
            Type fixtureType = assembly.GetType(FullFixtureTypeName);
            MethodInfo method = fixtureType.GetMethod(TestMethodName, Array.Empty<Type>());
            object fixture = Parsing.InvokeFactoryMethod(fixtureType, CompoundFactoryName);
            var func = (Func<Test>)method.CreateDelegate(typeof(Func<Test>), fixture);

            return TestRunner.RunTest(func, method.Name);
        }


        private static class Parsing
        {
            private const string factoryKindGroup = "factoryKind";
            private const string methodNameGroup = "methodName";
            private const string delimiter = @":";
            private static readonly Regex factoryNamePattern = new Regex(
                $@"(?<{factoryKindGroup}>[^\{delimiter}]*)\{delimiter}(?<{methodNameGroup}>.*)$");
            private const string defaultCtorKind = "default";
            private const string namedCtorKind = "named";

            public static string EncodeFactoryMethod(Factory factory)
            {
                string kind = string.Empty;
                if (factory.IsDefaultConstructor)
                    kind = defaultCtorKind;
                else if (factory.IsNamedConstructor)
                    kind = namedCtorKind;
                else
                    throw new ArgumentException("Unexpected kind of factory in EncodeFactoryMethod()");

                return $"{kind}{delimiter}{factory.Name}";
            }

            public static object InvokeFactoryMethod(Type fixtureType, string factoryName)
            {
                Match match = factoryNamePattern.Match(factoryName);

                Debug.Assert(match.Success);

                string kind = match.Groups[factoryKindGroup].Value;
                switch (kind)
                {
                    case defaultCtorKind:
                        var ctor = fixtureType.GetConstructor(Array.Empty<Type>());
                        return ctor.Invoke(Array.Empty<object>());
                    case namedCtorKind:
                        var method = fixtureType.GetMethod(match.Groups[methodNameGroup].Value);
                        return method.Invoke(null, Array.Empty<object>());
                    default:
                        throw new ArgumentOutOfRangeException("Unexpected factory kind in InvokeFactoryMethod()");
                }
            }

            public static string GetFactoryMethodName(string factoryName)
            {
                var match = factoryNamePattern.Match(factoryName);

                return match.Groups[methodNameGroup].Value;
            }
        }
    }
}
