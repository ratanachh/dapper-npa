using DapperNpa.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;

namespace DapperNpa.SourceGenerator
{
    [Generator(LanguageNames.CSharp)]
    internal sealed partial class DependencyInjectionSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var provider = context.GetSyntaxProvider<InterfaceDeclarationSyntax>(RepositoryAttributeName);

            var compilation = context.CompilationProvider.Combine(provider.Collect());

            context.RegisterSourceOutput(compilation, (spc, source) => Execute(spc, source.Left, source.Right));
        }

        private static void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<InterfaceDeclarationSyntax> typeList)
        {
            var repositoryRegistration = typeList
                .Select(repositoryInterface =>
                {
                    var @namespace = repositoryInterface.Parent is FileScopedNamespaceDeclarationSyntax ? repositoryInterface.GetParents<FileScopedNamespaceDeclarationSyntax>().Name : repositoryInterface.GetParents<NamespaceDeclarationSyntax>().Name;
                    var interfaceNamespace = @namespace.ToString();
                    var interfaceName = $"{interfaceNamespace}.{repositoryInterface.Identifier}";
                    return $"services.AddScoped<global::{interfaceName}, global::{interfaceName + "Impl"}>();";
                });

            _dependencyInjectionRegistration = _dependencyInjectionRegistration.Replace("//__DEPENDENCY_INJECTION_SERVICES__", string.Join("\n            ", repositoryRegistration));
            
            context.AddSource(DependencyInjectionSourceGeneratorName, _dependencyInjectionRegistration);
        }
    }
}