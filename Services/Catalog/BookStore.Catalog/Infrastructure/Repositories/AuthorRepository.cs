using BookStore.Catalog.Domain.AggregateModels.AuthorModel;
using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Infrastructure.Repositories;

public class AuthorRepository(CatalogDbContext context) : IAuthorRepository
{
    private readonly CatalogDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));


    public IUnitOfWork UnitOfWork => _context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default)
    {
        var entry = context.Authors.Add(author);
        return Task.FromResult(entry.Entity);
    }

    public void Remove(Author author)
    {
        context.Authors.Remove(author);
    }

    public async Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await context.Authors.FindAsync([id], cancellationToken);
        return result;
    }
}