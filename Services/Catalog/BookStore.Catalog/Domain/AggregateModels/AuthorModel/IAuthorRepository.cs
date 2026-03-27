using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Domain.AggregateModels.AuthorModel;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author> AddAsync(Author book, CancellationToken cancellationToken = default);
}