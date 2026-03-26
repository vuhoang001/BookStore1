using BuildingBlocks.SharedKernel.Helpers;

namespace BuildingBlocks.Chassis.EventBus;

public abstract record IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime CreationDate { get; } = DateTimeHelper.UtcNow();
}