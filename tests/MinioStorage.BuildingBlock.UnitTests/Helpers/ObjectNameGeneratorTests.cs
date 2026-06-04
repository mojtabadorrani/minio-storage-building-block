using FluentAssertions;
using MinioStorage.BuildingBlock.Services;

namespace MinioStorage.BuildingBlock.UnitTests.Helpers;

public sealed class ObjectNameGeneratorTests
{
    [Fact]
    public void Generate_Should_Create_ObjectName_With_Category_Date_And_Extension()
    {
        var result = ObjectNameGenerator.Generate("demo", "image.png");

        result.Should().StartWith("demo/");
        result.Should().EndWith(".png");
        result.Split('/').Should().HaveCount(5);
    }

    [Fact]
    public void Generate_Should_Normalize_Category()
    {
        var result = ObjectNameGenerator.Generate("User Files", "image.png");

        result.Should().StartWith("user-files/");
    }

    [Fact]
    public void Generate_WithOwnerId_Should_Create_ObjectName_With_Category_Owner_Date_And_Extension()
    {
        var result = ObjectNameGenerator.Generate("users", "123", "avatar.jpg");

        result.Should().StartWith("users/123/");
        result.Should().EndWith(".jpg");
        result.Split('/').Should().HaveCount(6);
    }

    [Fact]
    public void Generate_Should_Remove_Unsafe_File_Extension()
    {
        var result = ObjectNameGenerator.Generate("demo", "file.invalid-extension-name");

        result.Should().NotEndWith(".invalid-extension-name");
    }

    [Fact]
    public void Generate_Should_Throw_When_Category_Is_Empty()
    {
        Action act = () => ObjectNameGenerator.Generate("", "image.png");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Generate_WithOwnerId_Should_Throw_When_OwnerId_Is_Empty()
    {
        Action act = () => ObjectNameGenerator.Generate("users", "", "avatar.jpg");

        act.Should().Throw<ArgumentException>();
    }
    
}