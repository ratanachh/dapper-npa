﻿using DapperNpa.SourceGenerator.Extensions;
using DapperNpa.SourceGenerator.Models;
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

    private static void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<InterfaceDeclarationSyntax> typeList)
    {
        foreach (var repositoryInterface in typeList)
        {
            var @namespace = repositoryInterface.Parent is FileScopedNamespaceDeclarationSyntax ? repositoryInterface.GetParents<FileScopedNamespaceDeclarationSyntax>().Name : repositoryInterface.GetParents<NamespaceDeclarationSyntax>().Name;

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

    private static void GenerateRepositoryImplementation(
        SourceProductionContext context,
        Compilation compilation,
        RepositoryInformation information)
    {
        var methodSyntaxes = information.Syntax.Members.Select(m => (MethodDeclarationSyntax)m);
        List<string> methods = new();
        foreach (var methodDeclaration in methodSyntaxes)
        {
            var queryAttribute = methodDeclaration.AttributeLists
                .SelectMany(al => al.Attributes)
                .FirstOrDefault(a => a.Name.ToString() == "Query");

            var identifier = methodDeclaration.Identifier;
            if (queryAttribute != null)
            {
                var modifier = methodDeclaration.Modifiers;
                string[] objectType = { "List", "Dictionary", "HashSet", "Object" };
                var methodReturnType = compilation.GetTypeString(methodDeclaration);
                var returnType = objectType.Contains(methodReturnType) 
                    ? $"global::{compilation.GetFullyQualifiedName(methodDeclaration)}"
                    : ((PredefinedTypeSyntax)methodDeclaration.ReturnType).Keyword.ValueText;

                var parametersSyntax = methodDeclaration.ParameterList.Parameters;
                List<ParameterInfo> parameterInfos = new();
                foreach (var parameter in parametersSyntax)
                {
                    var parameterType = compilation.GetFullyQualifiedName(parameter);
                    var parameterName = parameter.Identifier.ValueText;
                    var isPredefinedType = parameter.Type is PredefinedTypeSyntax;
                    parameterInfos.Add(new ParameterInfo
                    {
                        Type = parameterType,
                        Name = parameterName,
                        IsPredefined = isPredefinedType,
                    });
                }

                var returnKeyword = returnType != "void" ? "return" : string.Empty;
                var resultReturnType = returnType != "void" ? $"<{returnType}>" : string.Empty;
                var resultReturn = returnType != "void" ? 
                    (methodDeclaration.ReturnType switch
                    {
                        IdentifierNameSyntax => compilation.GetTypeString(methodDeclaration) switch
                        {
                            "Array" => ".ToArray()",
                            "List" => ".ToList()",
                            "Dictionary" => ".ToDictionary()",
                            "HashSet" => ".ToHashSet()",
                            _ => ".FirstOrDefault()"
                        },
                        _ => ".FirstOrDefault()"
                    })
                    : string.Empty;
                var parameters = string.Join(", ", parameterInfos.Select(p => $"global::{p.Type} {p.Name}"));

                var arguments = queryAttribute.ArgumentList!.Arguments;
                var sql = arguments.ElementAt(0).Expression.ToString();

                var queryArgument = parameterInfos.Count switch
                {
                    0 => string.Empty,
                    1 => $", {parameterInfos.ElementAt(0).Name}",
                    _ => $", new {{ {string.Join(",", parameterInfos.Select(p => $"@{p.Name} = {p.Name}"))} }}",
                };

                if (!Debugger.IsAttached)
                {
                    Debugger.Launch();
                }

                methods.Add(MethodImplementationTemplate
                    .Replace("__MODIFIER__", $"{modifier}")
                    .Replace("__RETURNTYPE__", $"{returnType}")
                    .Replace("__IDENTIFIER__", $"{identifier}")
                    .Replace("__PARAMETERS__", $"{parameters}")
                    .Replace("__RETURNKEYWORD__", $"{returnKeyword}")
                    .Replace("__QUERYMETHOD__", "Query")
                    .Replace("__RESULTRETURNTYPE__", resultReturnType)
                    .Replace("__SQLIMPLEMENTATION__", $"{sql}{queryArgument}")
                    .Replace("__RETURN_RESULT__", $"{resultReturn}"));
            } 
            else
            {
                var desc = new DiagnosticDescriptor (
                    "SG0001",
                    "No Query attribute found",
                    "No Query attribute declared on method (" + identifier + "(...)) in interface (" + information.InterfaceFullyQualify +")",
                    "Problem",
                    DiagnosticSeverity.Error,
                    true
                    );
                context.ReportDiagnostic(Diagnostic.Create(desc, Location.None));
                
            }
        }

        var output = RepositoryImplementationTemplate
            .Replace("__NAMESPACE__", information.Namespace)
            .Replace("__CLASS__", information.Class)
            .Replace("__INTERFACE___", information.InterfaceFullyQualify)
            .Replace("__IMPLEMENTATION_METHODS__", string.Join("\n\n", methods));
        context.AddSource(information.GeneratedName, output);
    }
}