using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BookStore.Catalog.Exceptions;
using BuildingBlocks.Chassis.CQRS;
using MediatR;

namespace BookStore.Catalog.Features.Book.Delete;

public sealed record DeleteBookCommand(Guid Id) : ICommand;

public sealed class DeleteBookHandler(IBookRepository repository)
    : ICommandHandler<DeleteBookCommand>
{
    public async Task Handle(
        DeleteBookCommand request,
        CancellationToken cancellationToken
    )
    {
        var book = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (book is null) throw new CatalogDomainException("Book not found.");

        book.Delete();

        await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}