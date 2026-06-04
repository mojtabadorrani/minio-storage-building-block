using MinioStorage.BuildingBlock.Options;


namespace MinioStorage.BuildingBlock.Initialization;

public class MinioBucketInitializer(
    IMinioClient minioClient,
    IOptions<MinioStorageOptions> options,
    ILogger<MinioBucketInitializer> logger)
    : IHostedService
{
    private readonly MinioStorageOptions _options = options.Value;
    private readonly IMinioClient _minio = minioClient;
    private readonly ILogger<MinioBucketInitializer> _logger = logger;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("MinIO Storage disabled.");
            return;
        }

        try
        {
            var exists = await _minio.BucketExistsAsync(
                new BucketExistsArgs()
                    .WithBucket(_options.Bucket),
                cancellationToken);

            if (!exists && _options.CreateBucketIfNotExists)
            {
                _logger.LogInformation("Creating MinIO bucket {Bucket}", _options.Bucket);

                await _minio.MakeBucketAsync(
                    new MakeBucketArgs()
                        .WithBucket(_options.Bucket),
                    cancellationToken);
            }
            else
            {
                _logger.LogInformation("Bucket {Bucket} already exists", _options.Bucket);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize MinIO bucket");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}