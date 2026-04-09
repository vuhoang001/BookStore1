using BuildingBlocks.Chassis.Security.Settings;
using BuildingBlocks.Chassis.Utils;
using BuildingBlocks.Constants.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Chassis.Security.Extensions;

public static class AuthenticationExtensions
{
    public static IHostApplicationBuilder AddDefaultAuthentication(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        var identityOptions = builder.Configuration
                .GetSection(IdentityOptions.ConfigurationSection)
                .Get<IdentityOptions>()
            ?? throw new InvalidOperationException("Missing Identity configuration.");

        ArgumentException.ThrowIfNullOrWhiteSpace(identityOptions.Realm);
        ArgumentException.ThrowIfNullOrWhiteSpace(identityOptions.ClientId);

        services.Configure<IdentityOptions>(
            builder.Configuration.GetSection(IdentityOptions.ConfigurationSection)
        );
        services.AddSingleton(identityOptions);


        var keycloakBaseUrl = HttpUtilities
            .AsUrlBuilder()
            .WithScheme(builder.Environment.IsDevelopment() ? Http.Schemes.Http : Http.Schemes.Https)
            // .WithHost(Components.KeyCloak)
            .WithHost(Network.Localhost)
            .WithPort(8080)
            .Build();

        var authority = HttpUtilities
            .AsUrlBuilder()
            .WithBase(keycloakBaseUrl)
            .WithPath($"realms/{identityOptions.Realm}")
            .Build();

        services.AddHttpClient(
            Components.KeyCloak,
            client => client.BaseAddress = new Uri(keycloakBaseUrl)
        );

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority            = "http://keycloak:8080/realms/BookStoreRealm";
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer    = "http://keycloak:8080/realms/BookStoreRealm",

                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });


        return builder;
    }
}