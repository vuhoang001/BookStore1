using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.Events;

public class BookUpdateEvent(Book book) : DomainEvent
{ 
    
}