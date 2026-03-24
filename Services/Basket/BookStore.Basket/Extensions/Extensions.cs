using BookStore.Basket.Infrastructure;
using BuildingBlocks.Chassis.EndPoints;

namespace BookStore.Basket.Extensions;

internal static class Extensions
{
    public static void AddApplicationService(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddPersistenceServices();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IBasketApiMarker>());

        // Register all endpoints
        services.AddEndpoints(typeof(IBasketApiMarker));

        services.AddVersioning();
    }
}