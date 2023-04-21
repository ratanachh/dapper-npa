using System.Diagnostics;
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
            var provider = GetInterfacesSyntaxProvider(context).Collect();

            var repositoryRegistration = provider
                .Select((interfaces, _) =>
                {
                    var registrations = interfaces.Select(repositoryInterface =>
                    {
                        var interfaceNamespace = repositoryInterface.GetParents<NamespaceDeclarationSyntax>().Name.ToString();
                        var interfaceName = string.Format("{0}.{1}", interfaceNamespace, repositoryInterface.Identifier.ToString());
                        var className = string.Format("{0}.{1}", interfaceNamespace, interfaceName + "Impl");

                        

                        CentralStoreContext.Repository.Interfaces.Add(new Interface { 
                            DeclarationSyntax = repositoryInterface,
                            Name = interfaceName,
                            Namespace = interfaceNamespace,
                            ImplementationClassName = className,
                        });
                        return string.Format("services.AddScoped<global::{0}, global::{1}>();", interfaceName, className);
                    });
                    return string.Join("\n", registrations);
                });

            context.RegisterSourceOutput(repositoryRegistration, RegisterDependencyOutput);
        }


        private void RegisterDependencyOutput(SourceProductionContext context, string source)
        {
            _dependencyInjectionRegistration = _dependencyInjectionRegistration.Replace("//__DEPENDENCY_INJECTION_SERVICES__", source);
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }

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