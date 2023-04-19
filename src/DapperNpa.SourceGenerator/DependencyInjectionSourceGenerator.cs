using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DapperNpa.SourceGenerator;

[Generator(LanguageNames.CSharp)]
internal sealed partial class DependencyInjectionSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(PostInitializationCallback);
        context.RegisterImplementationSourceOutput(context.CompilationProvider, ImplementationCallback);
    }
    
    private static void PostInitializationCallback(IncrementalGeneratorPostInitializationContext context)
    {
        // context.AddSource(DependencyInjectionSourceGeneratorName, _dependencyInjectionRegistration);
    }

    private static void ImplementationCallback(SourceProductionContext context, Compilation provider)
    {
        _dependencyInjectionRegistration = string.Format(_dependencyInjectionRegistration, "services.AddScoped<>();");
        // provider.crea
        context.AddSource(DependencyInjectionSourceGeneratorName, _dependencyInjectionRegistration);

    }
}