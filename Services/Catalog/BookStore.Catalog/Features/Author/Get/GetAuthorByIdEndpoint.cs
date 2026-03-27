using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Author.Get;

public sealed class GetAuthorByIdEndpoint : IEndpoint<Ok<GetAuthorByIdResponse>, GetAuthorByIdQuery, ISender>
{
    public async Task<Ok<GetAuthorByIdResponse>> HandleAsync(
        GetAuthorByIdQuery query,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/author/{id:guid}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    await HandleAsync(new GetAuthorByIdQuery(id), sender, cancellationToken)
            )
            .ProducesGet<GetAuthorByIdResponse>(hasNotFound: true)
            .WithTags(nameof(Domain.AggregateModels.AuthorModel.Author))
            .WithName(nameof(GetAuthorByIdQuery))
            .WithSummary("Get Author By Id")
            .WithDescription("Get author details by id")
            .MapToApiVersion(Versions.V1);
    }
}

