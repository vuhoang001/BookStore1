using BuildingBlocks.Chassis.EF;
using BuildingBlocks.Chassis.Repository;
using BuildingBlocks.Constants.Core;
using BookStore.Basket.Infrastructure.Grpc;

namespace BookStore.Basket.Infrastructure;

internal static class Extensions
{
    public static void AddPersistenceServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;


        builder.AddSqlServerEfDbContext<BasketDbContext>(Components.Database.Basket, _ =>
        {
            services.AddMigration<BasketDbContext>();
            services.AddRepositories(typeof(IBasketApiMarker));
        });
    }
}