using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Book.Get;

public sealed class GetBookEndpoint : IEndpoint<Ok<BookDto>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/book/{id:guid}",
                async (
                    Guid id,
                    ISender sender
                ) => await HandleAsync(id, sender)
            )
            .ProducesGet<BookDto>(hasNotFound: true)
            .WithTags(nameof(Book))
            .WithName(nameof(GetBookEndpoint))
            .WithSummary("Get Book")
            .WithDescription("Get a book by identifier")
            .MapToApiVersion(Versions.V1);
    }

    public async Task<Ok<BookDto>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetBookQuery(id), cancellationToken);

        return TypedResults.Ok(result);
    }
}