using BookStore.Catalog.Domain.AggregateModels.AuthorModel;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.AggregateModels.BookModel;

public class BookAuthor : Entity
{
    public BookAuthor(Guid authorId)
    {
        AuthorId = authorId;
    }

    public Guid AuthorId { get; private set; }
    public Guid BookId { get; private set; }
    public Author Author { get; private set; } = null!;
}