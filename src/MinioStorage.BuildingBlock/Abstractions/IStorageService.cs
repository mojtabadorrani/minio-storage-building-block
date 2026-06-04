using MinioStorage.BuildingBlock.Models;

namespace MinioStorage.BuildingBlock.Abstractions;

/// <summary>
/// Provides object storage operations backed by MinIO.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Uploads or replaces an object in the configured bucket.
    /// </summary>
    /// <param name="request">
    /// The upload request containing object metadata and content stream.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// Information about the stored object.
    /// </returns>
    Task<StoredResult> UploadAsync(
        UploadRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a temporary download URL for the specified object.
    /// </summary>
    /// <param name="objectName">
    /// The object key within the bucket.
    /// </param>
    /// <param name="expiry">
    /// Optional expiration period for the generated URL.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A presigned URL and its expiration information.
    /// </returns>
    Task<PresignedResult> GetPresignedUrlAsync(
        string objectName,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether the specified object exists.
    /// </summary>
    /// <param name="objectName">
    /// The object key within the bucket.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// True if the object exists; otherwise false.
    /// </returns>
    Task<bool> ExistsAsync(
        string objectName,
        CancellationToken cancellationToken = default);
}