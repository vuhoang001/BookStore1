using BookStore.Basket.Domain.AggregateModels.BookModel;
using BuildingBlocks.Contracts.Catalog.IntegrationEvents;
using MassTransit;

namespace BookStore.Basket.IntegrationEvents.Handlers;

public class BookCreatedIntegrationEventHandlerV2(
    ILogger<BookCreatedIntegrationEventHandler> logger,
    IBookRepository bookRepository)
    : IConsumer<BookCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BookCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        try
        {
            logger.LogInformation(
                "[POST-COMMIT] Received {EventType} for BookId {BookId}, Name: {BookName}, Price: {BookPrice}",
                nameof(BookCreatedIntegrationEvent),
                message.BookId,
                message.BookName,
                message.BookPrice
            );

            var book = new Book(message.BookName, message.BookPrice);

            logger.LogInformation("[POST-COMMIT] Created Book entity with BookId {BookId}", book.Id);

            await bookRepository.AddAsync(book);

            logger.LogInformation("[POST-COMMIT] Added book to repository, about to save");

            await bookRepository.UnitOfWork.SaveChangesAsync();

            logger.LogInformation("[POST-COMMIT] Book {BookId} saved to Basket DB successfully", message.BookId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[POST-COMMIT-ERROR] Failed to consume {EventType} for BookId {BookId}",
                            nameof(BookCreatedIntegrationEvent),
                            message.BookId);
            throw;
        }
    }
}