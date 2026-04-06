using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Chassis.Exceptions;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddDefaultExceptionHandling(this IServiceCollection services)
    {
        // Add exception handlers (specific first, global fallback last)
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails(options =>
        {
            // Include traceId in all RFC7807 payloads.
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            };
        });

        return services;
    }

    public static IApplicationBuilder UseDefaultExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();

        app.Use(async (httpContext, next) =>
        {
            await next();

            var response = httpContext.Response;

            // Routing/constraint misses return bare 404 without throwing; convert to NotFoundException for unified formatting.
            if (response.StatusCode != StatusCodes.Status404NotFound || response.HasStarted)
            {
                return;
            }

            if (!string.IsNullOrEmpty(response.ContentType) || response.ContentLength.GetValueOrDefault() > 0)
            {
                return;
            }

            throw new NotFoundException("The requested resource was not found.");
        });

        return app;
    }
}
