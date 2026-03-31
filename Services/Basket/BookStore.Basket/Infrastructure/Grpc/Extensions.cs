using BookStore.Basket.Infrastructure.Grpc.Services.Book;
using Bookstore.Catalog.V1;
using BuildingBlocks.Chassis.Utils;
using BuildingBlocks.Constants.Components;
using BuildingBlocks.Constants.Core;

namespace BookStore.Basket.Infrastructure.Grpc;

public static class Extensions
{
    public static void AddGrpcServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        var catalogGrpcAddress =
            HttpUtilities
                .AsUrlBuilder()
                .WithScheme(Http.Schemes.Http)
                .WithHost(Modules.Catalog)
                .WithPort(8081)
                .Build();

        services
            .AddGrpcClient<BookGrpcService.BookGrpcServiceClient>(options =>
            {
                options.Address = new Uri(catalogGrpcAddress);
            });

        services.AddScoped<IBookService, BookService>();
    }
}