using Microsoft.CodeAnalysis;

namespace DapperNpa.SourceGenerator;

[Generator(LanguageNames.CSharp)]
internal sealed partial class RepositorySourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(PostInitializationCallback);
    }
    
    private static void PostInitializationCallback(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(RepositorySourceGeneratorName, RepositoryImplementationClass);
    }
}