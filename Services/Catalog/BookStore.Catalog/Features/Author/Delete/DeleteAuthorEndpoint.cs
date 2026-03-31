using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Author.Delete;

public class DeleteAuthorEndpoint : IEndpoint<Ok, DeleteAuthorCommand, ISender>
{
    public async Task<Ok> HandleAsync(DeleteAuthorCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/authors/{id:guid}",
                      async (Guid id, ISender sender) =>
                          await HandleAsync(new DeleteAuthorCommand(id), sender)
            )
            .ProducesDelete()
            .WithTags(nameof(Domain.AggregateModels.AuthorModel.Author))
            .WithName(nameof(DeleteAuthorEndpoint))
            .WithSummary("Delete Author")
            .WithDescription("Deletes an author from the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}