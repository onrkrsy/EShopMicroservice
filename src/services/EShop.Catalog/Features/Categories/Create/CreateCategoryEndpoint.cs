using EShop.Shared.Filters;
using EShop.Shared.Extensions;
using MediatR;

namespace EShop.Catalog.Features.Categories.Create;

public static class CreateCategoryEndpoint
{
    public static RouteGroupBuilder MapCreateCategoryEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (CreateCategoryCommand request, IMediator mediator) =>
                    (await mediator.Send(request)).ToActionResult())
            .WithName("CreateCategory") // Endpoint'e isim verir
            .Produces<
                CreateCategoryResponse>(StatusCodes
                .Status201Created).MapToApiVersion(1.0)
            .AddEndpointFilter<ValidationFilter<CreateCategoryCommand>>();


        // Üretilen yanıt türünü ve durum kodunu belirtir
        //.ProducesValidationProblem() // Doğrulama hatalarını belirtir
        //.RequireAuthorization() // Yetkilendirme gerektirir
        // Endpoint'i belirli bir etiketle gruplar


        return group;
    }
}