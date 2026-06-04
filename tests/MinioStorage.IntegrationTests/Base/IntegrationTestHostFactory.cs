using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minio;
using Minio.DataModel.Args;
using MinioStorage.BuildingBlock.Extensions;
using MinioStorage.BuildingBlock.Options;

namespace MinioStorage.IntegrationTests.Base;

public sealed class IntegrationTestHostFactory : IAsyncLifetime
{
    private IHost? _host;

    public IServiceProvider Services =>
        _host?.Services ?? throw new InvalidOperationException("Host is not initialized.");

    public async Task InitializeAsync()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(configuration =>
            {
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["MinioStorage:Enabled"] = "true",
                    ["MinioStorage:Endpoint"] = "localhost:9100",
                    ["MinioStorage:AccessKey"] = "minioadmin",
                    ["MinioStorage:SecretKey"] = "minioadmin123",
                    ["MinioStorage:Bucket"] = "demo-users-tests",
                    ["MinioStorage:UseSsl"] = "false",
                    ["MinioStorage:PresignedUrlMinutes"] = "30",
                    ["MinioStorage:MaxUploadBytes"] = "2097152",
                    ["MinioStorage:AllowedContentTypes:0"] = "image/jpeg",
                    ["MinioStorage:AllowedContentTypes:1"] = "image/png",
                    ["MinioStorage:AllowedContentTypes:2"] = "image/webp",
                    ["MinioStorage:CreateBucketIfNotExists"] = "true"
                });
            })
            .ConfigureServices((context, services) => { services.AddMinioStorage(context.Configuration); })
            .Build();

        await _host.StartAsync();

        await EnsureBucketExistsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
    }

    private async Task EnsureBucketExistsAsync()
    {
        var options = Services
            .GetRequiredService<Microsoft.Extensions.Options.IOptions<MinioStorageOptions>>()
            .Value;

        var minioClient = Services.GetRequiredService<IMinioClient>();

        var exists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs()
                .WithBucket(options.Bucket));

        if (!exists)
        {
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs()
                    .WithBucket(options.Bucket));
        }
    }
}