using SUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using System.Reflection;

namespace SUnit.Analyzers.Tests
{
    //  Any public class that contains unit tests is a Test Fixture.
    public class AssertUsedAsStatementTests
    {
        private static string CreateSource(string body)
        {
            return @$"
using SUnit;

namespace MockNamespace
{{
    public class MockTestClass
    {{
        public void MockTestMethod()
        {{
            {body}
        }}
    }}
}}";
        }

        public Test AssertUsedAsStatement_IsReported()
        {
            string body = @"Assert.That(2 + 2).Is.EqualTo(5);";
            string source = CreateSource(body);
            var syntax = CSharpSyntaxTree.ParseText(source);
            
            var compilation = CSharpCompilation.Create("MockCompilation")
                .AddReferences(
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Test).Assembly.Location))
                .AddSyntaxTrees(syntax);
            var root = syntax.GetCompilationUnitRoot();
            var model = compilation.GetSemanticModel(syntax);

            var reported = root.DescendantNodes()
                .Where(x => SUnitAnalyzer.IsViolation(compilation, model, x));

            return Assert.That(reported.Count()).Is.EqualTo(1) &&
                Assert.That(reported.Single().ToString()).Is.EqualTo(body);
        }
    }
}
