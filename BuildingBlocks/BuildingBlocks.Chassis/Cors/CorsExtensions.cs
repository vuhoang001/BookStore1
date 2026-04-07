using BuildingBlocks.Chassis.Utils;
using BuildingBlocks.Constants.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Chassis.Cors;

public static class CorsExtensions
{
    private const string AllowAllCorsPolicy      = "AllowAll";
    private const string AllowSpecificCorsPolicy = "AllowSpecific";

    public static void AddDefaultCors(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowAllCorsPolicy, policyBuilder =>
                {
                    policyBuilder
                        .SetIsOriginAllowed(option => new Uri(option).Host == Network.Localhost)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
        else
        {
            services.Configure<CorsSettings>(CorsSettings.ConfigurationSection);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    AllowSpecificCorsPolicy,
                    policyBuilder =>
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        var corsOptions     = serviceProvider.GetRequiredService<CorsSettings>();

                        policyBuilder
                            .WithOrigins([.. corsOptions.Origins])
                            .WithHeaders([.. corsOptions.Headers])
                            .WithMethods([.. corsOptions.Methods]);

                        if (corsOptions.MaxAge is not null)
                        {
                            policyBuilder.SetPreflightMaxAge(
                                TimeSpan.FromSeconds(corsOptions.MaxAge.Value)
                            );
                        }

                        if (corsOptions.AllowCredentials)
                        {
                            policyBuilder.AllowCredentials();
                        }
                    }
                );
            });
        }
    }
    
    public static void UseDefaultCors(this WebApplication app)
    {
        app.UseCors(app.Environment.IsDevelopment() ? AllowAllCorsPolicy : AllowSpecificCorsPolicy);
    }
}