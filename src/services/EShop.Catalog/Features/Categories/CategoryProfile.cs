using AutoMapper;
using EShop.Catalog.Features.Categories.Create;

namespace EShop.Catalog.Features.Categories;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        // CreateCategoryCommand -> Category
        CreateMap<CreateCategoryCommand, Category>();

        CreateMap<CategoryDto, Category>().ReverseMap();
    }
}