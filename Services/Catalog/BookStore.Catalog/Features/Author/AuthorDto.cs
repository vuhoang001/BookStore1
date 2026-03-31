namespace BookStore.Catalog.Features.Author;

public sealed record AuthorDto(
    Guid Id,
    string AuthorName,
    string? AuthorBio,
    string? ImageUrl
);