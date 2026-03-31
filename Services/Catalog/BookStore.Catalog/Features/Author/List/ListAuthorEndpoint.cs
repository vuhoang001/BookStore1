using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Chassis.Response;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Author.List;

public class ListAuthorEndpoint : IEndpoint<Ok<PaginatedItemsViewModel>, ListAuthorQuery, ISender>
{
    public async Task<Ok<PaginatedItemsViewModel>> HandleAsync(ListAuthorQuery cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/authors",
                   async ([AsParameters] ListAuthorQuery query, ISender sender) =>
                   await HandleAsync(query, sender)
            )
            .ProducesGet<PaginatedItemsViewModel>()
            .WithTags(nameof(Domain.AggregateModels.AuthorModel.Author))
            .WithName(nameof(ListAuthorQuery))
            .WithSummary("List Authors")
            .WithDescription("List all authors in the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}