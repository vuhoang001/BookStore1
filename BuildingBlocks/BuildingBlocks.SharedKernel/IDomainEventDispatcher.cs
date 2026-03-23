using System.Collections.Immutable;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BuildingBlocks.SharedKernel;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(ImmutableList<IHasDomainEvents> entitiesWithEvents);
}
