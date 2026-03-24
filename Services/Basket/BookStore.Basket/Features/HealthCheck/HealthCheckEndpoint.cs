using BuildingBlocks.Chassis.EndPoints;

namespace BookStore.Basket.Features.HealthCheck;

public class HealthCheckEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => TypedResults.Ok("hello world"));
    }
}