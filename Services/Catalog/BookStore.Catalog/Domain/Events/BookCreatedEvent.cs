using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.Events;

public class BookCreatedEvent(Book book) : DomainEvent
{
    public Book Book { get; init; } = book;
}