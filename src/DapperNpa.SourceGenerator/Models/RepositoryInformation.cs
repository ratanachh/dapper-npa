using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DapperNpa.SourceGenerator.Models;

public class RepositoryInformation
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