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
            BookCreateEvent bookCreateEvent => new BookCreatedIntegrationEvent(
                bookCreateEvent.BookId,
                bookCreateEvent.Title,  // Maps to BookName parameter
                bookCreateEvent.Price   // Maps to BookPrice parameter
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null),
        };
    }
}