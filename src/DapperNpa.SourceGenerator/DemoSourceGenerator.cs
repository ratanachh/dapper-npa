
using Microsoft.CodeAnalysis;

namespace DapperNpa.SourceGenerator
{
    [Generator(LanguageNames.CSharp)]
    public class DemoSourceGenerator : IIncrementalGenerator
    {
       public void Initialize(IncrementalGeneratorInitializationContext context)
       {
        //   throw new Exception("Test exception!"); // delete me after test
            context.RegisterPostInitializationOutput(PostInitializationCallback);
       }
        private static void PostInitializationCallback(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("EqualityComparerAttribute.g.cs", "EqualityComparerAttribute");
        }

    }
}
