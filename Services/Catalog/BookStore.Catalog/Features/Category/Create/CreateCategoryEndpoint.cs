using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Catalog.Features.Category.Create;

public class CreateCategoryEndpoint : IEndpoint<Ok<Guid>, CreateCategoryCommand, ISender>
{
    public async Task<Ok<Guid>> HandleAsync(CreateCategoryCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/category",
                    async ([FromBody] CreateCategoryCommand cmd, ISender sender, CancellationToken cancellationToken) =>
                        await HandleAsync(cmd, sender, cancellationToken)
            )
            .ProducesPostWithoutLocation<Guid>()
            .WithTags(nameof(Domain.AggregateModels.CategoryModel.Category))
            .WithName(nameof(CreateCategoryCommand))
            .WithSummary("Create new category")
            .WithDescription("Create new category with name")
            .MapToApiVersion(Versions.V1);
    }
}