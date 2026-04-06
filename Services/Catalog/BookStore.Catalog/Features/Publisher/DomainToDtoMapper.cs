namespace BookStore.Catalog.Features.Publisher;

public static class DomainToDtoMapper
{
    public static PublisherDto ToPublisherDto(this Domain.AggregateModels.PublisherModel.Publisher publisher)
    {
        return new(publisher.Id, publisher.Name);
    }

    public static IReadOnlyList<PublisherDto> ToPublisherDtos(
        this IEnumerable<Domain.AggregateModels.PublisherModel.Publisher> publishers
    )
    {
        return [.. publishers.Select(ToPublisherDto)];
    }
}
