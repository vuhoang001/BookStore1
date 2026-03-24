using BookStore.Basket.Domain.AggregateModels.BookModel;
using BuildingBlocks.SharedKernel.SeedWork;
using MediatR;

namespace BookStore.Basket.Domain.Events;

public class BookUpdateEvent(Book book) : DomainEvent
{ 
    
}