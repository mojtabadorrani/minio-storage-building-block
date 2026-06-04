using FluentAssertions;
using MinioStorage.BuildingBlock.Services;

namespace MinioStorage.BuildingBlock.UnitTests.Services;

public sealed class ObjectNameGeneratorTests
{
    [Fact]
    public void Generate_WithCategoryOnly_ReturnsPathWithDateAndGuid()
    {
        const string category = "demo";
        const string fileName = "image.png";

        var result = ObjectNameGenerator.Generate(category, fileName);

        result.Should().StartWith("demo/");
        result.Should().EndWith(".png");
        result.Split('/').Length.Should().Be(5); // demo/yyyy/MM/dd/guid.png
    }
    
    [Fact]
    public void Generate_WithCategoryAndOwner_ReturnsPathWithOwner()
    {
        var category = "users";
        var ownerId = "123";
        var fileName = "avatar.jpg";

        var result = ObjectNameGenerator.Generate(category, ownerId, fileName);

        result.Should().StartWith("users/123/");
        result.Should().EndWith(".jpg");
    }
    
    [Fact]
    public void Generate_Throws_WhenCategoryEmpty()
    {
        Action act = () => ObjectNameGenerator.Generate("", "123", "file.png");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Generate_Throws_WhenOwnerIdEmpty()
    {
        Action act = () => ObjectNameGenerator.Generate("users", "", "file.png");
        act.Should().Throw<ArgumentException>();
    }
    
}