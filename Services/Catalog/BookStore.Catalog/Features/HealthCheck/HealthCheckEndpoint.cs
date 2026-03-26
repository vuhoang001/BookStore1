using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;

namespace BookStore.Catalog.Features.HealthCheck;

public class HealthCheckEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/health", HealthCheckV1)
            .WithName("HealthCheckV1")
            .WithOpenApi()
            .HasApiVersion(Versions.V1);

        app.MapGet("/health", HealthCheckV2)
            .WithName("HealthCheckV2")
            .WithOpenApi()
            .HasApiVersion(Versions.V2);
    }

    private static IResult HealthCheckV1()
    {
        return TypedResults.Ok("hello world 1");
    }

    private static IResult HealthCheckV2()
    {
        var response = new
        {
            status      = "healthy",
            message     = "Basket Service is running",
            version     = "2.0",
            timestamp   = DateTime.UtcNow,
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
        };
        return TypedResults.Ok(response);
    }
}