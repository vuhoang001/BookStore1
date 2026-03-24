using BuildingBlocks.Constants.Core;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Chassis.EndPoints;

public static class ApiVersioningExtensions
{
    public static void AddVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options => { options.DefaultApiVersion = Versions.V1; })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat           = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}