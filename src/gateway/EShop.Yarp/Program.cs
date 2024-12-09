var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.MapReverseProxy();

//http://localhost:5215/v1/catalog/categories bu þekilde serviceten cevap geldi
app.MapGet("/", () => "Hello World!");

app.Run();
