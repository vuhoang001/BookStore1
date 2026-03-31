using BookStore.Catalog.Domain.AggregateModels.PublisherModel;
using BookStore.Catalog.Exceptions.Errors;
using BookStore.Catalog.Infrastructure;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Catalog.Features.Publisher.Get;

public record GetPublisherQuery(Guid PublisherId) : IQuery<Domain.AggregateModels.PublisherModel.Publisher>;

public class GetPublisherHandler(CatalogDbContext context)
    : IQueryHandler<GetPublisherQuery, Domain.AggregateModels.PublisherModel.Publisher>
{
    public async Task<Domain.AggregateModels.PublisherModel.Publisher> Handle(GetPublisherQuery query,
        CancellationToken cancellationToken)

    {
        var publisher = await context.Publishers.FirstOrDefaultAsync(x => x.Id == query.PublisherId, cancellationToken);
        return publisher ?? throw new NotFoundException(PublisherError.PublisherNotFound);
    }
}