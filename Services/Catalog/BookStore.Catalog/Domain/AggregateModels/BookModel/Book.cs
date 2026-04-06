using BookStore.Catalog.Domain.AggregateModels.CategoryModel;
using BookStore.Catalog.Domain.AggregateModels.PublisherModel;
using BookStore.Catalog.Domain.Events;
using BookStore.Catalog.Exceptions;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.AggregateModels.BookModel;

public class Book() : AuditableEntity, IAggregateRoot, ISoftDelete
{
    private readonly List<BookAuthor> _bookAuthors = [];

    public Book(string name, string? description, string? image, decimal price, decimal? priceSale, Guid categoryId,
        Guid publisherId, Guid[] authorIds) : this()

    {
        Title = !string.IsNullOrWhiteSpace(name)
            ? name
            : throw new CatalogDomainException("Book name is required.");
        Description  = description;
        Image        = image;
        Price        = new(price, priceSale);
        Status       = Status.InStock;
        CategoryId   = categoryId;
        PublisherId  = publisherId;
        _bookAuthors = [.. authorIds.Select(authorId => new BookAuthor(authorId))];
        RegisterDomainEvent(new BookCreatedEvent(this));
    }

    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public string? Image { get; private set; }
    public Price Price { get; private set; }

    public Status Status { get; private set; }

    public double AverageRating { get; private set; }

    public int TotalReviews { get; private set; }

    public Guid? CategoryId { get; private set; }

    public Category? Category { get; private set; }

    public Guid? PublisherId { get; private set; }

    public Publisher? Publisher { get; private set; }

    public IReadOnlyCollection<BookAuthor> BookAuthors => _bookAuthors.AsReadOnly();

    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        RegisterDomainEvent(new BookChangedEvent($"{nameof(Book).ToLowerInvariant()}:{Id}"));
    }


    /// <summary>
    ///  Sets the metadata for the book.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="categoryId"></param>
    /// <param name="publisherId"></param>
    /// <param name="authorIds"></param>
    /// <returns></returns>
    /// <exception cref="CatalogDomainException"></exception>
    public Book SetMetadata(string? description, Guid categoryId, Guid publisherId, Guid[] authorIds)
    {
        Description = !string.IsNullOrWhiteSpace(description)
            ? description
            : throw new CatalogDomainException("Book description is required.");

        CategoryId  = categoryId;
        PublisherId = publisherId;
        _bookAuthors.AddRange(authorIds.Select(authorId => new BookAuthor(authorId)));
        RegisterDomainEvent(new BookCreatedEvent(this));
        return this;
    }

    /// <summary>
    /// Updates the book details
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="price"></param>
    /// <param name="priceSale"></param>
    /// <param name="image"></param>
    /// <param name="categoryId"></param>
    /// <param name="publisherId"></param>
    /// <param name="authorIds"></param>
    /// <returns></returns>
    /// <exception cref="CatalogDomainException"></exception>
    public Book Update(string name, string? description, decimal price, decimal? priceSale, string? image,
        Guid categoryId, Guid publisherId, Guid[] authorIds)
    {
        var isChanged = string.Compare(Title, name, StringComparison.OrdinalIgnoreCase) != 0 ||
            string.Compare(Description, description, StringComparison.OrdinalIgnoreCase) != 0;
        Title = !string.IsNullOrWhiteSpace(name) ? name : throw new CatalogDomainException("Book name is required.");
        Price = new Price(price, priceSale);
        Description = description;
        CategoryId = categoryId;
        Image = image;

        _bookAuthors.Clear();
        _bookAuthors.AddRange(authorIds.Select(authorId => new BookAuthor(authorId)));

        if (isChanged)
        {
            RegisterDomainEvent(new BookUpdateEvent(this));
        }

        RegisterDomainEvent(new BookChangedEvent($"{nameof(Book).ToLowerInvariant()}:{Id}"));

        return this;
    }

    /// <summary>
    /// Add Ratings
    /// </summary>
    /// <param name="rating"></param>
    /// <returns></returns>
    public Book AddRating(int rating)
    {
        AverageRating = ((AverageRating * TotalReviews) + rating) / (TotalReviews + 1);

        TotalReviews++;
        RegisterDomainEvent(new BookChangedEvent($"{nameof(Book).ToLowerInvariant()}:{Id}"));
        return this;
    }


    /// <summary>
    /// Remove a rating from the book and updates the average rating.
    /// </summary>
    /// <param name="rating"></param>
    /// <returns></returns>
    public Book RemoveRating(int rating)
    {
        if (TotalReviews <= 1)
        {
            AverageRating = 0;
            TotalReviews  = 0;
        }
        else
        {
            AverageRating = ((AverageRating * TotalReviews) - rating) / (TotalReviews - 1);
            TotalReviews--;
        }

        RegisterDomainEvent(new BookChangedEvent($"{nameof(Book).ToLowerInvariant()}:{Id}"));
        return this;
    }
}