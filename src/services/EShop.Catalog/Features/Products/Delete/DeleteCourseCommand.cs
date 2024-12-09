using System.Net;
using EShop.Catalog.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EShop.Shared;
using EShop.Shared.Extensions;
namespace EShop.Catalog.Features.Products.Delete;

public record DeleteProductCommand(Guid Id) : IRequestByServiceResult;

public class DeleteProductCommandHandler(AppDbContext context) : IRequestHandler<DeleteProductCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var course = await context.Products.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
            return ServiceResult.Error("Product Not Found", $"The course with id '{request.Id}' was not found.",
                HttpStatusCode.NotFound);

        context.Products.Remove(course);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}

public static class DeleteProductCommandEndpoint
{
    public static RouteGroupBuilder MapDeleteProductCommandEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}",
                async (IMediator mediator, Guid id) =>
                {
                    var response = await mediator.Send(new DeleteProductCommand(id));
                    return response.ToActionResult();
                })
            .WithName("DeleteProduct")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}