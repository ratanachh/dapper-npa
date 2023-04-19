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
                    (node, ct) => node.IsKind(SyntaxKind.InterfaceDeclaration) && node.HasAttribute("Repository"), 
                    (context, ct) => (InterfaceDeclarationSyntax)context.Node)
                .Collect();

            var repositoryRegistration = repositoryInterfaces
                .Select((repositoryInterfaces, ct) =>
                {
                    var registrations = repositoryInterfaces.Select(repositoryInterface =>
                    {
                        var interfaceNamespace = repositoryInterface.GetParents<NamespaceDeclarationSyntax>().Name.ToString();
                        var interfaceName = repositoryInterface.Identifier.ToString();
                        var className = interfaceName.Substring(1) + "Impl";
                        return string.Format("services.AddScoped<global::{0}.{1}, global::{0}.{2}>();", interfaceNamespace, interfaceName, className);
                    });
                    return string.Join("\n", registrations);
                });

            context.RegisterSourceOutput(repositoryRegistration, (context, source) =>
            {
                _dependencyInjectionRegistration = _dependencyInjectionRegistration.Replace("###DEPENDENCY_INJECTION_SERVICES###", source);
                context.AddSource(DependencyInjectionSourceGeneratorName, _dependencyInjectionRegistration);
            });
        }
    }
}