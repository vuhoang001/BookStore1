namespace BookStore.Catalog.Infrastructure.Blob.Minio;

public class MinioSettings
{
    public string Endpoint { get; init; } = string.Empty;
    public string? PublicEndpoint { get; init; }
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string Bucket { get; init; } = string.Empty;
    public int SasExpiryHours { get; init; }
}