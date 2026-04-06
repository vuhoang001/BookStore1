using BuildingBlocks.Chassis.EndPoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Category.Update;

public class UpdateCategoryEndpoint : IEndpoint<Ok, UpdateCategoryCommand, ISender>
{
    public async Task<Ok> HandleAsync(UpdateCategoryCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(cmd, cancellationToken);
        return TypedResults.Ok();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/category/{id:guid}",
                   async (Guid id, UpdateCategoryCommand request, ISender sender) =>
                       await HandleAsync(new UpdateCategoryCommand(id, request.Name), sender))
            .ProducesPut()
            .WithTags(nameof(Domain.AggregateModels.CategoryModel.Category))
            .WithName(nameof(UpdateCategoryCommand))
            .WithSummary("Update category")
            .WithDescription("Update category by id");
    }
}