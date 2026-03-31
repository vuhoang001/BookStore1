using BookStore.Catalog.Infrastructure;
using BookStore.Catalog.Infrastructure.Blob;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Catalog.Features.Author.Get;

public sealed record GetAuthorByIdQuery(Guid Id) : IQuery<GetAuthorByIdResponse>;

public sealed record GetAuthorByIdResponse(
    Guid Id,
    string AuthorName,
    string? AuthorBio,
    string? ImageUrn,
    string? ImageUrl
);

public sealed class GetAuthorByIdHandler(CatalogDbContext context, IBlobService blobService)
    : IQueryHandler<GetAuthorByIdQuery, GetAuthorByIdResponse>
{
    public async Task<GetAuthorByIdResponse> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (author is null)
        {
            throw NotFoundException.For<Domain.AggregateModels.AuthorModel.Author>(request.Id);
        }

        var imageUrl = string.IsNullOrWhiteSpace(author.ImageUrn)
            ? null
            : blobService.GetFileSasUrl(author.ImageUrn);

        return new GetAuthorByIdResponse(
            author.Id,
            author.AuthorName,
            author.AuthorBio,
            author.ImageUrn,
            imageUrl
        );
    }
}