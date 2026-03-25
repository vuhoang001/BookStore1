using BookStore.Basket.Domain.AggregateModels.BookModel;
using BuildingBlocks.Chassis.Repository;

namespace BookStore.Basket.Infrastructure.Repositories;

public class BookRepository(BasketDbContext context) : IBookRepository
{
    private readonly BasketDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    public void Dispose()
    {
        // TODO release managed resources here
    }

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
}