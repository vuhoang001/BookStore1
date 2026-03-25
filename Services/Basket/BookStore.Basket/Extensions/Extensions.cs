using BookStore.Basket.Infrastructure;
using BuildingBlocks.Chassis.CQRS.Pipelines;
using BuildingBlocks.Chassis.EndPoints;
using FluentValidation;
using MediatR;

namespace BookStore.Basket.Extensions;

internal static class Extensions
{
    public static void AddApplicationService(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddPersistenceServices();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IBasketApiMarker>())
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // Configuration fluent validation to scan for validators in the assembly containing the IRatingApiMarker interface, including internal types.
        services.AddValidatorsFromAssemblyContaining<IBasketApiMarker>(
            includeInternalTypes: true); // Register all endpoints


        services.AddEndpoints(typeof(IBasketApiMarker));

        services.AddVersioning();
    }
}