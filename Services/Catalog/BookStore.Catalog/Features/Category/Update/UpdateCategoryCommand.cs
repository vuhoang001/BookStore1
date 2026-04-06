using BookStore.Catalog.Domain.AggregateModels.CategoryModel;
using BookStore.Catalog.Exceptions;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Catalog.Features.Category.Update;

public sealed record UpdateCategoryCommand(Guid CategoryId, string Name) : ICommand<Guid>;

public class UpdateCategoryHandler(ICategoryRepository categoryRepository)
    : ICommandHandler<UpdateCategoryCommand, Guid>
{
    public async Task<Guid> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new CatalogDomainException(CategoryError.CategoryNotFound);
        }

        category.Update(command.Name);

        await categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return category.Id;
    }
}