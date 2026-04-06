using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Domain.AggregateModels.BookModel;

public interface IBookRepository : IRepository<Book>
{
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}