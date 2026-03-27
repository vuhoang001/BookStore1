using BookStore.Catalog.Domain.Events;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.AggregateModels.AuthorModel;

public class Author : Entity, IAggregateRoot
{
    public string AuthorName { get; private set; }
    public string? AuthorBio { get; private set; }
    public string? ImageUrn { get; private set; }

    private Author(string authorName, string? authorBio, string? imageUrn)
    {
        AuthorName = authorName;
        AuthorBio  = authorBio;
        ImageUrn   = imageUrn;
        RegisterDomainEvent(new AuthorCreateEvent(this));
    }

    public static Author Create(string authorName, string? authorBio, string? imageUrn)
    {
        var author = new Author(authorName, authorBio, imageUrn);
        return author;
    }
}