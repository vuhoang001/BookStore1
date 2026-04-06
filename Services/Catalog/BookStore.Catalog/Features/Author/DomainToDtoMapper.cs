using BuildingBlocks.Chassis.Mapper;

namespace BookStore.Catalog.Features.Author;

public sealed class AuthorMapper : IMapper<Domain.AggregateModels.AuthorModel.Author, AuthorDto>
{
    public AuthorDto Map(Domain.AggregateModels.AuthorModel.Author source)
    {
        return source.ToAuthorDto();
    }

    public IReadOnlyList<AuthorDto> Map(IReadOnlyList<Domain.AggregateModels.AuthorModel.Author> sources)
    {
        return sources.Count == 0 ? [] : [.. sources.Select(Map)];
    }
}

public static class DomainToDtoMapper
{
    public static AuthorDto ToAuthorDto(this Domain.AggregateModels.AuthorModel.Author author)
    {
        return new(author.Id, author.AuthorName);
    }

    public static IReadOnlyList<AuthorDto> ToAuthorDtos(this IEnumerable<Domain.AggregateModels.AuthorModel.Author> authors)
    {
        return [.. authors.Select(ToAuthorDto)];
    }
}
