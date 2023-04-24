using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace DapperNpa.SourceGenerator.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static bool HasAttribute(this SyntaxNode node, string attributeName)
        {
            return node.DescendantNodesAndSelf()
                       .OfType<AttributeSyntax>()
                       .Any(attr => attr.Name.ToString() == attributeName);
        }

        public static T GetParents<T>(this SyntaxNode node)
        {
            var parent = node;
            while (true)
            {
                switch (parent)
                {
                    case null:
                        throw new Exception("Can't get namespace!");
                    case T t:
                        return t;
                    default:
                        parent = parent.Parent;
                        break;
                }
            }
        }

        public static IncrementalValuesProvider<T> GetSyntaxProvider<T>(this IncrementalGeneratorInitializationContext context, string attributeName) where T : SyntaxNode
        {
            return context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is T && node.HasAttribute(attributeName),
                    transform: (syntaxContext, _) => (T)syntaxContext.Node)
                .Where(m => m is not null);
        }

        public static string GetFullyQualifiedName(this Compilation compilation, MethodDeclarationSyntax methodDeclaration)
        {
            // get the Semantic Model for the syntax tree of the method declaration
            var semanticModel = compilation.GetSemanticModel(methodDeclaration.SyntaxTree);

            // get the symbol for the return type of the method
            var returnTypeSymbol = semanticModel.GetTypeInfo(methodDeclaration.ReturnType).Type;

            // get the fully qualified name of the return type symbol
            
            return returnTypeSymbol.ToDisplayString(
                new SymbolDisplayFormat(
                    typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces)
            );
        }
    }
}