using MinioStorage.BuildingBlock.Abstractions;
using MinioStorage.BuildingBlock.Models;
using MinioStorage.BuildingBlock.Services;

namespace MinioStorage.DemoApi.Endpoints;

public static class StorageEndpoints
{
    public static void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/storage")
            .WithTags("Storage");

        MapStorageRoutes(group);
    }

    private static void MapStorageRoutes(RouteGroupBuilder map)
    {
        map.MapPost("/upload",
            async (
                HttpRequest request,
                IStorageService storage,
                CancellationToken cancellationToken) =>
            {
                if (!request.HasFormContentType)
                    return Results.BadRequest("Request must be multipart/form-data.");

                var form = await request.ReadFormAsync(cancellationToken);
                var file = form.Files.FirstOrDefault();

                if (file is null || file.Length == 0)
                    return Results.BadRequest("File is required.");

                await using var stream = file.OpenReadStream();

                var objectName = ObjectNameGenerator.Generate(
                    category: "demo",
                    originalFileName: file.FileName);

                var result = await storage.UploadAsync(
                    new UploadRequest
                    {
                        Content = stream,
                        ObjectName = objectName,
                        ContentType = file.ContentType,
                        Size = file.Length
                    },
                    cancellationToken);

                return Results.Ok(result);
            })
            .Accepts<IFormFile>("multipart/form-data")
            .DisableAntiforgery();

        map.MapGet("/presigned-url",
            async (
                string objectName,
                IStorageService storage,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(objectName))
                    return Results.BadRequest("ObjectName is required.");

                var result = await storage.GetPresignedUrlAsync(
                    objectName,
                    cancellationToken: cancellationToken);

                return Results.Ok(result);
            });

        map.MapGet("/exists",
            async (
                string objectName,
                IStorageService storage,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(objectName))
                    return Results.BadRequest("ObjectName is required.");

                var exists = await storage.ExistsAsync(
                    objectName,
                    cancellationToken);

                return Results.Ok(exists);
            });
    }
}