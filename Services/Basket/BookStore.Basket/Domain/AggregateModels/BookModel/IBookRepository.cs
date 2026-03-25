using BuildingBlocks.Chassis.Repository;

namespace BookStore.Basket.Domain.AggregateModels.BookModel;

public interface IBookRepository : IRepository<Book>, IUnitOfWork
{
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
}