using System.Diagnostics.CodeAnalysis;
using BookStore.Catalog.Exceptions;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.AggregateModels.CategoryModel;

public class Category(string categoryName) : Entity, IAggregateRoot
{
    [DisallowNull]
    public string? CategoryName { get; private set; } = !string.IsNullOrWhiteSpace(categoryName)
        ? categoryName
        : throw new CatalogDomainException(CategoryError.CategoryNameRequired);

    public void Update(string categoryName)
    {
        CategoryName = !string.IsNullOrWhiteSpace(categoryName)
            ? categoryName
            : throw new CatalogDomainException(CategoryError.CategoryNameRequired);
    }
}