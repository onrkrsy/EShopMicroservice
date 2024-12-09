using Asp.Versioning.Builder;
using EShop.Catalog.Features.Products.Create;
using EShop.Catalog.Features.Products.Delete;
using EShop.Catalog.Features.Products.GetAll;
using EShop.Catalog.Features.Products.GetAllByUserId;
using EShop.Catalog.Features.Products.GetById;
using EShop.Catalog.Features.Products.Update;
namespace EShop.Catalog.Features.Products;

public static class ProductEndpointsExt
{
    public static void AddProductEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/courses")
            .MapAllProductsQueryEndpoint()
            .MapAllProductByUserIdEndpoint()
            .MapProductByIdQueryEndpoint()
            .MapCreateProductCommandEndpoint()
            .MapUpdateProductCommandEndpoint()
            .MapDeleteProductCommandEndpoint()
            .WithTags("Products").WithApiVersionSet(apiVersionSet).RequireAuthorization();
    }
}