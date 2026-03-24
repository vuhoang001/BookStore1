using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;

namespace BuildingBlocks.Chassis.ApiDocument;

public static class ScalarGatewayExtensions
{
    /// <summary>
    /// Map Scalar UI tự động build URL dựa theo request host.
    /// Truyền vào danh sách (routePrefix, displayName).
    /// </summary>
    public static IEndpointRouteBuilder MapScalarWithServices(
        this IEndpointRouteBuilder app,
        string scalarPath = "/scalar",
        params (string Prefix, string Name)[] services)
    {
        app.MapScalarApiReference(scalarPath, (options, httpContext) =>
        {
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

            foreach (var (prefix, name) in services)
            {
                var specUrl = $"{baseUrl}{prefix}/swagger/v1/swagger.json";
                // key dùng làm id nội bộ trong Scalar, lấy prefix bỏ dấu /
                var key = prefix.TrimStart('/');
                options.AddDocument(key, name, specUrl);
            }
        });

        return app;
    }

    public static IEndpointRouteBuilder MapScalarFromYarp(
        this IEndpointRouteBuilder app,
        IConfiguration config,
        string scalarPath = "/scalar", string? subPath = "/swagger/v1/swagger.json")
    {
        var routes = config
            .GetSection("ReverseProxy:Routes")
            .GetChildren();

        app.MapScalarApiReference(scalarPath, (options, httpContext) =>
        {
            var request = httpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            foreach (var route in routes)
            {
                var routeId = route.Key;

                var path = route.GetSection("Match:Path").Value;

                if (string.IsNullOrEmpty(path))
                    continue;

                var prefix = path.Split("/{")[0];

                if (string.IsNullOrEmpty(prefix))
                    continue;

                var key = prefix.Trim('/');

                var displayName = $"{key.ToUpper()} API";

                var swaggerUrl = "";

                swaggerUrl = subPath is null
                    ? $"{baseUrl}{prefix}/swagger/v1/swagger.json"
                    : $"{baseUrl}{prefix}{subPath}";


                options.AddDocument(key, displayName, swaggerUrl);
            }
        });

        return app;
    }
}