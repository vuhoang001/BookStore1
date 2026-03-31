using BuildingBlocks.Constants.Core;
using FluentValidation;

namespace BookStore.Catalog.Features.Author.Update;

public class UpdateAuthorValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorValidator()
    {
        RuleFor(x => x.AuthorName).NotNull().MaximumLength(DataSchemaLength.Large);
    }
}