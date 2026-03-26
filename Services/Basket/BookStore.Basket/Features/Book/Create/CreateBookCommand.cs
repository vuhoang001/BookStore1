using BookStore.Basket.Domain.AggregateModels.BookModel;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Basket.Features.Book.Create;

public sealed record CreateBookCommand(string Title, decimal Price) : ICommand<Guid>;

public sealed class CreateBookHandler(IBookRepository bookRepository, ILogger<CreateBookHandler> logger)
    : ICommandHandler<CreateBookCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var result = await bookRepository.AddAsync(
            new Domain.AggregateModels.BookModel.Book(command.Title, command.Price),
            cancellationToken);


        await bookRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);


        return result.Id;
    }
}