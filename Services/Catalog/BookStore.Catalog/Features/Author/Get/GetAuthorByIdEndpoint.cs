using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookStore.Catalog.Features.Author.Get;

public sealed class GetAuthorByIdEndpoint : IEndpoint<Ok<GetAuthorByIdResponse>, GetAuthorByIdQuery, ISender>
{
    private readonly ILogger<GetAuthorByIdEndpoint> _logger;

    public GetAuthorByIdEndpoint(ILogger<GetAuthorByIdEndpoint> logger)
    {
        _logger = logger;
    }

    public async Task<Ok<GetAuthorByIdResponse>> HandleAsync(
        GetAuthorByIdQuery query,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }

    private async Task<IResult> Handle(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[GetAuthorByIdEndpoint] Bắt đầu lấy author với id: {Id}", id);
        
        try
        {
            var result = await sender.Send(new GetAuthorByIdQuery(id), cancellationToken);
            _logger.LogInformation("[GetAuthorByIdEndpoint] Lấy author thành công");
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GetAuthorByIdEndpoint] Exception: {ExceptionType} - {Message}", ex.GetType().FullName, ex.Message);
            throw;
        }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/author/{id:guid}", Handle)
            .ProducesGet<GetAuthorByIdResponse>(hasNotFound: true)
            .WithTags(nameof(Domain.AggregateModels.AuthorModel.Author))
            .WithName(nameof(GetAuthorByIdQuery))
            .WithSummary("Get Author By Id")
            .WithDescription("Get author details by id")
            .MapToApiVersion(Versions.V1);
    }
}

