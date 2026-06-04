namespace MinioStorage.BuildingBlock.Options;

/// <summary>
/// Configuration options for MinIO storage.
/// </summary>
public sealed class MinioStorageOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "MinioStorage";

    /// <summary>
    /// Enables or disables the storage module.
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// MinIO server endpoint.
    /// </summary>
    public string? Endpoint { get; init; }

    /// <summary>
    /// Access key used to authenticate with MinIO.
    /// </summary>
    public string? AccessKey { get; init; }

    /// <summary>
    /// Secret key used to authenticate with MinIO.
    /// </summary>
    public string? SecretKey { get; init; }

    /// <summary>
    /// Default bucket name.
    /// </summary>
    public string? Bucket { get; init; }

    /// <summary>
    /// Indicates whether SSL should be used.
    /// </summary>
    public bool UseSsl { get; init; }

    /// <summary>
    /// Default presigned URL expiration time in minutes.
    /// </summary>
    public int PresignedUrlMinutes { get; init; }

    /// <summary>
    /// Maximum allowed upload size in bytes.
    /// </summary>
    public long MaxUploadBytes { get; init; }

    /// <summary>
    /// Allowed content types.
    /// </summary>
    public string[] AllowedContentTypes { get; init; } = [];

    /// <summary>
    /// Optional public base URL.
    /// </summary>
    public string? PublicBaseUrl { get; init; }

    /// <summary>
    /// Automatically creates the bucket if it does not exist.
    /// </summary>
    public bool CreateBucketIfNotExists { get; init; }
}