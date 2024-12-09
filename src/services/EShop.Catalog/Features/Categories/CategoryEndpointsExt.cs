using Asp.Versioning.Builder;
using EShop.Catalog.Features.Categories.Create;
using EShop.Catalog.Features.Categories.GetAll;
using EShop.Catalog.Features.Categories.GetById;

namespace EShop.Catalog.Features.Categories;

public static class CategoryEndpointsExt
{
    public static void AddCategoryEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/categories")
            .MapCreateCategoryEndpoint()
            .MapCategoryByIdQueryEndpoint()
            .MapCategoryByIdQueryEndpointV2()
            .MapAllCategoryQueryEndpoint()
            .WithTags("Categories").WithApiVersionSet(apiVersionSet);
    }
}