using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Category.Get;

public class GetCategoryEndpoint : IEndpoint<Ok<CategoryDto>, GetCategoryQuery, ISender>
{
    public async Task<Ok<CategoryDto>> HandleAsync(GetCategoryQuery cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetCategoryQuery(cmd.CategoryId), cancellationToken);

        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/category/{id:guid}",
                async (
                    Guid id,
                    ISender sender
                ) => await HandleAsync(new GetCategoryQuery(id), sender)
            )
            .ProducesGet<CategoryDto>(hasNotFound: true)
            .WithTags(nameof(Domain.AggregateModels.CategoryModel.Category))
            .WithName(nameof(GetCategoryQuery))
            .WithSummary("Get category")
            .WithDescription("Get a category by identifier")
            .MapToApiVersion(Versions.V1);
    }
}