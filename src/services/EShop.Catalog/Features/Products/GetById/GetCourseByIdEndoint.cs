using System.Net;
using AutoMapper;
using EShop.Catalog.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EShop.Shared;
using EShop.Shared.Extensions;

namespace EShop.Catalog.Features.Products.GetById;

public record GetProductByIdQuery(Guid Id) : IRequest<ServiceResult<ProductDto>>;

public class GetProductByIdProductQueryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetProductByIdQuery, ServiceResult<ProductDto>>
{
    public async Task<ServiceResult<ProductDto>> Handle(GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var course = await context.Products.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course is null)
            return ServiceResult<ProductDto>.Error("Product Not Found",
                $"The course with id '{request.Id}' was not found.", HttpStatusCode.NotFound);

        var category =
            await context.Categories.FirstOrDefaultAsync(x => x.Id == course.CategoryId, cancellationToken);
        course.Category = category;

        var courseDto = mapper.Map<ProductDto>(course);
        return ServiceResult<ProductDto>.SuccessAsOk(courseDto);
    }
}

public static class GetProductByIdQueryEndpoint
{
    public static RouteGroupBuilder MapProductByIdQueryEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (IMediator mediator, Guid id) =>
            {
                var response = await mediator.Send(new GetProductByIdQuery(id));
                return response.ToActionResult();
            })
            .WithName("GetProductById")
            .Produces<ProductDto>()
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}