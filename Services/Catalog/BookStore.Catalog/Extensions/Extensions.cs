using BookStore.Catalog.Features.Author.Create;
using BookStore.Catalog.Features.Book.Create;
using BookStore.Catalog.Features.Book.Update;
using BookStore.Catalog.Infrastructure;
using BookStore.Catalog.Infrastructure.Blob;
using BookStore.Catalog.Infrastructure.Grpc;
using BookStore.Catalog.Infrastructure.Services;
using BuildingBlocks.Chassis.Caching;
using BuildingBlocks.Chassis.Cors;
using BuildingBlocks.Chassis.CQRS.Pipelines;
using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Chassis.EventBus;
using BuildingBlocks.Chassis.EventBus.Dispatcher;
using BuildingBlocks.Chassis.Exceptions;
using BuildingBlocks.Chassis.Mapper;
using BuildingBlocks.Constants.Core;
using FluentValidation;
using MassTransit;
using MediatR;
using MediatR.Pipeline;

namespace BookStore.Catalog.Extensions;

internal static class Extensions
{
    public static void AddApplicationService(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddPersistenceServices();

        // AddCors
        builder.AddDefaultCors();

        services.AddScoped<IEventMapper, EventMapper>();
        services.AddScoped<IEventDispatcher, EventDispatcher>();

        services.Configure<CachingOptions>(builder.Configuration.GetSection(CachingOptions.ConfigurationSection));

        var cachingOptions = builder.Configuration
                .GetSection(CachingOptions.ConfigurationSection)
                .Get<CachingOptions>()
            ?? new CachingOptions
            {
                MaximumPayloadBytes = 1024 * 1024,
                Expiration          = TimeSpan.FromMinutes(10),
            };

        var redisConnectionString = builder.Configuration.GetConnectionString(Components.Redis);

        services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnectionString; });

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = cachingOptions.MaximumPayloadBytes;
            options.DefaultEntryOptions = new()
            {
                Expiration           = cachingOptions.Expiration,
                LocalCacheExpiration = cachingOptions.Expiration,
            };
        });

        services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<ICatalogApiMarker>();

                cfg.AddOpenBehavior(typeof(RequestPostProcessorBehavior<,>));
                cfg.AddRequestPostProcessor<UpdateBookCommandPostProcessor>();
            })
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IPipelineBehavior<UpdateBookCommand, Guid>, UpdateBookCommandProcessor>()
            .AddScoped<CreateBookCommandProcessor>()
            .AddScoped<CreateAuthorCommandPreProcessor>();

        // Configuration fluent validation to scan for validators in the assembly containing the IRatingApiMarker interface, including internal types.
        services.AddValidatorsFromAssemblyContaining<ICatalogApiMarker>(
            includeInternalTypes: true); // Register all endpoints

        // Register standard exception handling and RFC7807 problem details in one place.
        services.AddDefaultExceptionHandling();


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