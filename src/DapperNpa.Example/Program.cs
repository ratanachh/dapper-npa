// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

public class MyTest
{
    public int MyProperty { get; set; }
}

using Roslyn.Generated;

[EqualityComparer2(typeof(MyTest))]
public class Test 
{

}