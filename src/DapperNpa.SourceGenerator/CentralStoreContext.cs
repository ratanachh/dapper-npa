using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperNpa.SourceGenerator
{
    internal static class CentralStoreContext
    {
        public static Repository Repository { get; set; } = new();
    }

    internal class Repository
    {
        public List<Interface> Interfaces { get; set; } = new();
    }

    internal class Interface
    {
        public InterfaceDeclarationSyntax DeclarationSyntax { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public string ImplementationClassName { get; set; } = string.Empty;
        public List<Query> Queries { get; set; } = new();
    }

    internal class SelectProvider
    {

    }

    internal class Query
    {
        public string Sql { get; }
        public Query(string sql)
        {
            Sql = sql;
        }
    }

    internal class Method
    {
        public string Name { get; set; } = string.Empty;
        public Type ReturnType { get; set;}

        public List<object> Parameters { get; set; }
    }
}
