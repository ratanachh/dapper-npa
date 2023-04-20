var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDapperNpa();
var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();