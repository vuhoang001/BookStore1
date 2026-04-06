using System.Diagnostics.CodeAnalysis;
using BookStore.Catalog.Features.Author;
using BookStore.Catalog.Features.Category;
using BookStore.Catalog.Features.Publisher;
using BookStore.Catalog.Infrastructure.Blob;
using BuildingBlocks.Chassis.Mapper;

namespace BookStore.Catalog.Features.Book;

[ExcludeFromCodeCoverage]
public sealed class DomainToDtoMapper(IBlobService blobService)
    : IMapper<Domain.AggregateModels.BookModel.Book, BookDto>
{
    public BookDto Map(Domain.AggregateModels.BookModel.Book book)
    {
        var imageUrl = book.Image is not null ? blobService.GetFileSasUrl(book.Image) : null;

        return new(
            book.Id,
            book.Title,
            book.Description,
            imageUrl,
            book.Price?.OriginalPrice ?? 0,
            book.Price?.DiscountPrice,
            book.Status,
            book.Category?.ToCategoryDto(),
            book.Publisher?.ToPublisherDto(),
            [.. book.BookAuthors.Select(x => x.Author.ToAuthorDto())],
            book.AverageRating,
            book.TotalReviews
        );
    }

    public IReadOnlyList<BookDto> Map(IReadOnlyList<Domain.AggregateModels.BookModel.Book> models)
    {
        return models.Count == 0 ? [] : [.. models.Select(Map)];
    }
}