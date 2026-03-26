using BuildingBlocks.SharedKernel.SeedWork;

namespace BuildingBlocks.Chassis.EventBus.Dispatcher;

public interface IEventDispatcher
{
    Task DispatchAsync(DomainEvent @event, CancellationToken cancellationToken = default);
}