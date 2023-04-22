using DapperNpa.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace DapperNpa.SourceGenerator
{
    [Generator(LanguageNames.CSharp)]
    internal sealed partial class DependencyInjectionSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var provider = GetInterfacesSyntaxProvider(context);

            var compilation = context.CompilationProvider.Combine(provider.Collect());

            context.RegisterSourceOutput(compilation, (spc, source) => Execute(spc, source.Left, source.Right));
        }

        private void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<InterfaceDeclarationSyntax> typeList)
        {
            var repositoryRegistration = typeList
                .Select(repositoryInterface =>
                {
                    var interfaceNamespace = repositoryInterface.GetParents<NamespaceDeclarationSyntax>().Name.ToString();
                    var interfaceName = string.Format("{0}.{1}", interfaceNamespace, repositoryInterface.Identifier.ToString());
                    return string.Format("services.AddScoped<global::{0}, global::{1}>();", interfaceName, interfaceName + "Impl");
                });

            _dependencyInjectionRegistration = _dependencyInjectionRegistration.Replace("//__DEPENDENCY_INJECTION_SERVICES__", string.Join("\n", repositoryRegistration));
            context.AddSource(DependencyInjectionSourceGeneratorName, _dependencyInjectionRegistration);
        }

        private IncrementalValuesProvider<InterfaceDeclarationSyntax> GetInterfacesSyntaxProvider(IncrementalGeneratorInitializationContext context)
        {
            return context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node.IsKind(SyntaxKind.InterfaceDeclaration) && node.HasAttribute(RepositoryAttributeName),
                    transform: (syntaxContext, _) => (InterfaceDeclarationSyntax)syntaxContext.Node);
        }
    }
}