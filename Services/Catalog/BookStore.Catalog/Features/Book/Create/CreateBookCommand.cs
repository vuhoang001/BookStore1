using System.Text.Json.Serialization;
using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Catalog.Features.Book.Create;

public sealed record CreateBookCommand(
    string Title,
    string Description,
    IFormFile? Image,
    decimal Price,
    decimal? PriceSale,
    Guid CategoryId,
    Guid PublisherId,
    Guid[] AuthorIds
) : ICommand<Guid>
{
    [JsonIgnore] public string? ImageName { get; set; }
};

public sealed class CreateBookHandler(IBookRepository bookRepository, ILogger<CreateBookHandler> logger)
    : ICommandHandler<CreateBookCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Catalog.Domain.AggregateModels.BookModel.Book(
            request.Title,
            request.Description,
            request.ImageName,
            request.Price,
            request.PriceSale,
            request.CategoryId,
            request.PublisherId,
            request.AuthorIds
        );
        var result = await bookRepository.AddAsync(
            book, cancellationToken);

        await bookRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return result.Id;
    }
}