using BookStore.Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace BookStore.Catalog.Domain.EventHandlers;

public class BookChangedEventHandler(HybridCache cache) : INotificationHandler<BookChangedEvent>
{
    public async Task Handle(BookChangedEvent notification, CancellationToken cancellationToken)
    {
        await cache.RemoveAsync(notification.Key, cancellationToken);
    }
}