using BuildingBlocks.Chassis.ApiDocument;
using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;

var builder = WebApplication.CreateBuilder(args);

// Add API versioning support
builder.Services.AddVersioning();

// Configure API documentation with explicit version list
// IMPORTANT: Versions MUST match actual mapped API versions below (V1, V2)
builder.Services.AddApiDocument("Catalog", configure: o => o
    .WithVersions("v1", "v2")
    .WithPrefix("/catalog")
    .WithDescription("Catalog Service - Product catalog and search")
);

var app = builder.Build();

// Enable Swagger UI and JSON endpoints
app.UseApiDocument();
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// Configure API versioning and route mapping
var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(Versions.V1)
    .HasApiVersion(Versions.V2)
    .ReportApiVersions()
    .Build();

var catalogGroup = app.MapGroup("/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

// V1: Simple weather forecast
catalogGroup.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                                                         new WeatherForecast
                                                         (
                                                             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                             Random.Shared.Next(-20, 55),
                                                             summaries[Random.Shared.Next(summaries.Length)]
                                                         ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecastV1")
    .WithOpenApi()
    .HasApiVersion(Versions.V1);

catalogGroup.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                                                         new WeatherForecast
                                                         (
                                                             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                             Random.Shared.Next(-20, 55),
                                                             summaries[Random.Shared.Next(summaries.Length)]
                                                         ))
            .ToArray();
        
        return TypedResults.Ok(new
        {
            version = "2.0",
            count = forecast.Length,
            items = forecast,
            timestamp = DateTime.UtcNow
        });
    })
    .WithName("GetWeatherForecastV2")
    .WithOpenApi()
    .HasApiVersion(Versions.V2);

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}