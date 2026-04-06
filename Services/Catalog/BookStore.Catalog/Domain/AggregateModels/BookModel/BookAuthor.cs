using BookStore.Catalog.Domain.AggregateModels.AuthorModel;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.AggregateModels.BookModel;

public class BookAuthor() : Entity
{
    public BookAuthor(Guid authorId) : this()
    {
        AuthorId = authorId;
    }

    public Guid AuthorId { get; private set; }

    public Book Book { get; private set; } = null!;
    public Author Author { get; private set; } = null!;
}