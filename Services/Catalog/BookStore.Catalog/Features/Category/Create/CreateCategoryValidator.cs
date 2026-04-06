using BuildingBlocks.Constants.Core;
using FluentValidation;

namespace BookStore.Catalog.Features.Category.Create;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(DataSchemaLength.Large);
    }
}