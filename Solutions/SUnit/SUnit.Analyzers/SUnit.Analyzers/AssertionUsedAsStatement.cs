using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

[assembly: InternalsVisibleTo("SUnit.Analyzers.Tests")]

namespace SUnit.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AssertionUsedAsStatement : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SUNIT0001";
        private const string category = "Usage";

        private static readonly LocalizableString title = new LocalizableResourceString(
            nameof(Resources.AssertionUsedAsStatement_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString messageFormat = new LocalizableResourceString(
            nameof(Resources.AssertionUsedAsStatement_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString description = new LocalizableResourceString(
            nameof(Resources.AssertionUsedAsStatement_Description), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor rule = new DiagnosticDescriptor(DiagnosticId, title, messageFormat, category, DiagnosticSeverity.Error, true, description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            static void syntaxNodeAction(SyntaxNodeAnalysisContext context)
            {
                if (IsNodeViolation(context.Compilation, context.SemanticModel, context.Node, context.CancellationToken))
                {
                    var operation = (IExpressionStatementOperation)context.SemanticModel.GetOperation(context.Node, context.CancellationToken);
                    string syntaxText = operation.Operation.Syntax.WithoutTrivia().GetText().ToString();
                    Diagnostic diagnostic = Diagnostic.Create(rule, context.Node.GetLocation(), syntaxText);
                    context.ReportDiagnostic(diagnostic);
                }
            }

            context.RegisterSyntaxNodeAction(syntaxNodeAction, SyntaxKind.ExpressionStatement);
        }

        internal static bool IsNodeViolation(Compilation compilation, SemanticModel model, SyntaxNode node, CancellationToken cancellationToken)
        {
            if (compilation is null) throw new ArgumentNullException(nameof(compilation));
            if (model is null) throw new ArgumentNullException(nameof(model));

            var operation = model.GetOperation(node, cancellationToken) as IExpressionStatementOperation;

            if (operation is null)
                return false;

            var testType = compilation.GetTypeByMetadataName(typeof(Test).FullName);

            return compilation.HasImplicitConversion(operation.Operation.Type, testType);
        }
    }
}

