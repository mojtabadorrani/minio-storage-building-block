using System.Globalization;
using System.Text.RegularExpressions;

namespace MinioStorage.BuildingBlock.Services;

/// <summary>
/// Generates safe and predictable object names for MinIO storage.
/// </summary>
public static partial class ObjectNameGenerator
{
    /// <summary>
    /// Generates a unique object name using the specified category.
    /// </summary>
    /// <param name="category">
    /// Logical object category such as users, products or invoices.
    /// </param>
    /// <param name="originalFileName">
    /// Original uploaded file name.
    /// </param>
    /// <returns>
    /// A normalized object path.
    /// </returns>
    public static string Generate(
        string category,
        string originalFileName)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category is required.", nameof(category));

        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new ArgumentException("Original file name is required.", nameof(originalFileName));

        var safeCategory = NormalizeSegment(category);
        var extension = GetSafeExtension(originalFileName);
        var datePath = DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
        var uniqueName = $"{Guid.NewGuid():N}{extension}";

        return $"{safeCategory}/{datePath}/{uniqueName}";
    }

    /// <summary>
    /// Generates a unique object name using the specified category and owner identifier.
    /// </summary>
    /// <param name="category">
    /// Logical object category.
    /// </param>
    /// <param name="ownerId">
    /// Object owner identifier.
    /// </param>
    /// <param name="originalFileName">
    /// Original uploaded file name.
    /// </param>
    /// <returns>
    /// A normalized object path.
    /// </returns>
    public static string Generate(
        string category,
        string ownerId,
        string originalFileName)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category is required.", nameof(category));

        if (string.IsNullOrWhiteSpace(ownerId))
            throw new ArgumentException("OwnerId is required.", nameof(ownerId));

        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new ArgumentException("Original file name is required.", nameof(originalFileName));

        var safeCategory = NormalizeSegment(category);
        var safeOwnerId = NormalizeSegment(ownerId);

        var extension = GetSafeExtension(originalFileName);
        var datePath = DateTime.UtcNow.ToString(
            "yyyy/MM/dd",
            CultureInfo.InvariantCulture);

        var uniqueName = $"{Guid.NewGuid():N}{extension}";

        return $"{safeCategory}/{safeOwnerId}/{datePath}/{uniqueName}";
    }

    private static string NormalizeSegment(string value)
    {
        var normalized = value.Trim().ToLowerInvariant();
        normalized = UnsafeCharsRegex().Replace(normalized, "-");
        normalized = DuplicateDashRegex().Replace(normalized, "-");

        return normalized.Trim('-');
    }

    private static string GetSafeExtension(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);

        if (string.IsNullOrWhiteSpace(extension))
            return string.Empty;

        extension = extension.Trim().ToLowerInvariant();

        return SafeExtensionRegex().IsMatch(extension)
            ? extension
            : string.Empty;
    }

    [GeneratedRegex(@"[^a-z0-9\-]")]
    private static partial Regex UnsafeCharsRegex();

    [GeneratedRegex(@"-+")]
    private static partial Regex DuplicateDashRegex();

    [GeneratedRegex(@"^\.[a-z0-9]{1,10}$")]
    private static partial Regex SafeExtensionRegex();
}