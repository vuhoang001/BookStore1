using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Basket.Domain.Events;

public class BookCreateEvent(Guid bookId, string title, decimal price) : DomainEvent
{
    public Guid BookId { get; } = bookId;
    public string Title { get; } = title;
    public decimal Price { get; } = price;
}