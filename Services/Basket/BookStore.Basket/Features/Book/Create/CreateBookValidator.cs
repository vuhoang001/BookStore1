using FluentValidation;

namespace BookStore.Basket.Features.Book.Create;

public class CreateBookValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required");

        RuleFor(x => x.Price)
            .GreaterThan(10)
            .WithMessage("Price must be greater than 10");
    }
}