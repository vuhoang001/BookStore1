using BookStore.Basket.Infrastructure;
using BookStore.Basket.Infrastructure.Grpc;
using BuildingBlocks.Chassis.Cors;
using BuildingBlocks.Chassis.CQRS.Pipelines;
using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Chassis.EventBus;
using BuildingBlocks.Chassis.Exceptions;
using FluentValidation;
using MassTransit;
using MediatR;

namespace BookStore.Basket.Extensions;

internal static class Extensions
{
    public static void AddApplicationService(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        // AddCors
        builder.AddDefaultCors();

        builder.AddPersistenceServices();
        builder.AddGrpcServices();

        // services.AddScoped<IEventMapper, EventMapper>();
        // services.AddScoped<IEventDispatcher, EventDispatcher>();


        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IBasketApiMarker>())
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // Configuration fluent validation to scan for validators in the assembly containing the IRatingApiMarker interface, including internal types.
        services.AddValidatorsFromAssemblyContaining<IBasketApiMarker>(
            includeInternalTypes: true); // Register all endpoints


        // Register standard exception handling and RFC7807 problem details in one place.
        services.AddDefaultExceptionHandling();


        // Configure EventBus
        builder.AddEventBus(
            typeof(IBasketApiMarker),
            cfg =>
            {
                cfg.AddEntityFrameworkOutbox<BasketDbContext>(outbox =>
                {
                    outbox.QueryDelay = TimeSpan.FromSeconds(1);

                    outbox.DuplicateDetectionWindow = TimeSpan.FromMinutes(5);

                    outbox.UseSqlServer();
                    outbox.UseBusOutbox();
                });

                cfg.AddConfigureEndpointsCallback((context, _, configurator) =>
                                                      configurator.UseEntityFrameworkOutbox<BasketDbContext>(context)
                );
            }
        );


        services.AddEndpoints(typeof(IBasketApiMarker));

        services.AddVersioning();
    }
}