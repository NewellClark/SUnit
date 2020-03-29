using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SUnit;

namespace SUnit.Analyzers
{
    public class AssertionUsedAsStatementTests
    {
        private static string FromTestBody(string body)
        {
            return $@"
using System;
using SUnit;

namespace MockNamespace
{{
    public class MockFixture
    {{
        public Test MockTestMethod()
        {{
            {body}
        }}
    }}
}}";
        }

        private static string FromStatementExpression(string statementExpression)
        {
            string body = $@"{statementExpression}
            return Assert.That(2 + 2).Is.EqualTo(4);";

            return FromTestBody(body);
        }

        public AssertionUsedAsStatementTests(string sourceCode)
        {
            this.SourceCode = sourceCode;
            SyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            Compilation = CSharpCompilation.Create("MockCompilation")
                .AddReferences(
                    MetadataReference.CreateFromFile(typeof(string).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Test).Assembly.Location))
                .AddSyntaxTrees(SyntaxTree);
            SemanticModel = Compilation.GetSemanticModel(SyntaxTree);
            Root = SyntaxTree.GetCompilationUnitRoot();
            ReportedNodes = Root.DescendantNodes()
                .OfType<ExpressionStatementSyntax>()
                .Where(node => AssertionUsedAsStatement.IsNodeViolation(Compilation, SemanticModel, node, default));
        }

        protected string SourceCode { get; }
        protected SyntaxTree SyntaxTree { get; }
        protected Compilation Compilation { get; }
        protected SemanticModel SemanticModel { get; }
        protected CompilationUnitSyntax Root { get; }
        protected IEnumerable<SyntaxNode> ReportedNodes { get; }

        public class SingleReturnStatement : AssertionUsedAsStatementTests
        {
            public SingleReturnStatement() 
                : base(FromTestBody("return Assert.That(2 + 2).Is.EqualTo(4);")) { }

            public Test IsNotViolation() => Assert.That(ReportedNodes).Is.Empty;
        }

        public class VariableAssignmentStatement : AssertionUsedAsStatementTests
        {
            public VariableAssignmentStatement() 
                : base(FromStatementExpression("var test = Assert.That(17 + 4).Is.Not.EqualTo(49);")) { }

            public Test IsNotViolation() => Assert.That(ReportedNodes).Is.Empty;
        }

        public class SingleStatementExpression : AssertionUsedAsStatementTests
        {
            private readonly string expression;
            public SingleStatementExpression(string expression) 
                : base(FromStatementExpression(expression))
            {
                this.expression = expression;
            }

            public Test IsFlaggedAsViolation()
            {
                var nodes = ReportedNodes.Select(node => node.WithoutTrivia().GetText().ToString());

                return Assert.That(nodes.Count()).Is.EqualTo(1) &&
                    Assert.That(nodes.Single()).Is.EqualTo(expression);
            }
        }

        public class MethodInvocation : SingleStatementExpression
        {
            public MethodInvocation() 
                : base("Assert.That(5).Is.EqualTo(7);") { }
        }
    }
}
