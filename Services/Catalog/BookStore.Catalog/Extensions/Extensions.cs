using BookStore.Catalog.Features.Author.Create;
using BookStore.Catalog.Infrastructure;
using BookStore.Catalog.Infrastructure.Blob;
using BookStore.Catalog.Infrastructure.Grpc;
using BookStore.Catalog.Infrastructure.Services;
using BuildingBlocks.Chassis.CQRS.Pipelines;
using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Chassis.EventBus;
using BuildingBlocks.Chassis.EventBus.Dispatcher;
using BuildingBlocks.Chassis.Exceptions;
using BuildingBlocks.Chassis.Mapper;
using FluentValidation;
using MassTransit;
using MediatR;

namespace BookStore.Catalog.Extensions;

internal static class Extensions
{
    public static void AddApplicationService(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddPersistenceServices();

        // Keep domain handlers decoupled from MassTransit by resolving through dispatcher abstractions.
        services.AddScoped<IEventMapper, EventMapper>();
        services.AddScoped<IEventDispatcher, EventDispatcher>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ICatalogApiMarker>())
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IPipelineBehavior<CreateAuthorCommand, Guid>, CreateAuthorCommandPreProcessor>();

        // Configuration fluent validation to scan for validators in the assembly containing the IRatingApiMarker interface, including internal types.
        services.AddValidatorsFromAssemblyContaining<ICatalogApiMarker>(
            includeInternalTypes: true); // Register all endpoints

 // Add exception handlers (specific first, global fallback last)
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails(options =>
        {
            // Customize problem details to include traceId by default
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            };
        });


        // Configure EventBus
        builder.AddEventBus(
            typeof(ICatalogApiMarker),
            cfg =>
            {
                cfg.AddEntityFrameworkOutbox<CatalogDbContext>(outbox =>
                {
                    // Increase polling interval temporarily so you can observe records in OutboxMessage before dispatch.
                    outbox.QueryDelay = TimeSpan.FromSeconds(1);

                    outbox.DuplicateDetectionWindow = TimeSpan.FromMinutes(5);

                    outbox.UseSqlServer();
                    outbox.UseBusOutbox();
                });

                cfg.AddConfigureEndpointsCallback((context, _, configurator) =>
                                                      configurator.UseEntityFrameworkOutbox<CatalogDbContext>(context)
                );
            }
        );

        // Add mapper profiles
        services.AddMapper(typeof(ICatalogApiMarker));

        // Add minio blob storage
        builder.AddMinioBlobStorage();
        services.AddEndpoints(typeof(ICatalogApiMarker));

        // Add gRPC services
        builder.AddGrpcServices();

        services.AddVersioning();
    }
}