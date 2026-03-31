using BookStore.Catalog.Infrastructure.Blob;
using BuildingBlocks.Chassis.Mapper;

namespace BookStore.Catalog.Features.Author;

public sealed class DomainToDtoMapper(IBlobService blobService)
    : IMapper<Domain.AggregateModels.AuthorModel.Author, AuthorDto>
{
    public AuthorDto Map(Domain.AggregateModels.AuthorModel.Author source)
    {
        var imageUrl = string.IsNullOrWhiteSpace(source.ImageUrn)
            ? null
            : blobService.GetFileSasUrl(source.ImageUrn);

        return new AuthorDto(
            source.Id, source.AuthorName, source.AuthorBio, imageUrl
        );
    }

    public IReadOnlyList<AuthorDto> Map(IReadOnlyList<Domain.AggregateModels.AuthorModel.Author> sources)
    {
        return sources.Count == 0 ? [] : [.. sources.Select(Map)];
    }
}