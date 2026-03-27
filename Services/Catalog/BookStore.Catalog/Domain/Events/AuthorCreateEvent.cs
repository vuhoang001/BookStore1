using BookStore.Catalog.Domain.AggregateModels.AuthorModel;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.Events;

public class AuthorCreateEvent(Author author) : DomainEvent
{
}