using BuildingBlocks.Chassis.EndPoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Book.Update;

public sealed class UpdateBookEndpoint : IEndpoint<NoContent, UpdateBookCommand, ISender>
{
    public async Task<NoContent> HandleAsync(UpdateBookCommand cmd, ISender sender,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(cmd, cancellationToken);
        return TypedResults.NoContent();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/book/{id:guid}",
                   async (Guid id, UpdateBookCommand request, ISender sender) =>
                       await HandleAsync(
                           new UpdateBookCommand(id, request.Name, request.Description, request.Image, request.Price,
                                                 request.PriceSale, request.CategoryId, request.PublisherId,
                                                 request.AuthorIds, request.IsRemoveImage), sender))
            .ProducesPut()
            .WithTags(nameof(Domain.AggregateModels.BookModel.Book))
            .WithName(nameof(UpdateBookCommand))
            .WithSummary("Update book")
            .WithDescription("Update book by id");
    }
}