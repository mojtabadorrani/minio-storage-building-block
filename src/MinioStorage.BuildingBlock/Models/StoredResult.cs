namespace MinioStorage.BuildingBlock.Models;


/// <summary>
/// Represents a successful storage operation result.
/// </summary>
public sealed class StoredResult
{
    /// <summary>
    /// Gets or sets the bucket name.
    /// </summary>
    public required string BucketName { get; init; }

    /// <summary>
    /// Gets or sets the stored object name.
    /// </summary>
    public required string ObjectName { get; init; }

    /// <summary>
    /// Gets or sets the content type.
    /// </summary>
    public required string ContentType { get; init; }

    /// <summary>
    /// Gets or sets the stored object size in bytes.
    /// </summary>
    public required long Size { get; init; }

    /// <summary>
    /// Gets or sets the storage timestamp in UTC.
    /// </summary>
    public required DateTimeOffset StoredAt { get; init; }
}