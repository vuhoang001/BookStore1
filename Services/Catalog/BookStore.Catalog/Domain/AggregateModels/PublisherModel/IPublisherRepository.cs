using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Domain.AggregateModels.PublisherModel;

public interface IPublisherRepository : IRepository<Publisher>
{
    Task<Publisher>                AddAsync(Publisher publisher, CancellationToken cancellationToken);
    Task<IReadOnlyList<Publisher>> ListAsync(CancellationToken cancellationToken);
    Task<Publisher?>               GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void                           Delete(Publisher publisher);
}