using System.Net.Mime;
using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Catalog.Features.Author.Create;

public class CreateAuthorEndpoint : IEndpoint<Ok<Guid>, CreateAuthorCommand, ISender>
{
    public async Task<Ok<Guid>> HandleAsync(CreateAuthorCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(cmd, cancellationToken);

        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/author",
                    async ([FromForm] CreateAuthorCommand command, ISender sender, CancellationToken cancellationToken) =>
                    await HandleAsync(command, sender, cancellationToken)
            )
            .Accepts<CreateAuthorCommand>(MediaTypeNames.Multipart.FormData)
            .ProducesPostWithoutLocation<Guid>()
            .WithTags(nameof(Domain.AggregateModels.AuthorModel.Author))
            .WithName(nameof(CreateAuthorCommand))
            .WithSummary("Create Author")
            .WithDescription("Create a new author in the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}