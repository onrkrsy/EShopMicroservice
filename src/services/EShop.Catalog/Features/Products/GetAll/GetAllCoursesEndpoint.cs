using AutoMapper;
using EShop.Catalog.Features.Products;
using EShop.Catalog.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EShop.Shared;


namespace EShop.Catalog.Features.Products.GetAll;

public class GetAllProductsQuery : IRequest<ServiceResult<List<ProductDto>>>
{
}

public class GetAllProductsQueryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetAllProductsQuery, ServiceResult<List<ProductDto>>>
{
    public async Task<ServiceResult<List<ProductDto>>> Handle(GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await context.Products
            .ToListAsync(cancellationToken);

        var categories = await context.Categories.ToListAsync(cancellationToken);


        foreach (var course in courses) course.Category = categories.First(x => x.Id == course.CategoryId);


        var coursesListDtos = mapper.Map<List<ProductDto>>(courses);


        return ServiceResult<List<ProductDto>>.SuccessAsOk(coursesListDtos);
    }
}


public static class GetAllProductsEndpoint
{
    public static RouteGroupBuilder MapAllProductsQueryEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator) =>
            {
                var response = await mediator.Send(new GetAllProductsQuery());
                return Results.Ok(response);
            })
            .WithName("GetAllProducts")
            .Produces<List<ProductDto>>();

        return group;
    }
}
