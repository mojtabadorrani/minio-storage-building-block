namespace MinioStorage.BuildingBlock.Validations;

public sealed record FileValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public IReadOnlyList<string> Errors { get; init; } = [];
    public static FileValidationResult Success() => new();

    public static FileValidationResult Failure(params string[] errors)
    {
        return new FileValidationResult
        {
            Errors = errors
                .Where(error => !string.IsNullOrWhiteSpace(error))
                .ToArray()
        };
    }

    public static FileValidationResult Failure(IEnumerable<string> errors)
    {
        return new FileValidationResult
        {
            Errors = errors
                .Where(error => !string.IsNullOrWhiteSpace(error))
                .ToArray()
        };
    }
}