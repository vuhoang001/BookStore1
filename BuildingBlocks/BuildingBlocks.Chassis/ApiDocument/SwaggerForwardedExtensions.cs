using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace BuildingBlocks.Chassis.ApiDocument;

public static class SwaggerForwardedExtensions
{
    /// <summary>
    /// Thêm Swagger với server URL tự động lấy từ X-Forwarded headers của YARP.
    /// Gọi thay cho AddSwaggerGen() thông thường.
    /// </summary>
    public static IServiceCollection AddSwaggerWithForwardedPrefix(
        this IServiceCollection services,
        string fallbackPrefix = "")
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Lưu fallbackPrefix vào config để dùng ở middleware
        services.Configure<SwaggerForwardedOptions>(o =>
                                                        o.FallbackPrefix = fallbackPrefix);

        return services;
    }

    /// <summary>
    /// Bật Swagger UI và tự inject server URL từ X-Forwarded-Prefix header.
    /// Gọi thay cho UseSwagger() + UseSwaggerUI() thông thường.
    /// </summary>
    public static IApplicationBuilder UseSwaggerWithForwardedPrefix(
        this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;

        var options = app.Services
            .GetRequiredService<IOptions<SwaggerForwardedOptions>>().Value;

        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                var proto = httpReq.Headers["X-Forwarded-Proto"].FirstOrDefault()
                    ?? httpReq.Scheme;
                var host = httpReq.Headers["X-Forwarded-Host"].FirstOrDefault()
                    ?? httpReq.Host.Value;
                var prefix = httpReq.Headers["X-Forwarded-Prefix"].FirstOrDefault()
                    ?? options.FallbackPrefix;

                swagger.Servers = [new OpenApiServer() { Url = $"{proto}://{host}{prefix}" }];
            });
        });

        app.UseSwaggerUI();

        return app;
    }
}

public class SwaggerForwardedOptions
{
    public string FallbackPrefix { get; set; } = string.Empty;
}