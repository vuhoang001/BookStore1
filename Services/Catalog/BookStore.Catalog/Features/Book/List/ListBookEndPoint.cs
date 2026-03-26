using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Chassis.Response;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Book.List;

public class ListBookEndPoint : IEndpoint<Ok<PaginatedItemsViewModel>, ListBookQuery, ISender>
{
    public async Task<Ok<PaginatedItemsViewModel>> HandleAsync(ListBookQuery query, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/book",
                   async ([AsParameters] ListBookQuery query, ISender sender) =>
                   await HandleAsync(query, sender)
            )
            .ProducesGet<PaginatedItemsViewModel>()
            .WithTags(nameof(Catalog.Domain.AggregateModels.BookModel.Book))
            .WithName(nameof(ListBookQuery))
            .WithSummary("List Books")
            .WithDescription("List all books in the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}