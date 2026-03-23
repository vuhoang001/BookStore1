using BuildingBlocks.SharedKernel.Helpers;
using MediatR;

namespace BuildingBlocks.SharedKernel.SeedWork;

public class DomainEvent : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTimeHelper.UtcNow();
}