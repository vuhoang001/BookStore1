using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Basket.Features.Book.Create;

public class CreateBookEndpoint : IEndpoint<Ok<string>, CreateBookCommand, ISender>
{
    public async Task<Ok<string>> HandleAsync(CreateBookCommand command, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/book",
                    async (CreateBookCommand command, ISender sender) =>
                        await HandleAsync(command, sender)
            )
            .ProducesPostWithoutLocation<Guid>()
            .WithTags(nameof(Domain.AggregateModels.BookModel.Book))
            .WithName(nameof(CreateBookCommand))
            .WithSummary("Create Book")
            .WithDescription("Create a new book in the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}