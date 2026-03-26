using BuildingBlocks.SharedKernel.SeedWork;

namespace BuildingBlocks.Chassis.EventBus.Dispatcher;

public interface IEventMapper
{
    IntegrationEvent MapToIntegrationEvent(DomainEvent @event);
}