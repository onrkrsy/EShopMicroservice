using AutoMapper;
using EShop.Catalog.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EShop.Shared;
using EShop.Shared.Extensions;
namespace EShop.Catalog.Features.Products.GetAllByUserId;

public record GetAllProductByUserIdQuery(Guid UserId) : IRequest<ServiceResult<List<ProductDto>>>;

public class GetProductByUserIdQueryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetAllProductByUserIdQuery, ServiceResult<List<ProductDto>>>
{
    public async Task<ServiceResult<List<ProductDto>>> Handle(GetAllProductByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await context.Products.Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken);


        if (!courses.Any()) return ServiceResult<List<ProductDto>>.SuccessAsOk(Enumerable.Empty<ProductDto>().ToList());


        var categories = await context.Categories.ToListAsync(cancellationToken);


        foreach (var course in courses) course.Category = categories.First(x => x.Id == course.CategoryId);


        var coursesListDtos = mapper.Map<List<ProductDto>>(courses);


        return ServiceResult<List<ProductDto>>.SuccessAsOk(coursesListDtos);
    }
}

public static class GetAllProductByUserIdEndpoint
{
    public static RouteGroupBuilder MapAllProductByUserIdEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/user/{userId}",
                async (IMediator mediator, Guid userId) =>
                    (await mediator.Send(new GetAllProductByUserIdQuery(userId))).ToActionResult())
            .WithName("GetAllProductByUserId")
            .Produces<List<ProductDto>>()
            .Produces(StatusCodes.Status404NotFound);
        return group;
    }
}