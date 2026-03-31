using BuildingBlocks.Constants.Core;
using FluentValidation;

namespace BookStore.Catalog.Features.Publisher.Update;

public class UpdatePublisherValidator : AbstractValidator<UpdatePublisherCommand>
{
   public UpdatePublisherValidator()
   {
      RuleFor(x => x.Name.Trim()).NotEmpty().MaximumLength(DataSchemaLength.Large);
   } 
}