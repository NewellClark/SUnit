using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using static SUnit.Analyzers.Helpers;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("SUnit.Analyzers.Tests")]

namespace SUnit.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SUnitAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SUNIT0000";
        private static readonly string SUnitTestFullName = "SUnit.Test";

        private static readonly DiagnosticDescriptor AssertUsedAsStatement = new DiagnosticDescriptor(
            DiagnosticId, 
            GetLocalizedString(nameof(Resources.AssertUsedAsStatement_Title)),
            GetLocalizedString(nameof(Resources.AssertUsedAsStatement_MessageFormat)),
            "SUnit",
            DiagnosticSeverity.Error,
            true,
            GetLocalizedString(nameof(Resources.AssertUsedAsStatement_Description)));

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(AssertUsedAsStatement); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterOperationAction(AnalyzeOperation, OperationKind.ExpressionStatement);
        }

        private static void AnalyzeOperation(OperationAnalysisContext context)
        {
            var compilation = context.Compilation;
            var syntaxTree = context.Operation.Syntax.SyntaxTree;
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            if (IsViolation(compilation, semanticModel, context.Operation.Syntax))
            {
                var diagnostic = Diagnostic.Create(AssertUsedAsStatement, context.Operation.Syntax.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

        internal static bool IsViolation(Compilation compilation, SemanticModel model, SyntaxNode node)
        {
            var operation = model.GetOperation(node) as IExpressionStatementOperation;

            if (operation is null)
                return false;

            var testType = compilation.GetTypeByMetadataName(SUnitTestFullName);

            return compilation.HasImplicitConversion(operation.Operation.Type, testType);
        }
        internal static bool IsAssertionStatement(IOperation operation)
        {
            if (operation is IExpressionStatementOperation statement && statement.Type != null)
            {
                if (SelfAndBaseTypes(statement.Type).Any(x => x.Name == SUnitTestFullName))
                    return true;
            }

            return false;
        }

        private static IEnumerable<ITypeSymbol> SelfAndBaseTypes(ITypeSymbol typeSymbol)
        {
            for (var current = typeSymbol; current != null; current = current.BaseType)
            {
                yield return current;
            }
        }
    }

    internal static class Helpers
    {
        public static LocalizableString GetLocalizedString(string resourceName)
        {
            return new LocalizableResourceString(resourceName, Resources.ResourceManager, typeof(Resources));
        }
    }
}
