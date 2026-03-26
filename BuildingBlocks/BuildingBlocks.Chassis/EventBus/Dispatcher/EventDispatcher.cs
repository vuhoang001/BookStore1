using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Basket.Domain.Events.Dispatcher;

public class EventDispatcher : IEventDispatcher
{
    public Task DispatchAsync(DomainEvent @event, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}