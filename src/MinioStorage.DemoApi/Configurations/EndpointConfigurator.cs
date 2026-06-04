using MinioStorage.DemoApi.Endpoints;
 

namespace MinioStorage.DemoApi.Configurations;

public static class EndpointConfigurator
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        StorageEndpoints.Map(app);

        app.MapGet("/",
            () => $"Minio Storage Demo API - {DateTime.UtcNow:u}");

        app.MapGet("/env",
            () => app.Environment.EnvironmentName);

        app.MapGet("/health",
            () => Results.Ok("Healthy"));
    }
}