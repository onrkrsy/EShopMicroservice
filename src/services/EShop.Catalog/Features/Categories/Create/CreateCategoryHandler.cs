using System.Net;
using AutoMapper;
using EShop.Catalog.Features.Categories;
using EShop.Catalog.Repositories;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EShop.Shared;

namespace EShop.Catalog.Features.Categories.Create;

public class CreateCategoryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<CreateCategoryCommand, ServiceResult<CreateCategoryResponse>>
{
    public async Task<ServiceResult<CreateCategoryResponse>> Handle(CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        // exit category check
        var existingCategory =
            await context.Categories.AnyAsync(x => x.Name == request.Name, cancellationToken);

        if (existingCategory)
            return ServiceResult<CreateCategoryResponse>.Error("Category Name Already Exists",
                $"The category name '{request.Name}' already exists.", HttpStatusCode.NotFound);


        var category = mapper.Map<Category>(request);

        category.Id = NewId.NextGuid();
        await context.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);


        return ServiceResult<CreateCategoryResponse>.SuccessAsCreated(
            new CreateCategoryResponse(category.Id.ToString()), "");
    }
}