using BuildingBlocks.Constants.Core;
using FluentValidation;

namespace BookStore.Catalog.Features.Publisher.Create;

public class CreatePublisherValidator : AbstractValidator<CreatePublisherCommand>
{
    public CreatePublisherValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(DataSchemaLength.Large);
    }
}