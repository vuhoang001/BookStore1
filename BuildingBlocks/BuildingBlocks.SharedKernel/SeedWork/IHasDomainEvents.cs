using MediatR;

namespace BuildingBlocks.SharedKernel.SeedWork;

public interface IHasDomainEvents : INotification
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
}