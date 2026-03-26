using BuildingBlocks.Chassis.EventBus;

namespace BuildingBlocks.Contracts.Catalog.IntegrationEvents;

public record BookCreatedIntegrationEvent(Guid BookId, string BookName, decimal BookPrice) : IntegrationEvent
{
}