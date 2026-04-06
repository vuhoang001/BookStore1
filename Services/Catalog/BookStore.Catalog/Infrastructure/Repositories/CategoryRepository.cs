using BookStore.Catalog.Domain.AggregateModels.CategoryModel;
using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Infrastructure.Repositories;

public class CategoryRepository(CatalogDbContext context) : ICategoryRepository
{
    private readonly CatalogDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork => _context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        return category;
    }


    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _context.Categories.FindAsync([id], cancellationToken);
        return result;
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }
}