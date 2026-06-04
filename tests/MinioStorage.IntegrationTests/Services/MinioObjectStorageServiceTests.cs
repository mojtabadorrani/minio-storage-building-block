using FluentAssertions;
using MinioStorage.BuildingBlock.Models;
using MinioStorage.BuildingBlock.Services;
using MinioStorage.IntegrationTests.Base;

namespace MinioStorage.IntegrationTests.Services;

public sealed class MinioObjectStorageServiceTests(
    IntegrationTestHostFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task UploadAsync_Should_Upload_File()
    {
        var objectName = ObjectNameGenerator.Generate(
            category: "integration-tests",
            originalFileName: "sample.png");

        await using var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 });

        var request = new UploadRequest
        {
            Content = stream,
            ObjectName = objectName,
            ContentType = "image/png",
            Size = stream.Length
        };

        var result = await StorageService.UploadAsync(request);

        result.ObjectName.Should().Be(objectName);
        result.ContentType.Should().Be("image/png");
        result.Size.Should().Be(4);
    }
    
    [Fact]
    public async Task ExistsAsync_Should_Return_True_When_Object_Exists()
    {
        var objectName = ObjectNameGenerator.Generate(
            category: "integration-tests",
            originalFileName: "exists.png");

        await using var stream = new MemoryStream(new byte[] { 10, 20, 30 });

        await StorageService.UploadAsync(new UploadRequest
        {
            Content = stream,
            ObjectName = objectName,
            ContentType = "image/png",
            Size = stream.Length
        });

        var exists = await StorageService.ExistsAsync(objectName);

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task GetPresignedUrlAsync_Should_Return_Url_When_Object_Exists()
    {
        var objectName = ObjectNameGenerator.Generate(
            category: "integration-tests",
            originalFileName: "presigned.png");

        await using var stream = new MemoryStream(new byte[] { 5, 6, 7 });

        await StorageService.UploadAsync(new UploadRequest
        {
            Content = stream,
            ObjectName = objectName,
            ContentType = "image/png",
            Size = stream.Length
        });

        var result = await StorageService.GetPresignedUrlAsync(objectName);

        result.ObjectName.Should().Be(objectName);
        result.Url.Should().NotBeNullOrWhiteSpace();
        result.Url.Should().Contain("localhost:9100");
        result.Url.Should().Contain(objectName);
        result.ExpiresAt.Should().BeAfter(DateTimeOffset.UtcNow);
    }
}