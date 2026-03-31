using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Chassis.Response;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Publisher.List;

public class ListPublisherEndpoint : IEndpoint<Ok<PaginatedItemsViewModel>, ListPublisherQuery, ISender>
{
    public async Task<Ok<PaginatedItemsViewModel>> HandleAsync(ListPublisherQuery query, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/publishers",
                   async ([AsParameters] ListPublisherQuery query, ISender sender) =>
                   await HandleAsync(query, sender)
            )
            .ProducesGet<PaginatedItemsViewModel>()
            .WithTags(nameof(Domain.AggregateModels.PublisherModel.Publisher))
            .WithName(nameof(ListPublisherQuery))
            .WithSummary("List publishers")
            .WithDescription("List all publisher in the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}