using BuildingBlocks.Chassis.EventBus;

namespace BookStore.Basket.IntegrationEvents;

public sealed record BookCreatedIntegrationEvent(Guid BookId, string Title, decimal Price) : IntegrationEvent;

