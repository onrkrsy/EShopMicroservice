using AutoMapper;

namespace EShop.Catalog.Features.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();

        CreateMap<Feature, FeatureDto>().ReverseMap();
    }
}