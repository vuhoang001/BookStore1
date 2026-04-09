using BuildingBlocks.Chassis.Utils;
using BuildingBlocks.Constants.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Chassis.Security.Keycloak;

public static class KeycloakClaimsTransformationExtensions
{
    /// <summary>
    ///     Adds an <see cref="IClaimsTransformation" /> that transforms Keycloak resource access roles claims into regular
    ///     role claims.
    /// </summary>
    public static IHostApplicationBuilder WithKeycloakClaimsTransformation(
        this IHostApplicationBuilder builder
    )
    {
        builder.Services.AddTransient<IClaimsTransformation, KeycloakRolesClaimsTransformation>();
        return builder;
    }

    public static IHostApplicationBuilder WithKeycloakTokenIntrospection(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<KeycloakTokenIntrospectionMiddleware>();
        return builder;
    }
}