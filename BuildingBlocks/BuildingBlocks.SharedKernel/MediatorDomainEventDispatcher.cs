using System.Collections.Immutable;
using BuildingBlocks.SharedKernel.SeedWork;
using MediatR;

namespace BuildingBlocks.SharedKernel;

public class MediatorDomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
{
    public async Task DispatchAndClearEvents(ImmutableList<IHasDomainEvents> entitiesWithEvents)
    {
        foreach (var entity in entitiesWithEvents)
        {
            if (entity is not HasDomainEvents hasDomainEvents)
            {
                continue;
            }

            DomainEvent[] events = [.. hasDomainEvents.DomainEvents];
            hasDomainEvents.ClearDomainEvents();
            foreach (var domainEvent in events)
            {
                await publisher.Publish(domainEvent);
            }
        }
    }
}