using System.Linq;
using DapperNpa.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DapperNpa.SourceGenerator
{
    [Generator(LanguageNames.CSharp)]
    internal sealed partial class DependencyInjectionSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var repositoryInterfaces = context.SyntaxProvider
                .CreateSyntaxProvider(
                    (node, _) => node.IsKind(SyntaxKind.InterfaceDeclaration) && node.HasAttribute("Repository"), 
                    (syntaxContext, _) => (InterfaceDeclarationSyntax)syntaxContext.Node)
                .Collect();

            var repositoryRegistration = repositoryInterfaces
                .Select((interfaces, _) =>
                {
                    var registrations = interfaces.Select(repositoryInterface =>
                    {
                        var interfaceNamespace = repositoryInterface.GetParents<NamespaceDeclarationSyntax>().Name.ToString();
                        var interfaceName = repositoryInterface.Identifier.ToString();
                        var className = interfaceName.Substring(1) + "Impl";
                        return string.Format("services.AddScoped<global::{0}.{1}, global::{0}.{2}>();", interfaceNamespace, interfaceName, className);
                    });
                    return string.Join("\n", registrations);
                });

            context.RegisterSourceOutput(repositoryRegistration, (productionContext, source) =>
            {
                _dependencyInjectionRegistration = _dependencyInjectionRegistration.Replace("###DEPENDENCY_INJECTION_SERVICES###", source);
                productionContext.AddSource(DependencyInjectionSourceGeneratorName, _dependencyInjectionRegistration);
            });
        }
    }
}