using EShop.Catalog;
using EShop.Catalog.Features.Categories;
using EShop.Catalog.Features.Products;
using EShop.Catalog.Repositories;
using EShop.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthenticationExt(builder.Configuration);
//builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServicesExt();
builder.Services.AddDatabaseServicesExt();
builder.Services.AddCommonServicesExt(typeof(CatalogAssembly));
builder.Services.AddOptionsExt();
builder.Services.AddVersioningExt();
var app = builder.Build();
app.UseExceptionHandler();
 
app.AddSwaggerMiddlewareExt();
//app.UseAuthentication();
//app.UseAuthorization();

var apiVersionSet = app.AddVersionSetExt();  
app.AddCategoryEndpointsExt(apiVersionSet);
app.AddProductEndpointsExt(apiVersionSet);
_ = app.AddSeedDataExt().ContinueWith(task => Console.WriteLine("Seed data has been saved successfully."));
app.Run();
