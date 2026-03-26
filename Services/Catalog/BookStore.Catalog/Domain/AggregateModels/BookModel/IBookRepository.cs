using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Domain.AggregateModels.BookModel;

public interface IBookRepository : IRepository<Book>, IUnitOfWork
{
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
}