using BookStore.Catalog.Domain.AggregateModels.PublisherModel;
using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Infrastructure.Repositories;

public class PublisherRepository(CatalogDbContext context) : IPublisherRepository
{
    private readonly CatalogDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork => _context;

    public async Task<Publisher> AddAsync(Publisher publisher, CancellationToken cancellationToken)
    {
        var entry = await _context.Publishers.AddAsync(publisher, cancellationToken);
        return entry.Entity;
    }

    public Task<IReadOnlyList<Publisher>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Publisher?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _context.Publishers.FindAsync([id], cancellationToken);
        return result;
    }

    public void Delete(Publisher publisher)
    {
        _context.Publishers.Remove(publisher);
    }
}