namespace BookStore.Catalog.Exceptions.Errors;

public static class AuthorError
{
    public const string AuthorNameRequired = "Author name must be provided.";
    public const string AuthorNotFound     = "Author with the specified ID was not found.";
}