using BuildingBlocks.Chassis.EndPoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Category.Delete;

public class DeleteCategoryEndpoint : IEndpoint<Ok, DeleteCategoryCommand, ISender>
{
    public async Task<Ok> HandleAsync(DeleteCategoryCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/category/{id:guid}",
                      async (Guid id, ISender sender) =>
                          await HandleAsync(new DeleteCategoryCommand(id), sender))
            .ProducesDelete()
            .WithTags(nameof(Domain.AggregateModels.CategoryModel.Category))
            .WithName(nameof(DeleteCategoryCommand))
            .WithSummary("Delete category")
            .WithDescription("Delete category by id");
    }
}