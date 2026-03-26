using BookStore.Catalog.IntegrationEvents.Events;
using MassTransit;

namespace BookStore.Catalog.IntegrationEvents.Handlers;

public class BookCreatedIntegrationEventHandler(ILogger<BookCreatedIntegrationEventHandler> logger)
    : IConsumer<BookCreatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<BookCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        return Task.CompletedTask;
    }
}