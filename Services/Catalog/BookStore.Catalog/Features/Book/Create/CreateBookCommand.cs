using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Catalog.Features.Book.Create;

public sealed record CreateBookCommand(string Title, decimal Price) : ICommand<Guid>;

public sealed class CreateBookHandler(IBookRepository bookRepository, ILogger<CreateBookHandler> logger)
    : ICommandHandler<CreateBookCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("[ENDPOINT] CreateBookHandler called with Title: {Title}, Price: {Price}", 
                              command.Title, command.Price);

        var result = await bookRepository.AddAsync(
            new Catalog.Domain.AggregateModels.BookModel.Book(command.Title, command.Price),
            cancellationToken);

        logger.LogInformation("[DB-BEFORE-SAVE] Book added to repository with Id: {BookId}", result.Id);

        await bookRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        logger.LogInformation("[DB-SAVED] Book saved to database with Id: {BookId}", result.Id);

        return result.Id;
    }
}