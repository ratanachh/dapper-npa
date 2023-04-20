var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDapperNpa();
var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();



namespace DapperNpa.Attributes
{
    using System;
    
    [AttributeUsage(AttributeTargets.Interface)]
    public class RepositoryAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryAttribute : Attribute
    {
        public string Sql { get; }

        public QueryAttribute(string sql)
        {
            Sql = sql;
        }
    }
}


namespace DapperNpa.Aspnet.Example.Model
{
    using System;
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

namespace DapperNpa.Aspnet.Example.Repository
{
    using DapperNpa.Aspnet.Example.Model;
    using DapperNpa.Attributes;
    
    using System;
    
    [Repository]
    public interface IUserRepository
    {
        [Query(sql: "SELECT * FROM users WHERE id = @id")]
        User GetById(Guid id);
    }
    
    [Repository]
    public interface IProductRepository
    {
        [Query(sql: "SELECT * FROM products WHERE id = @id")]
        Product GetById(Guid id);
    }
    
}