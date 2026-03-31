using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Publisher.Get;

public class
    GetPublisherEndpoint : IEndpoint<Ok<Domain.AggregateModels.PublisherModel.Publisher>, GetPublisherQuery, ISender>
{
    public async Task<Ok<Domain.AggregateModels.PublisherModel.Publisher>> HandleAsync(GetPublisherQuery cmd,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        var publisher = await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok(publisher);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/publishers/{id:guid}",
                   async (Guid id, ISender sender) =>
                       await HandleAsync(new GetPublisherQuery(id), sender)
            )
            .ProducesGet<Domain.AggregateModels.PublisherModel.Publisher>()
            .WithTags(nameof(Domain.AggregateModels.PublisherModel.Publisher))
            .WithName(nameof(GetPublisherEndpoint))
            .WithSummary("Get Publisher")
            .WithDescription("Gets a publisher from the catalog system by its id")
            .MapToApiVersion(Versions.V1);
    }
}