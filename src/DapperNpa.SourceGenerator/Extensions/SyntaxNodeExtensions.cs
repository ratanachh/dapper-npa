using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

        public static T GetParents<T>(this SyntaxNode node)
        {
            var parent = node;
            while (true)
            {
                if (parent == null) 
                {
                   throw new Exception("Can't get namespace!");
                }

                if (parent is T t) {
                    return t;
                }

                parent = parent.Parent;
            }
        }
    }
}