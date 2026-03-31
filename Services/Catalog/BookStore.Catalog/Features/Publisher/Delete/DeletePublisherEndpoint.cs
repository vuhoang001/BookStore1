using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Publisher.Delete;

public class DeletePublisherEndpoint : IEndpoint<Ok, DeletePublisherCommand, ISender>
{
    public async Task<Ok> HandleAsync(DeletePublisherCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/publishers/{id:guid}",
                      async (Guid id, ISender sender) =>
                          await HandleAsync(new DeletePublisherCommand(id), sender)
            )
            .ProducesDelete()
            .WithTags(nameof(Domain.AggregateModels.PublisherModel.Publisher))
            .WithName(nameof(DeletePublisherEndpoint))
            .WithSummary("Delete Publisher")
            .WithDescription("Deletes a publisher from the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}