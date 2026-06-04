using Microsoft.Extensions.DependencyInjection;
using MinioStorage.BuildingBlock.Abstractions;

namespace MinioStorage.IntegrationTests.Base;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestHostFactory>
{
    protected readonly IStorageService StorageService;

    protected BaseIntegrationTest(IntegrationTestHostFactory factory)
    {
        var scope = factory.Services.CreateScope();

        StorageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
    }
}