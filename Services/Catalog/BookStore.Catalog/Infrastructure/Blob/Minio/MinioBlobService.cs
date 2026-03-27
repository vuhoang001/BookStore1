using System.Text.RegularExpressions;
using Amazon.S3;
using Amazon.S3.Model;

namespace BookStore.Catalog.Infrastructure.Blob.Minio;

public partial class MinioBlobService(IAmazonS3 s3, MinioSettings minioSettings) : IBlobService
{
    public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var fileName = GenerateUniqueFileName(file.FileName);

        await using var stream = file.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName  = minioSettings.Bucket,
            Key         = fileName,
            InputStream = stream,
            ContentType = file.ContentType
        };

        await s3.PutObjectAsync(request, cancellationToken);

        return BuildUrn(fileName);
    }

    public async Task DeleteFileAsync(string urn, CancellationToken cancellationToken = default)
    {
        var fileName = ParseFileName(urn);

        var request = new DeleteObjectRequest
        {
            BucketName = minioSettings.Bucket,
            Key        = fileName
        };

        await s3.DeleteObjectAsync(request, cancellationToken);
    }

    public string GetFileSasUrl(string urn)
    {
        var fileName = ParseFileName(urn);

        var request = new GetPreSignedUrlRequest
        {
            BucketName = minioSettings.Bucket,
            Key        = fileName,
            Expires    = DateTime.UtcNow.AddHours(minioSettings.SasExpiryHours)
        };

        using var presignClient = CreatePresignClient();
        return presignClient.GetPreSignedURL(request);
    }


    private string BuildUrn(string fileName) =>
        $"urn:minio:{minioSettings.Bucket}:{fileName}";

    /// <summary>
    /// Parses and validates a URN of the form <c>urn:minio:{bucket}:{fileName}</c>
    /// and returns the raw file name.
    /// </summary>
    private string ParseFileName(string urn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);

        var parts = urn.Split(':', 4);

        if (parts.Length != 4)
            throw new ArgumentException(
                "URN must follow the format: urn:minio:{bucket}:{fileName}",
                nameof(urn));

        var (scheme, provider, bucket, fileName) = (parts[0], parts[1], parts[2], parts[3]);

        if (!scheme.Equals("urn", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("URN must start with 'urn'.", nameof(urn));

        if (!provider.Equals("minio", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("URN provider must be 'minio'.", nameof(urn));

        if (!bucket.Equals(minioSettings.Bucket, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"URN bucket '{bucket}' does not match configured bucket '{minioSettings.Bucket}'.",
                nameof(urn));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("URN must contain a non-empty file name.", nameof(urn));

        return fileName;
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        var name     = Path.GetFileNameWithoutExtension(originalFileName);
        var ext      = Path.GetExtension(originalFileName);
        var safeName = InvalidFileNameChars().Replace(name, string.Empty);

        return $"{Guid.NewGuid()}-{safeName}{ext}";
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await s3.PutBucketAsync(
                new PutBucketRequest { BucketName = minioSettings.Bucket },
                cancellationToken);
        }
        catch (AmazonS3Exception ex)
            when (ex.ErrorCode is "BucketAlreadyOwnedByYou" or "BucketAlreadyExists")
        {
            // Bucket already exists — safe to continue.
        }
    }

    private IAmazonS3 CreatePresignClient()
    {
        var serviceUrl = string.IsNullOrWhiteSpace(minioSettings.PublicEndpoint)
            ? minioSettings.Endpoint
            : minioSettings.PublicEndpoint;

        var endpointUri = new Uri(serviceUrl, UriKind.Absolute);

        return new AmazonS3Client(
            minioSettings.AccessKey,
            minioSettings.SecretKey,
            new AmazonS3Config
            {
                ServiceURL     = serviceUrl,
                UseHttp        = endpointUri.Scheme == Uri.UriSchemeHttp,
                ForcePathStyle = true,
                Timeout        = TimeSpan.FromSeconds(30),
                MaxErrorRetry  = 3
            }
        );
    }

    [GeneratedRegex(@"[^a-zA-Z0-9.\-_]")]
    private static partial Regex InvalidFileNameChars();
}