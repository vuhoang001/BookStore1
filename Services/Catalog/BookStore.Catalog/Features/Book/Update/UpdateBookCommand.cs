using System.Text.Json.Serialization;
using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BookStore.Catalog.Exceptions;
using BuildingBlocks.Chassis.CQRS;
using MediatR;

namespace BookStore.Catalog.Features.Book.Update;

public sealed record UpdateBookCommand(
    Guid Id,
    string Name,
    string Description,
    IFormFile? Image,
    decimal Price,
    decimal? PriceSale,
    Guid CategoryId,
    Guid PublisherId,
    Guid[] AuthorIds,
    bool IsRemoveImage = false
) : ICommand<Guid>
{
    [JsonIgnore] public string? ImageUrn { get; set; }
}

public sealed class UpdateBookHandler(IBookRepository repository)
    : ICommandHandler<UpdateBookCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateBookCommand request,
        CancellationToken cancellationToken
    )
    {
        var book = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (book is null) throw new CatalogDomainException("Book not found.");


        var imageName = request.IsRemoveImage ? null : request.ImageUrn ?? book.Image;

        book.Update(
            request.Name,
            request.Description,
            request.Price,
            request.PriceSale,
            imageName,
            request.CategoryId,
            request.PublisherId,
            request.AuthorIds
        );

        await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return book.Id;
    }
}