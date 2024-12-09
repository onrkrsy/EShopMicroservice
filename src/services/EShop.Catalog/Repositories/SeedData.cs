using EShop.Catalog.Features.Categories;
using EShop.Catalog.Features.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;  

namespace EShop.Catalog.Repositories;

public static class SeedData
{
    public static async Task AddSeedDataExt(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
        var mongoOption = scope.ServiceProvider.GetRequiredService<MongoOption>();
        var dbContext = AppDbContext.Create(mongoClient.GetDatabase(mongoOption.DatabaseName));

        dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        await SeedDatabaseAsync(dbContext);
    }


    private static async Task SeedDatabaseAsync(AppDbContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Id = NewId.NextGuid(), Name = "Computer" },
                new() { Id = NewId.NextGuid(), Name = "Software" },
                new() { Id = NewId.NextGuid(), Name = "Book" },
                new() { Id = NewId.NextGuid(), Name = "Electronic" }

            };

            var randomUserId = Guid.NewGuid();

            List<Product> Products =
            [
                new()
                {
                    Id = NewId.NextGuid(), Name = "Laptop",
                    Description = "Laptop Product", 
                    Price = 100,
                    UserId = randomUserId, 
                    CreatedTime = DateTime.Now,
                    Feature = new Feature {  Rating = 4, OwnerFullName = "Company1" },
                    CategoryId = categories.First().Id
                },
                new()
                {
                    Id = NewId.NextGuid(), Name = "Desktop",
                    Description = "Desktop Product",
                    Price = 200,
                    UserId = randomUserId,
                    CreatedTime = DateTime.Now,
                    Feature = new Feature {  Rating = 4, OwnerFullName = "Company2" },
                    CategoryId = categories.First().Id
                },
                new()
                {
                    Id = NewId.NextGuid(), Name = "Windows",
                    Description = "Windows Product",
                    Price = 300,
                    UserId = randomUserId,
                    CreatedTime = DateTime.Now,
                    Feature = new Feature {  Rating = 4, OwnerFullName = "Company3" },
                    CategoryId = categories[1].Id
                },

            ];


            await context.Categories.AddRangeAsync(categories);
            await context.Products.AddRangeAsync(Products);
            await context.SaveChangesAsync();
        }
    }
}