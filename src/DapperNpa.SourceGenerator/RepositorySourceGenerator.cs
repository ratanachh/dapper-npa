using DapperNpa.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;

namespace DapperNpa.SourceGenerator;

[Generator(LanguageNames.CSharp)]
internal sealed partial class RepositorySourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.GetSyntaxProvider<InterfaceDeclarationSyntax>(RepositoryAttributeName);
        var compilation = context.CompilationProvider.Combine(provider.Collect());
        context.RegisterSourceOutput(compilation, (spc, source) => Execute(spc, source.Left, source.Right));
    }

    private static void Execute(SourceProductionContext context, Compilation compilation,ImmutableArray<InterfaceDeclarationSyntax> typeList)
    {
        foreach (var repositoryInterface in typeList)
        {
            var @namespace = repositoryInterface.Parent is FileScopedNamespaceDeclarationSyntax ? repositoryInterface.GetParents<FileScopedNamespaceDeclarationSyntax>().Name: repositoryInterface.GetParents<NamespaceDeclarationSyntax>().Name;
          
            var interfaceNamespace = @namespace.ToString();
            var interfaceName = $"{repositoryInterface.Identifier}";
            var className = $"{repositoryInterface.Identifier}Impl";
            GenerateRepositoryImplementation(context, compilation, new RepositoryInformation
            (
                @interface: interfaceName,
                @class: className,
                @namespace: interfaceNamespace,
                syntax: repositoryInterface
            ));
        }
    }
    
    private class RepositoryInformation
    {
        public string Interface { get; }
        public string InterfaceFullyQualify => $"{Namespace}.{Interface}";
        public string Class { get; }
        public string ClassFullyQualify => $"{Namespace}.{Class}";
        public string Namespace { get; }
        public string GeneratedName => $"{Class}.g.cs";

        public InterfaceDeclarationSyntax Syntax { get; }

        public RepositoryInformation(string @interface, string @class, string @namespace, InterfaceDeclarationSyntax syntax)
        {
            Interface = @interface;
            Class = @class;
            Namespace = @namespace;
            Syntax = syntax;
        }
    }

    private static void GenerateRepositoryImplementation(
        SourceProductionContext context, 
        Compilation compilation, 
        RepositoryInformation information)
    {
        var method = "public global::DapperNpa.Aspnet.Example.Model.User GetById(Guid id) { return new global::DapperNpa.Aspnet.Example.Model.User(); }";


        if (information.Class == "IUserRepositoryImpl")
        {
            var methods = information.Syntax.Members;

            foreach ( var methodDeclaration in methods)
            {
                var modifier = "public";
                var returnType = methodDeclaration.ReturnType;
                var identifier = methodDeclaration;
                if (!Debugger.IsAttached)
                {
                    Debugger.Launch();
                }
            }
            
        }

        var output = RepositoryImplementationTemplate
            .Replace("__NAMESPACE__", information.Namespace)
            .Replace("__CLASS__", information.Class)
            .Replace("__INTERFACE___", information.InterfaceFullyQualify)
            .Replace("__IMPLEMENTATION_METHODS__", method);
        context.AddSource(information.GeneratedName, output);
    }
}