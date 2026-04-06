namespace BookStore.Catalog.Exceptions.Errors;

public static class CategoryError
{
    public const string CategoryNameRequired = "Category name must be provided.";
    public const string CategoryNotFound     = "Category with the specified ID was not found.";
}