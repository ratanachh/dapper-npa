﻿namespace DapperNpa.SourceGenerator;

internal sealed partial class DependencyInjectionSourceGenerator
{
    private static string _dependencyInjectionRegistration = $@"// <auto-generated/>
// {DateTime.Now}
#nullable enable

namespace DapperNpa.DependencyInjection
{{
    internal static class DependencyInjectionExtension
    {{
        public static global::Microsoft.Extensions.DependencyInjection.IServiceCollection AddDapperNpa(global::Microsoft.Extensions.DependencyInjection.IServiceCollection services, global::System.string connectionString)
        {{
            services.AddTransient<global::System.Data.IDbConnection>((sp) => new global::Npgsql.NpgsqlConnection(connectionString));
            {{0}}
            return services;
        }}
    }}
}}
";
    
}