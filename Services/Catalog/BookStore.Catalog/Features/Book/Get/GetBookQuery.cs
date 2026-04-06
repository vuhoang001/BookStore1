using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BookStore.Catalog.Exceptions;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Mapper;
using Microsoft.Extensions.Caching.Hybrid;

namespace BookStore.Catalog.Features.Book.Get;

public sealed record GetBookQuery(Guid Id) : IQuery<BookDto>;

public sealed class GetBookHandler(
    IBookRepository repository,
    HybridCache cache,
    IMapper<Domain.AggregateModels.BookModel.Book, BookDto> mapper
) : IQueryHandler<GetBookQuery, BookDto>
{
    public async Task<BookDto> Handle(
        GetBookQuery request,
        CancellationToken cancellationToken
    )
    {
        var tag = nameof(Book).ToLowerInvariant();

        var bookDto = await cache.GetOrCreateAsync(
            $"{tag}:{request.Id}",
            async ctx =>
            {
                var book = await repository.GetByIdAsync(request.Id, ctx);
                if (book is null)
                    throw new CatalogDomainException("Book not found");

                return mapper.Map(book);
            },
            tags: [tag],
            cancellationToken: cancellationToken
        );

        return bookDto;
    }
}