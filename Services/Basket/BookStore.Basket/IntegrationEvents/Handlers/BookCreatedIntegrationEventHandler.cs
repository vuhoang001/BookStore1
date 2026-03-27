using BookStore.Basket.Domain.AggregateModels.BookModel;
using BuildingBlocks.Contracts.Catalog.IntegrationEvents;
using MassTransit;

namespace BookStore.Basket.IntegrationEvents.Handlers;

public class BookCreatedIntegrationEventHandler(
    ILogger<BookCreatedIntegrationEventHandler> logger,
    IBookRepository bookRepository)
    : IConsumer<BookCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BookCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        try
        {
            var book = new Book(message.BookName, message.BookPrice);

            await bookRepository.AddAsync(book);

            await bookRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}