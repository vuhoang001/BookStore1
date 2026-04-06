using BookStore.Catalog.Domain.AggregateModels.CategoryModel;
using BookStore.Catalog.Exceptions;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Catalog.Features.Category.Delete;

public sealed record DeleteCategoryCommand(Guid CategoryId) : ICommand;

public class DeleteCategoryHandler(ICategoryRepository categoryRepository)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new CatalogDomainException(CategoryError.CategoryNotFound);
        }

        categoryRepository.Delete(category);
        await categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}