using BuildingBlocks.Chassis.EF;
using BuildingBlocks.Chassis.Repository;
using BuildingBlocks.Constants.Core;

namespace BookStore.Catalog.Infrastructure;

internal static class Extensions
{
    public static void AddPersistenceServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;


        builder.AddSqlServerEfDbContext<CatalogDbContext>(Components.Database.Catalog, _ =>
        {
            services.AddMigration<CatalogDbContext>();
            services.AddRepositories(typeof(ICatalogApiMarker));
        });
    }
}