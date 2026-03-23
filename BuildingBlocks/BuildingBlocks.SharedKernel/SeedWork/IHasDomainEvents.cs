using MediatR;

namespace BuildingBlocks.SharedKernel.SeedWork;

public interface IHasDomainEvents : INotification
{
    public DateTime DateOccurred { get; protected set; }
}