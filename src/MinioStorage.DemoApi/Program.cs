using MinioStorage.BuildingBlock.Extensions;
using MinioStorage.DemoApi.Configurations;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAntiforgery();

builder.Services.AddMinioStorage(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.ConfigureEndpoints();
app.UseAntiforgery();

app.MapGet("/routes", (IEnumerable<EndpointDataSource> sources) =>
    sources
        .SelectMany(x => x.Endpoints)
        .OfType<RouteEndpoint>()
        .Select(x => x.RoutePattern.RawText));

app.Run();