using EShop.Catalog.Features.Products;
using EShop.Catalog.Repositories;


namespace EShop.Catalog.Features.Categories;

public class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public List<Product>? Products { get; set; }
}