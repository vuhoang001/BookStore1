using BuildingBlocks.SharedKernel.Helpers;

namespace BookStore.Basket.Domain.Events;

public abstract record IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime CreationDate { get; } = DateTimeHelper.UtcNow();
}