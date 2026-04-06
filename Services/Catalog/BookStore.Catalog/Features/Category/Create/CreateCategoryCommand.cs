using BookStore.Catalog.Domain.AggregateModels.CategoryModel;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Catalog.Features.Category.Create;

public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;

public sealed class CreateCategoryHandler(ICategoryRepository categoryRepository, ILogger<CreateCategoryHandler> logger)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Guid> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = new Catalog.Domain.AggregateModels.CategoryModel.Category(command.Name);

        var result = await categoryRepository.AddAsync(category, cancellationToken);

        await categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return result.Id;
    }
}