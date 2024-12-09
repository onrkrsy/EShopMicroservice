using System.Net;
using AutoMapper;
using EShop.Catalog.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EShop.Shared;
using EShop.Shared.Extensions;
using EShop.Shared.Filters;
using EShop.Shared.Services;
namespace EShop.Catalog.Features.Products.Update;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string? Picture,
    Guid CategoryId) : IRequest<ServiceResult>;

public class UpdateProductCommandHandler(AppDbContext context, IMapper mapper, IIdentityService identityService)
    : IRequestHandler<UpdateProductCommand, ServiceResult>
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ServiceResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Products
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
            return ServiceResult.Error("Product Not Found", $"The course with id '{request.Id}' was not found.",
                HttpStatusCode.NotFound);

        var category =
            await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category == null)
            return ServiceResult.Error("Category Not Found",
                $"The category with id '{request.CategoryId}' was not found.", HttpStatusCode.NotFound);

        course.Name = request.Name;
        course.Description = request.Description;
        course.Price = request.Price;
        course.Picture = request.Picture;
        course.UserId = identityService.GetUserId;
        course.CategoryId = request.CategoryId;


        //course.Feature ??= new Feature();

        //course.Feature.Duration = request.Duration;


        await _context.SaveChangesAsync(cancellationToken);

        var courseDto = _mapper.Map<ProductDto>(course);
        return ServiceResult.SuccessAsNoContent();
    }
}

public static class UpdateProductCommandEndpoint
{
    public static RouteGroupBuilder MapUpdateProductCommandEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/",
                async (IMediator mediator, UpdateProductCommand command) =>
                {
                    var response = await mediator.Send(command);
                    return response.ToActionResult();
                })
            .WithName("UpdateProduct")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<UpdateProductCommand>>();
        return group;
    }
}