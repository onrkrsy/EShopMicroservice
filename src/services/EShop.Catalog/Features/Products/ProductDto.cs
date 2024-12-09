using EShop.Catalog.Features.Categories;

namespace EShop.Catalog.Features.Products;

public record ProductDto(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string UserId,
    string Picture,
    DateTime CreatedTime,
    FeatureDto Feature,
    CategoryDto Category);