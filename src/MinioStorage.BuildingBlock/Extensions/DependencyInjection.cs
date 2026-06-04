using Microsoft.Extensions.Configuration;
using MinioStorage.BuildingBlock.Abstractions;
using MinioStorage.BuildingBlock.Initialization;
using MinioStorage.BuildingBlock.Options;
using MinioStorage.BuildingBlock.Services;

namespace MinioStorage.BuildingBlock.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMinioStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<MinioStorageOptions>()
            .BindConfiguration(MinioStorageOptions.SectionName)
            .Validate(options => !string.IsNullOrWhiteSpace(options.Endpoint))
            .Validate(options => !string.IsNullOrWhiteSpace(options.AccessKey))
            .Validate(options => !string.IsNullOrWhiteSpace(options.SecretKey))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Bucket))
            .ValidateOnStart();
        
        services.AddSingleton<IMinioClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MinioStorageOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(options.UseSsl)
                .Build();
        });

        services.AddSingleton<IStorageService, MinioObjectStorageService>();

        services.AddHostedService<MinioBucketInitializer>();

        return services;
    }
}