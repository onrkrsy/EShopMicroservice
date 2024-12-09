using System.Net;
using AutoMapper;
using EShop.Catalog.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore; 
using EShop.Shared;
using EShop.Shared.Extensions;
using EShop.Shared.Services;
using EShop.Shared.Filters;
namespace EShop.Catalog.Features.Products.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string? Picture,
    Guid CategoryId) : IRequestByServiceResult<ProductDto>;

public class CreateProductCommandHandler(AppDbContext context, IMapper mapper, IIdentityService identityService)
    : IRequestHandler<CreateProductCommand, ServiceResult<ProductDto>>
{
    public async Task<ServiceResult<ProductDto>> Handle(CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var category =
            await context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null)
            return ServiceResult<ProductDto>.Error("Category Not Found",
                $"The category with id '{request.CategoryId}' was not found.", HttpStatusCode.NotFound);

        // product name is same as another product name
        var productName = await context.Products.FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);
        if (productName != null)
            return ServiceResult<ProductDto>.Error("Product Name Already Exists",
                $"The product name '{request.Name}' already exists.", HttpStatusCode.BadRequest);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Picture = request.Picture,
            UserId = identityService.GetUserId,
            CategoryId = request.CategoryId,
            CreatedTime = DateTime.UtcNow,
            Feature = new Feature {  Rating = 0, OwnerFullName = identityService.GetFullName }
        };

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        var productDto = mapper.Map<ProductDto>(product);
        return ServiceResult<ProductDto>.SuccessAsCreated(productDto, "");
    }
}

public static class CreateProductCommandEndpoint
{
    public static RouteGroupBuilder MapCreateProductCommandEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IMediator mediator, CreateProductCommand command) =>
                    (await mediator.Send(command)).ToActionResult())
            .WithName("CreateProduct")
            .Produces<ProductDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .AddEndpointFilter<ValidationFilter<CreateProductCommand>>();


        return group;
    }
}