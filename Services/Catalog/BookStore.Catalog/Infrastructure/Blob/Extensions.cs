using Amazon.S3;
using BookStore.Catalog.Infrastructure.Blob.Minio;

namespace BookStore.Catalog.Infrastructure.Blob;

internal static class Extensions
{
    public static void AddMinioBlobStorage(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        var minioSettings = builder.Configuration
            .GetSection("Minio")
            .Get<MinioSettings>();

        if (minioSettings is null                              ||
            string.IsNullOrWhiteSpace(minioSettings.Endpoint)  ||
            string.IsNullOrWhiteSpace(minioSettings.AccessKey) ||
            string.IsNullOrWhiteSpace(minioSettings.SecretKey) ||
            string.IsNullOrWhiteSpace(minioSettings.Bucket))
        {
            throw new InvalidOperationException(
                "Minio configuration is missing or incomplete. Please ensure all required settings are provided in the configuration.");
        }

        // inject config
        services.AddSingleton(minioSettings);

        // inject S3 client
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var settings = sp.GetRequiredService<MinioSettings>();
            var endpointUri = new Uri(settings.Endpoint, UriKind.Absolute);

            return new AmazonS3Client(
                settings.AccessKey,
                settings.SecretKey,
                new AmazonS3Config
                {
                    ServiceURL     = settings.Endpoint,
                    UseHttp        = endpointUri.Scheme == Uri.UriSchemeHttp,
                    ForcePathStyle = true,
                    Timeout        = TimeSpan.FromSeconds(30),
                    MaxErrorRetry  = 3
                }
            );
        });

        // inject blob service
        services.AddScoped<IBlobService, MinioBlobService>();
    }
}