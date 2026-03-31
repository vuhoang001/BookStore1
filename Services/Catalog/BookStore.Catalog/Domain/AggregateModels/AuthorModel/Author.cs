using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BookStore.Catalog.Domain.Events;
using BookStore.Catalog.Exceptions;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.AggregateModels.AuthorModel;

public class Author : Entity, IAggregateRoot
{
    private readonly List<BookAuthor> _bookAuthors = [];
    public string AuthorName { get; private set; }
    public string? AuthorBio { get; private set; }
    public string? ImageUrn { get; private set; }
    
    public IReadOnlyCollection<BookAuthor> BookAuthors => _bookAuthors.AsReadOnly();


    public Author(string authorName, string? authorBio, string? imageUrn)
    {
        AuthorName = !string.IsNullOrWhiteSpace(authorName)
            ? authorName
            : throw new CatalogDomainException(AuthorError.AuthorNameRequired);

        AuthorBio = authorBio;
        ImageUrn  = imageUrn;
        RegisterDomainEvent(new AuthorCreateEvent(this));
    }

    public Author Update(string authorName, string? authorBio, string? imageUrn)
    {
        AuthorName = !string.IsNullOrWhiteSpace(authorName)
            ? authorName
            : throw new CatalogDomainException(AuthorError.AuthorNameRequired);

        AuthorBio = authorBio;
        ImageUrn  = imageUrn;
        return this;
    }
}