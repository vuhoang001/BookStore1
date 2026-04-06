using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.Constants.Core;
using FluentValidation;

namespace BookStore.Catalog.Features.Category.Update;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
   public UpdateCategoryValidator()
   {
     RuleFor(x => x.Name).MaximumLength(DataSchemaLength.Large) ;
   } 
}