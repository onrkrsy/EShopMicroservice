using EShop.Catalog.Features.Categories;
using EShop.Catalog.Repositories;

namespace EShop.Catalog.Features.Products;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;


    public decimal Price { get; set; }

    public Guid UserId { get; set; } = default!;

    public string? Picture { get; set; }


    public DateTime CreatedTime { get; set; }

    public Feature? Feature { get; set; }

    public Guid CategoryId { get; set; } = default!;

    public Category? Category { get; set; }
}