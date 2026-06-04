using MinioStorage.BuildingBlock.Models;
using MinioStorage.BuildingBlock.Options;

namespace MinioStorage.BuildingBlock.Validations;

public static class FileValidator
{
    public static FileValidationResult Validate(
        UploadRequest request,
        MinioStorageOptions options)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(options);

        var errors = new List<string>();

        if (request.Content is null)
            errors.Add("File content is required.");

        if (string.IsNullOrWhiteSpace(request.ObjectName))
            errors.Add("Object name is required.");

        if (request.Size <= 0)
            errors.Add("File size must be greater than zero.");

        if (options.MaxUploadBytes > 0 && request.Size > options.MaxUploadBytes)
            errors.Add($"File size exceeds the maximum allowed size of {options.MaxUploadBytes} bytes.");

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            errors.Add("Content type is required.");
        }
        else if (options.AllowedContentTypes.Length > 0 &&
                 !options.AllowedContentTypes.Contains(request.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            errors.Add($"Content type '{request.ContentType}' is not allowed.");
        }

        return errors.Count == 0
            ? FileValidationResult.Success()
            : FileValidationResult.Failure(errors);
    }
}