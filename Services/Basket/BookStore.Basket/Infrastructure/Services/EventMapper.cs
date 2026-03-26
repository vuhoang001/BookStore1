using BookStore.Basket.Domain.Events;
using BuildingBlocks.Chassis.EventBus;
using BuildingBlocks.Chassis.EventBus.Dispatcher;
using BuildingBlocks.Contracts.Catalog.IntegrationEvents;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Basket.Infrastructure.Services;

public class EventMapper : IEventMapper
{
    public IntegrationEvent MapToIntegrationEvent(DomainEvent @event)
    {
        return @event switch
        {
            BookCreateEvent bookCreateEvent => new BookCreatedIntegrationEvent(
                bookCreateEvent.BookId,
                bookCreateEvent.Title,
                bookCreateEvent.Price
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null),
        };
    }
}