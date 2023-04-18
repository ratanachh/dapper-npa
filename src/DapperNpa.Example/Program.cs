// See https://aka.ms/new-console-template for more information

using DapperNpa.Repository;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

Console.WriteLine("Hello, World!");

var userRepo = new UserRepositoryImpl();

public class MyTest
{
    public int MyProperty { get; set; }
}