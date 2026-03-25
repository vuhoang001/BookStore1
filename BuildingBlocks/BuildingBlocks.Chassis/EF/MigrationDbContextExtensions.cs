using Microsoft.Data.SqlClient;
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
    private const int MaxMigrationAttempts = 5;
    private static readonly TimeSpan InitialRetryDelay = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Đăng ký một hosted service chạy EF Core migrations khi khởi động, không có seeding.
    /// </summary>
    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        return services.AddMigrationInternal<TContext>((_, _, _) => Task.CompletedTask);
    }

    /// <summary>
    /// Đăng ký một hosted service chạy EF Core migrations khi khởi động, có seeding.
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
        return services.AddMigrationInternal(seeder);
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
        : IHostedService
        where TContext : DbContext
    {
        public Task StartAsync(CancellationToken cancellationToken)
            => serviceProvider.MigrateDbContextAsync(seeder, cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }

    private static async Task MigrateDbContextAsync<TContext>(
        this IServiceProvider services,
        Func<TContext, IServiceProvider, CancellationToken, Task> seeder,
        CancellationToken cancellationToken)
        where TContext : DbContext
    {
        using var loggerScope = services.CreateScope();
        var logger = loggerScope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

        logger.LogInformation(
            "Migrating database associated with context {DbContextName}",
            typeof(TContext).Name
        );

        for (var attempt = 1; attempt <= MaxMigrationAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await MigrateAndSeedOnceAsync(services, seeder, cancellationToken);

                logger.LogInformation(
                    "Database migration completed for context {DbContextName}",
                    typeof(TContext).Name
                );

                return;
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning(
                    "Database migration was cancelled for context {DbContextName}",
                    typeof(TContext).Name
                );
                throw;
            }
            catch (Exception ex) when (IsRetriableLockException(ex) && attempt < MaxMigrationAttempts)
            {
                var delay = GetBackoffDelay(attempt);

                logger.LogWarning(
                    ex,
                    "Database migration lock contention for context {DbContextName}. Retrying in {DelaySeconds}s (attempt {Attempt}/{MaxAttempts})",
                    typeof(TContext).Name,
                    delay.TotalSeconds,
                    attempt,
                    MaxMigrationAttempts
                );

                await Task.Delay(delay, cancellationToken);
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
    }

    private static async Task MigrateAndSeedOnceAsync<TContext>(
        IServiceProvider services,
        Func<TContext, IServiceProvider, CancellationToken, Task> seeder,
        CancellationToken cancellationToken)
        where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeServices = scope.ServiceProvider;
        var context = scopeServices.GetRequiredService<TContext>();
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(
            ct => MigrateAndSeedAsync(context, scopeServices, seeder, ct),
            cancellationToken
        );
    }

    private static async Task MigrateAndSeedAsync<TContext>(
        TContext context,
        IServiceProvider services,
        Func<TContext, IServiceProvider, CancellationToken, Task> seeder,
        CancellationToken cancellationToken)
        where TContext : DbContext
    {
        await context.Database.MigrateAsync(cancellationToken);

        await seeder(context, services, cancellationToken);
    }

    private static TimeSpan GetBackoffDelay(int attempt)
    {
        var jitterMilliseconds = Random.Shared.Next(100, 700);
        var exponentialFactor = Math.Pow(2, attempt - 1);
        var baseDelay = TimeSpan.FromMilliseconds(InitialRetryDelay.TotalMilliseconds * exponentialFactor);

        return baseDelay + TimeSpan.FromMilliseconds(jitterMilliseconds);
    }

    private static bool IsRetriableLockException(Exception exception)
    {
        if (exception is SqlException sqlException)
        {
            return sqlException.Number is -2 or 1205 or 1222;
        }

        if (exception.InnerException is not null)
        {
            return IsRetriableLockException(exception.InnerException);
        }

        return exception.Message.Contains("exclusive lock for migration", StringComparison.OrdinalIgnoreCase)
               || exception.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase)
               || exception.Message.Contains("deadlock", StringComparison.OrdinalIgnoreCase);
    }
}