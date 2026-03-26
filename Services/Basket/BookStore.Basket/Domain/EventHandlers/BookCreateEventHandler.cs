using BookStore.Basket.Domain.Events;
using BuildingBlocks.Chassis.EventBus.Dispatcher;
using BuildingBlocks.Contracts.Catalog.IntegrationEvents;
using MediatR;

namespace BookStore.Basket.Domain.EventHandlers;

public class BookCreateEventHandler(IEventDispatcher pub, ILogger<BookCreateEventHandler> logger)
    : INotificationHandler<BookCreateEvent>
{
    public async Task Handle(BookCreateEvent notification, CancellationToken cancellationToken)
    {
        await pub.DispatchAsync(notification, cancellationToken);

        logger.LogInformation("[IN-TX] Enqueued {EventType} to outbox for BookId {BookId}",
                              nameof(BookCreatedIntegrationEvent),
                              notification.BookId);
    }
}