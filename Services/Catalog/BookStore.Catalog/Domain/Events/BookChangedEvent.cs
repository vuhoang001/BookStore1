using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.Events;

public class BookChangedEvent(string key) : DomainEvent
{
   public string Key { get; init; } = key;
}