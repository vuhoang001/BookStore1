using BookStore.Basket.Domain.Events;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Basket.Domain.AggregateModels.BookModel;

public class Book(string title, decimal price) : AuditableEntity, IAggregateRoot
{
    public string Title { get; private set; } = title;
    public decimal Price { get; private set; } = price;

    public void UpdatePrice(decimal newPrice)
    {
        Price          = newPrice;
        LastModifiedAt = DateTime.UtcNow;

        RegisterDomainEvent(new BookUpdateEvent(this));
    }
}