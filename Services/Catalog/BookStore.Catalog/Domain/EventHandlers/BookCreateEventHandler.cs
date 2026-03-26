using BookStore.Catalog.Domain.Events;
using BuildingBlocks.Chassis.EventBus.Dispatcher;
using MediatR;

namespace BookStore.Catalog.Domain.EventHandlers;

public class BookCreateEventHandler(IEventDispatcher pub, ILogger<BookCreateEventHandler> logger)
    : INotificationHandler<BookCreateEvent>
{
    public async Task Handle(BookCreateEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("[IN-TX] Dispatching domain event to integration event for BookId {BookId}",
                              notification.BookId);
        await pub.DispatchAsync(notification, cancellationToken);

        logger.LogInformation("[IN-TX] Integration event enqueued to outbox for BookId {BookId}",
                              notification.BookId);
    }
}