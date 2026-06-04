namespace MinioStorage.BuildingBlock.Models;

/// <summary>
/// Represents a generated presigned URL.
/// </summary>
public sealed class PresignedResult
{
    /// <summary>
    /// Gets or sets the object name.
    /// </summary>
    public required string ObjectName { get; init; }

    /// <summary>
    /// Gets or sets the generated URL.
    /// </summary>
    public required string Url { get; init; }

    /// <summary>
    /// Gets or sets the expiration timestamp in UTC.
    /// </summary>
    public required DateTimeOffset ExpiresAt { get; init; }
}