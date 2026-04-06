using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Chassis.Response;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Category.List;

public class ListCategoryEndpoint : IEndpoint<Ok<PaginatedItemsViewModel>, ListCategoryQuery, ISender>
{
    public async Task<Ok<PaginatedItemsViewModel>> HandleAsync(ListCategoryQuery query, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/category",
                   async ([AsParameters] ListCategoryQuery query, ISender sender) =>
                   await HandleAsync(query, sender)
            )
            .ProducesGet<PaginatedItemsViewModel>()
            .WithTags(nameof(Domain.AggregateModels.CategoryModel.Category))
            .WithName(nameof(ListCategoryQuery))
            .WithSummary("List Books")
            .WithDescription("List all books in the catalog system")
            .MapToApiVersion(Versions.V1);
    }
}