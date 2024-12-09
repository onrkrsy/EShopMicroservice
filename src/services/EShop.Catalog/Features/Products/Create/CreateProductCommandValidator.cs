using FluentValidation;

namespace EShop.Catalog.Features.Products.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Length(5, 100).WithMessage("{PropertyName} must be between 5 and 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Length(5, 5000).WithMessage("{PropertyName} must be between 5 and 5000 characters");


        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");


        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} cannot be an empty GUID");
    }
}