using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Basket.Domain.Events.Dispatcher;

public interface IEventDispatcher
{
    Task DispatchAsync(DomainEvent @event, CancellationToken cancellationToken = default);
}