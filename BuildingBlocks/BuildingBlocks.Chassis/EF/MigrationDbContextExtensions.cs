using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Chassis.EF;

/// <summary>
/// Khai báo cho việc thêm dữ liệu sau khi chạy migration, có thể dùng với AddMigration() để tự động chạy seeding sau khi migration hoàn tất.
/// </summary>
public interface IDbSeeder<in TContext>
    where TContext : DbContext
{
    Task SeedAsync(TContext context, IServiceProvider services, CancellationToken cancellationToken = default);
}

public static class BusMigrationDbContextExtensions
{
    /// <summary>
    /// Đăng ký một hosted service chạy EF Cỏe migrations khi khởi động, không có seeding.
    /// </summary>
    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        return services.AddMigrationInternal<TContext>((_, _, _) => Task.CompletedTask);
    }

    /// <summary>
    /// Đăng ký một hosted service chạy EF Cỏe migrations khi khởi động, có seeding.
    /// </summary>
    public static IServiceCollection AddMigration<TContext, TDbSeeder>(this IServiceCollection services)
        where TContext : DbContext
        where TDbSeeder : class, IDbSeeder<TContext>
    {
        services.AddTransient<IDbSeeder<TContext>, TDbSeeder>();

        return services.AddMigrationInternal<TContext>((ctx, sp, ct) =>
                                                           sp.GetRequiredService<IDbSeeder<TContext>>()
                                                               .SeedAsync(ctx, sp, ct)
        );
    }

    public static IServiceCollection AddMigration<TContext>(
        this IServiceCollection services,
        Func<TContext, IServiceProvider, CancellationToken, Task> seeder)
        where TContext : DbContext
    {
        return services.AddMigrationInternal<TContext>(seeder);
    }

    // -------------------------------------------------------------------------
    // Internals
    // -------------------------------------------------------------------------

    private static IServiceCollection AddMigrationInternal<TContext>(
        this IServiceCollection services,
        Func<TContext, IServiceProvider, CancellationToken, Task> seeder)
        where TContext : DbContext
    {
        return services.AddHostedService(sp => new MigrationHostedService<TContext>(sp, seeder));
    }

    private sealed class MigrationHostedService<TContext>(
        IServiceProvider serviceProvider,
        Func<TContext, IServiceProvider, CancellationToken, Task> seeder)
        : BackgroundService
        where TContext : DbContext
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => serviceProvider.MigrateDbContextAsync(seeder, stoppingToken);
    }

    private static async Task MigrateDbContextAsync<TContext>(
        this IServiceProvider services,
        Func<TContext, IServiceProvider, CancellationToken, Task> seeder,
        CancellationToken cancellationToken)
        where TContext : DbContext
    {
        using var scope         = services.CreateScope();
        var       scopeServices = scope.ServiceProvider;
        var       logger        = scopeServices.GetRequiredService<ILogger<TContext>>();
        var       context       = scopeServices.GetRequiredService<TContext>();

        logger.LogInformation(
            "Migrating database associated with context {DbContextName}",
            typeof(TContext).Name
        );

        try
        {
            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(
                ct => MigrateAndSeedAsync(context, scopeServices, seeder, ct),
                cancellationToken
            );

            logger.LogInformation(
                "Database migration completed for context {DbContextName}",
                typeof(TContext).Name
            );
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Database migration was cancelled for context {DbContextName}",
                typeof(TContext).Name
            );
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while migrating the database used on context {DbContextName}",
                typeof(TContext).Name
            );

            throw new InvalidOperationException(
                $"Database migration failed for {typeof(TContext).Name}. See inner exception for details.",
                ex
            );
        }
    }

    private static async Task MigrateAndSeedAsync<TContext>(
        TContext context,
        IServiceProvider services,
        Func<TContext, IServiceProvider, CancellationToken, Task> seeder,
        CancellationToken cancellationToken)
        where TContext : DbContext
    {
        // Only apply migrations without creating DB first; DB creation is handled by migrations
        await context.Database.MigrateAsync(cancellationToken);
        await seeder(context, services, cancellationToken);
    }
}