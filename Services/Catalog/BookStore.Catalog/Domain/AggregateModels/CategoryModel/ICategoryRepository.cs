using BuildingBlocks.Chassis.Repository;

namespace BookStore.Catalog.Domain.AggregateModels.CategoryModel;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category>                AddAsync(Category category, CancellationToken cancellationToken);
    Task<Category?>               GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void                           Delete(Category category);
}