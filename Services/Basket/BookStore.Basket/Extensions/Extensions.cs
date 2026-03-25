using BookStore.Basket.Infrastructure;
using BuildingBlocks.Chassis.CQRS.Pipelines;
using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Chassis.Exceptions;
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

        services.AddEndpoints(typeof(IBasketApiMarker));

        services.AddVersioning();
    }
}