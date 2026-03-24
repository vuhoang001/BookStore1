using BookStore.Basket.Domain.AggregateModels.BookModel;
using BuildingBlocks.Chassis.Repository;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Basket.Infrastructure;

public class BasketDbContext(DbContextOptions<BasketDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Book> Books => Set<Book>();

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        return true;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketDbContext).Assembly);
    }
}