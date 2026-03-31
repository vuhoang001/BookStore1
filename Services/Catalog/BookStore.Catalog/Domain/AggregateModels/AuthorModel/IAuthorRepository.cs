using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Domain.AggregateModels.AuthorModel;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default);
    void         Remove(Author author);

    Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}