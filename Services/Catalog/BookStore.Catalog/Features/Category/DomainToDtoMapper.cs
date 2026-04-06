namespace BookStore.Catalog.Features.Category;

public static class DomainToDtoMapper
{
    public static CategoryDto ToCategoryDto(this Domain.AggregateModels.CategoryModel.Category category)
    {
        return new(category.Id, category.CategoryName);
    }

    public static IReadOnlyList<CategoryDto> ToCategoryDtos(this IEnumerable<Domain.AggregateModels.CategoryModel.Category> categories)
    {
        return [.. categories.Select(ToCategoryDto)];
    }
}
