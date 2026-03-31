using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Catalog.Features.Author.Update;

public class UpdateAuthorEndpoint : IEndpoint<Ok, UpdateAuthorCommand, ISender>
{
    public async Task<Ok> HandleAsync(UpdateAuthorCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/authors/{id:guid}",
                     async (Guid id, [FromForm] UpdateAuthorCommand request, ISender sender) =>
                     await HandleAsync(
                         new UpdateAuthorCommand()
                         {
                             AuthorId   = id,
                             AuthorName = request.AuthorName,
                             AuthorBio  = request.AuthorBio,
                             Image      = request.Image,
                             ImageName  = request.ImageName
                         },
                         sender)
            )
            .Produces(StatusCodes.Status200OK)
            .WithTags(nameof(Domain.AggregateModels.AuthorModel.Author))
            .WithName(nameof(UpdateAuthorEndpoint))
            .WithSummary("Update Author")
            .WithDescription("Updates an author in the catalog system by its id")
            .MapToApiVersion(Versions.V1);
    }
}