namespace MinioStorage.BuildingBlock.Models;

/// <summary>
/// Represents an upload request.
/// </summary>
public sealed  record UploadRequest
{
   /// <summary>
      /// Gets or sets the file content stream.
      /// </summary>
      public required Stream Content { get; init; }
  
      /// <summary>
      /// Gets or sets the destination object name.
      /// </summary>
      public required string ObjectName { get; init; }
  
      /// <summary>
      /// Gets or sets the MIME content type.
      /// </summary>
      public required string ContentType { get; init; }
  
      /// <summary>
      /// Gets or sets the content size in bytes.
      /// </summary>
      public required long Size { get; init; }
}