using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BuildingBlocks.Chassis.Repository;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Catalog.Infrastructure.Repositories;

public class BookRepository(CatalogDbContext context) : IBookRepository
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

    public Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        var entry = context.Books.Add(book);
        return Task.FromResult(entry.Entity);
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await context.Books
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return result;
    }
}