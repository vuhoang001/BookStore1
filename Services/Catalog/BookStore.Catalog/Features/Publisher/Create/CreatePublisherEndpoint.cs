using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Publisher.Create;

public class CreatePublisherEndpoint : IEndpoint<Ok<Guid>, CreatePublisherCommand, ISender>
{
    public async Task<Ok<Guid>> HandleAsync(CreatePublisherCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/publishers",
                    async (CreatePublisherCommand cmd, ISender sender) =>
                        await HandleAsync(cmd, sender)
            )
            .ProducesPostWithoutLocation<Guid>()
            .WithTags(nameof(Catalog.Domain.AggregateModels.PublisherModel.Publisher))
            .WithName(nameof(CreatePublisherEndpoint))
            .WithSummary("Create Publisher")
            .WithDescription("Creates a new publisher in the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}