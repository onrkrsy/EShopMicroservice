using EShop.Catalog.Features.Categories;
using EShop.Catalog.Features.Products;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace EShop.Catalog.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; init; }
    public DbSet<Product> Products { get; init; }

    public static AppDbContext Create(IMongoDatabase database)
    {
        return new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Product entity configuration
        modelBuilder.Entity<Product>().ToCollection("Products");
        modelBuilder.Entity<Product>().HasKey(x => x.Id);
        modelBuilder.Entity<Product>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<Product>().Property(x => x.Name).HasElementName("name").HasMaxLength(100);
        modelBuilder.Entity<Product>().Property(x => x.Description).HasElementName("description").HasMaxLength(500);


        modelBuilder.Entity<Product>().Property(x => x.Price).HasElementName("price");
        modelBuilder.Entity<Product>().Property(x => x.Picture).HasElementName("picture").HasMaxLength(200);
        modelBuilder.Entity<Product>().Property(x => x.UserId).HasElementName("user_id").HasMaxLength(50);
        modelBuilder.Entity<Product>().Property(x => x.CreatedTime).HasElementName("created_time");
        modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasElementName("category_id");
        //modelBuilder.Entity<Product>().Property(x => x.Feature).HasElementName("feature");

        modelBuilder.Entity<Product>().Ignore(x => x.Category);
        // Category entity configuration
        modelBuilder.Entity<Category>().ToCollection("categories");
        modelBuilder.Entity<Category>().HasKey(x => x.Id);
        modelBuilder.Entity<Category>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<Category>().Property(x => x.Name).HasElementName("name").HasMaxLength(100);
        modelBuilder.Entity<Category>().Ignore(x => x.Products);


        // Configure Feature as an owned entity
        modelBuilder.Entity<Product>().OwnsOne(p => p.Feature, feature =>
        {
            feature.HasElementName("feature");
            feature.Property(f => f.Rating).HasElementName("rating");
            feature.Property(f => f.OwnerFullName).HasElementName("owner_full_name");
            // Add other properties of Feature if needed
        });

        base.OnModelCreating(modelBuilder);
    }
}