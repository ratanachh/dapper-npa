using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
    }
}