using FluentValidation;

namespace BookStore.Catalog.Features.Author.Create;

public class CreateAuthorValidator : AbstractValidator<Domain.AggregateModels.AuthorModel.Author>
{
    public CreateAuthorValidator()
    {
        RuleFor(x => x.AuthorName).NotEmpty().MaximumLength(BuildingBlocks.Constants.Core.DataSchemaLength.Large);
    }
}