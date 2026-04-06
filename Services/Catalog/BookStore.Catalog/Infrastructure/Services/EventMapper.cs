using BookStore.Catalog.Domain.Events;
using BuildingBlocks.Chassis.EventBus;
using BuildingBlocks.Chassis.EventBus.Dispatcher;
using BuildingBlocks.Contracts.Catalog.IntegrationEvents;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Infrastructure.Services;

public class EventMapper : IEventMapper
{
    public IntegrationEvent MapToIntegrationEvent(DomainEvent @event)
    {
        return @event switch
        {
            BookCreatedEvent bookCreatedEvent => new BookCreatedIntegrationEvent(
                bookCreatedEvent.Book.Id,
                bookCreatedEvent.Book.Title ?? string.Empty,
                bookCreatedEvent.Book.Price.OriginalPrice
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null),
        };
    }
}
