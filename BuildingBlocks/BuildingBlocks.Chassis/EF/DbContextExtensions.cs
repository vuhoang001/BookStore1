using BuildingBlocks.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Chassis.EF;

public static class DbContextExtensions
{
    public static void AddSqlServerEfDbContext<TDbContext>(this IHostApplicationBuilder builder, string name,
        Action<IHostApplicationBuilder>? action = null, bool excludeDefaultInterceptors = false)
        where TDbContext : DbContext
    {
        var services = builder.Services;

        if (!excludeDefaultInterceptors)
        {
            services.AddScoped<ISaveChangesInterceptor, EventDispatchInterceptor>();
            services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();
        }

        // Run extra registrations at composition time, not inside DbContext options creation.
        action?.Invoke(builder);

        services.AddDbContext<TDbContext>((sp, options) =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString(name), sql =>
                {
                    sql.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure();
                })
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));


            var interceptors = sp.GetServices<ISaveChangesInterceptor>()
                .Cast<IInterceptor>()
                .ToArray();


            if (interceptors.Length > 0)
            {
                options.AddInterceptors(interceptors);
            }
        });
    }
}