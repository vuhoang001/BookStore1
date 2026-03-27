namespace BookStore.Catalog.Infrastructure.Blob.Minio;

public class MinioSettings
{
    public string Endpoint { get; set; }
    public string? PublicEndpoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Bucket { get; set; }
    public int SasExpiryHours { get; set; }
}