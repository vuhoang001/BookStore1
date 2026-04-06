using BookStore.Catalog.Domain.AggregateModels.CategoryModel;
using BookStore.Catalog.Exceptions;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Catalog.Features.Category.Get;

public sealed record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryDto>;

public class GetCategoryHandler(ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoryQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryQuery query,
        CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(query.CategoryId, cancellationToken);

        if (category is null)
            throw new CatalogDomainException(CategoryError.CategoryNotFound);

        return category.ToCategoryDto();
    }
}