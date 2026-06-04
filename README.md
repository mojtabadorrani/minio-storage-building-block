# Minio Storage Building Block

A reusable and production-ready MinIO storage building block for .NET applications.

## Projects

### MinioStorage.BuildingBlock

Core library containing:

- Storage abstractions
- MinIO implementation
- Validation
- Object name generation
- Dependency Injection extensions
- Bucket initialization

### MinioStorage.DemoApi

Minimal API demonstrating:

- Upload file
- Generate presigned URL
- Check object existence

### MinioStorage.BuildingBlock.UnitTests

Unit tests for:

- ObjectNameGenerator
- Validation logic

### MinioStorage.IntegrationTests

Integration tests using a real MinIO instance.

## Solution Structure

```text
minio-storage-building-block
├── src
│   ├── MinioStorage.BuildingBlock
│   └── MinioStorage.DemoApi
│
├── tests
│   ├── MinioStorage.BuildingBlock.UnitTests
│   └── MinioStorage.IntegrationTests
│
├── MinioStorage.BuildingBlock.slnx
├── docker-compose.yml
└── README.md
```

### Folder Responsibilities

* `src/MinioStorage.BuildingBlock`: reusable MinIO storage library
* `src/MinioStorage.DemoApi`: minimal API sample
* `tests/MinioStorage.BuildingBlock.UnitTests`: unit tests
* `tests/MinioStorage.IntegrationTests`: integration tests

## Quick Start

### Prerequisites

Before running the project, make sure the following tools are installed:

* .NET 10 SDK
* Docker Desktop

### Clone Repository

```bash
git clone <repository-url>

cd minio-storage-building-block
```

### Start MinIO

```bash
docker compose up -d
```

### Run Demo API

```bash
cd src/MinioStorage.DemoApi

dotnet run
```

### Open Scalar

After the application starts, open:

```text
http://localhost:6001/scalar
```

### Verify Health Endpoint

```text
GET http://localhost:6001/health
```

Expected response:

```text
Healthy
```

## Configuration

The building block uses the following configuration section:

```json
{
  "MinioStorage": {
    "Enabled": true,
    "Endpoint": "localhost:9100",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin123",
    "Bucket": "identity-users",
    "UseSsl": false,
    "PresignedUrlMinutes": 30,
    "MaxUploadBytes": 2097152,
    "AllowedContentTypes": [
      "image/jpeg",
      "image/png",
      "image/webp"
    ],
    "PublicBaseUrl": null,
    "CreateBucketIfNotExists": true
  }
}
```

### Configuration Options

| Property                | Description                            |
| ----------------------- | -------------------------------------- |
| Enabled                 | Enables or disables the storage module |
| Endpoint                | MinIO API endpoint                     |
| AccessKey               | MinIO access key                       |
| SecretKey               | MinIO secret key                       |
| Bucket                  | Default bucket name                    |
| UseSsl                  | Enables HTTPS communication            |
| PresignedUrlMinutes     | Default presigned URL lifetime         |
| MaxUploadBytes          | Maximum allowed upload size            |
| AllowedContentTypes     | Allowed MIME types                     |
| PublicBaseUrl           | Optional public URL override           |
| CreateBucketIfNotExists | Automatically creates the bucket       |


## Dependency Injection

Register the building block in your application:

```csharp
builder.Services.AddMinioStorage(
    builder.Configuration);
```

The building block automatically registers:

* MinIO client
* Storage service
* Configuration validation
* Bucket initialization hosted service

After registration, inject `IStorageService` anywhere in your application.

```csharp
public sealed class UserService(
    IStorageService storageService)
{
}
```

## Usage Examples

### Upload File

```csharp
var objectName = ObjectNameGenerator.Generate(
    "users",
    userId.ToString(),
    file.FileName);

await using var stream = file.OpenReadStream();

var result = await storageService.UploadAsync(
    new UploadRequest
    {
        Content = stream,
        ObjectName = objectName,
        ContentType = file.ContentType,
        Size = stream.Length
    });
```

### Check Object Existence

```csharp
var exists = await storageService.ExistsAsync(
    objectName);
```

### Generate Presigned URL

```csharp
var url = await storageService.GetPresignedUrlAsync(
    objectName);
```

### Generate Object Name

```csharp
var objectName = ObjectNameGenerator.Generate(
    "users",
    userId.ToString(),
    "avatar.png");
```

Example output:

```text
123/users/2026/06/04/8f1f4e9b5e8a4b8db6f2b0d57f8a1f29.png
```

## Testing

Run all tests:

```bash
dotnet test
```

### Unit Tests

The project contains unit tests for:

* ObjectNameGenerator
* Validation logic

### Integration Tests

Integration tests use a real MinIO instance running in Docker and verify:

* File upload
* Object existence
* Presigned URL generation
* Bucket initialization





