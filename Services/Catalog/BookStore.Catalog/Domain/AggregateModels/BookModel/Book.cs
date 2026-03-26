using BookStore.Catalog.Domain.Events;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.AggregateModels.BookModel;

public class Book : AuditableEntity, IAggregateRoot
{
    public Book(string title, decimal price)
    {
        Id = Guid.NewGuid();
        Title = title;
        Price = price;

        RegisterDomainEvent(new BookCreateEvent(Id, Title, Price));
    }

    public string Title { get; private set; }
    public decimal Price { get; private set; }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
        LastModifiedAt = DateTime.UtcNow;

        RegisterDomainEvent(new BookUpdateEvent(this));
    }
}