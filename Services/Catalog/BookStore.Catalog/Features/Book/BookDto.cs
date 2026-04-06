using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BookStore.Catalog.Features.Author;
using BookStore.Catalog.Features.Category;
using BookStore.Catalog.Features.Publisher;

namespace BookStore.Catalog.Features.Book;

public sealed record BookDto(
    Guid Id,
    string? Name,
    string? Description,
    string? ImageUrl,
    decimal Price,
    decimal? PriceSale,
    Status Status,
    CategoryDto? Category,
    PublisherDto? Publisher,
    IReadOnlyList<AuthorDto> Authors,
    double AverageRating,
    int TotalReviews
);