using FluentAssertions;
using MinioStorage.BuildingBlock.Models;
using MinioStorage.BuildingBlock.Options;
using MinioStorage.BuildingBlock.Validations;

namespace MinioStorage.BuildingBlock.UnitTests.Validators;

public sealed class FileValidatorTests
{
    
    private static MinioStorageOptions CreateOptions() => new()
    {
        Enabled = true,
        MaxUploadBytes = 1024,
        AllowedContentTypes = new[] { "image/jpeg", "image/png" }
    };
    
    [Fact]
    public void Validate_ValidFile_ReturnsSuccess()
    {
        var options = CreateOptions();
        var request = new UploadRequest
        {
            Content = new MemoryStream(new byte[100]),
            ObjectName = "file.png",
            ContentType = "image/png",
            Size = 100
        };

        var result = FileValidator.Validate(request, options);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidSize_ReturnsError()
    {
        var options = CreateOptions();
        var request = new UploadRequest
        {
            Content = new MemoryStream(),
            ObjectName = "file.png",
            ContentType = "image/png",
            Size = 0
        };

        var result = FileValidator.Validate(request, options);

        result.IsValid.Should().BeFalse();
    }
    
}