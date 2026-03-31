using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Publisher.Update;

public class UpdatePublisherEndpoint : IEndpoint<Ok, UpdatePublisherCommand, ISender>
{
    public async Task<Ok> HandleAsync(UpdatePublisherCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/publishers/{id:guid}",
                     async (Guid id, UpdatePublisherCommand request, ISender sender) =>
                         await HandleAsync(new UpdatePublisherCommand(id, request.Name), sender)
            )
            .Produces(StatusCodes.Status200OK)
            .WithTags(nameof(Domain.AggregateModels.PublisherModel.Publisher))
            .WithName(nameof(UpdatePublisherEndpoint))
            .WithSummary("Update Publisher")
            .WithDescription("Updates a publisher in the catalog system by its id")
            .MapToApiVersion(Versions.V1);
    }
}