
var builder = WebApplication.CreateBuilder(args);
var con = builder.Configuration.GetConnectionString("default");
var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();