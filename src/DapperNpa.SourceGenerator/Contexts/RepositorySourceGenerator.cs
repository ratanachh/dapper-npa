﻿namespace DapperNpa.SourceGenerator;

internal sealed partial class RepositorySourceGenerator
{
    private static readonly string RepositoryImplementationClass = $@"// <auto-generated/>
// {DateTime.Now}
#nullable enable

namespace DapperNpa.Repository
{{
    internal sealed class UserRepositoryImpl : global::DapperNpa.Example.Repository.IUserRepository
    {{
        public global::DapperNpa.Example.Model.User GetById(global::System.Guid id)
        {{
            return new global::DapperNpa.Example.Model.User();
        }}
    }}
}}
";
    
}