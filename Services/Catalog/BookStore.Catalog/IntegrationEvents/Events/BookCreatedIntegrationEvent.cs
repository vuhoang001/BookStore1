using BuildingBlocks.Chassis.EventBus;

namespace BookStore.Catalog.IntegrationEvents.Events;

public sealed record BookCreatedIntegrationEvent(Guid BookId, string Title, decimal Price) : IntegrationEvent;

