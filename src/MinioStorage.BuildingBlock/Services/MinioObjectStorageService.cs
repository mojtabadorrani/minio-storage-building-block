using Minio.Exceptions;
using MinioStorage.BuildingBlock.Abstractions;
using MinioStorage.BuildingBlock.Models;
using MinioStorage.BuildingBlock.Options;
using MinioStorage.BuildingBlock.Validations;

namespace MinioStorage.BuildingBlock.Services;

public sealed class MinioObjectStorageService(
    IMinioClient minioClient,
    IOptions<MinioStorageOptions> options,
    ILogger<MinioObjectStorageService> logger)
    : IStorageService
{
    private readonly MinioStorageOptions _options = options.Value;

    public async Task<StoredResult> UploadAsync(
        UploadRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = FileValidator.Validate(request, _options);

        if (!validation.IsValid)
            throw new InvalidOperationException(string.Join("; ", validation.Errors));

        if (request.Content.CanSeek)
            request.Content.Position = 0;

        await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_options.Bucket)
                .WithObject(request.ObjectName)
                .WithStreamData(request.Content)
                .WithObjectSize(request.Size)
                .WithContentType(request.ContentType),
            cancellationToken);

        logger.LogInformation(
            "Object stored successfully. Bucket: {Bucket}, ObjectName: {ObjectName}, Size: {Size}",
            _options.Bucket,
            request.ObjectName,
            request.Size);

        return new StoredResult
        {
            BucketName = _options.Bucket!,
            ObjectName = request.ObjectName,
            ContentType = request.ContentType,
            Size = request.Size,
            StoredAt = DateTimeOffset.UtcNow
        };
    }

    public async Task<PresignedResult> GetPresignedUrlAsync(
        string objectName,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("Object name is required.", nameof(objectName));

        var actualExpiry = expiry ?? TimeSpan.FromMinutes(_options.PresignedUrlMinutes);

        var url = await minioClient.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(_options.Bucket)
                .WithObject(objectName)
                .WithExpiry((int)actualExpiry.TotalSeconds));

        if (!string.IsNullOrWhiteSpace(_options.PublicBaseUrl))
        {
            var uri = new Uri(url);
            url = $"{_options.PublicBaseUrl.TrimEnd('/')}{uri.PathAndQuery}";
        }

        return new PresignedResult
        {
            ObjectName = objectName,
            Url = url,
            ExpiresAt = DateTimeOffset.UtcNow.Add(actualExpiry)
        };
    }

    public async Task<bool> ExistsAsync(
        string objectName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("Object name is required.", nameof(objectName));

        try
        {
            await minioClient.StatObjectAsync(
                new StatObjectArgs()
                    .WithBucket(_options.Bucket)
                    .WithObject(objectName),
                cancellationToken);

            return true;
        }
        catch (ObjectNotFoundException)
        {
            return false;
        }
    }
}