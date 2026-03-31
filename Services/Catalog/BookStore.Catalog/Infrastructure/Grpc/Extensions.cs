namespace BookStore.Catalog.Infrastructure.Grpc;

internal static class Extensions
{
   public static void AddGrpcServices(this IHostApplicationBuilder builder)
   {
      var services = builder.Services;

      builder.Services.AddGrpc(options =>
      {
         options.EnableDetailedErrors = true;
      });
   }
}