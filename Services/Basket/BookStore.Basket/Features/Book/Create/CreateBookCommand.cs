using BookStore.Basket.Infrastructure.Grpc.Services.Book;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Exceptions;

namespace BookStore.Basket.Features.Book.Create;

public sealed record CreateBookCommand(string Title, decimal Price) : ICommand<string>;

public sealed class CreateBookHandler(IBookService bookService, ILogger<CreateBookHandler> logger)
    : ICommandHandler<CreateBookCommand, string>
{
    public async Task<string> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var result = await bookService.GetBookByIdAsync("123", cancellationToken);
        return result is null ? throw new NotFoundException("Book not found") : result.Book.Id;
    }
}