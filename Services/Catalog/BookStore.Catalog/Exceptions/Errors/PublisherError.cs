namespace BookStore.Catalog.Exceptions.Errors;

public static class PublisherError
{
    public const string PublisherNameRequired = "Publisher name must be provided.";
    public const string PublisherNotFound     = "Publisher with the specified ID was not found.";
}