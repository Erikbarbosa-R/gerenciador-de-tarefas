using FluentAssertions;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Tests.Domain.ValueObjects;

public class PasswordHashTests
{
    [Fact]
    public void Create_ValidPassword_ShouldCreatePasswordHash()
    {
        // Arrange
        var password = "senha123";

        // Act
        var passwordHash = PasswordHash.Create(password);

        // Assert
        passwordHash.Should().NotBeNull();
        passwordHash.Value.Should().NotBeNullOrEmpty();
        passwordHash.Value.Should().NotBe(password);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidPassword_ShouldThrowException(string? password)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PasswordHash.Create(password!));
    }

    [Theory]
    [InlineData("123")]
    [InlineData("abc")]
    [InlineData("12345")]
    public void Create_ShortPassword_ShouldThrowException(string password)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PasswordHash.Create(password));
    }

    [Fact]
    public void Verify_CorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "senha123";
        var passwordHash = PasswordHash.Create(password);

        // Act
        var result = passwordHash.Verify(password);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Verify_IncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "senha123";
        var wrongPassword = "senha456";
        var passwordHash = PasswordHash.Create(password);

        // Act
        var result = passwordHash.Verify(wrongPassword);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Verify_EmptyPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "senha123";
        var passwordHash = PasswordHash.Create(password);

        // Act
        var result = passwordHash.Verify("");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_SameHash_ShouldReturnTrue()
    {
        // Arrange
        var password = "senha123";
        var hash1 = PasswordHash.Create(password);
        var hash2 = new PasswordHash(hash1.Value);

        // Act & Assert
        hash1.Equals(hash2).Should().BeTrue();
        (hash1 == hash2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentHash_ShouldReturnFalse()
    {
        // Arrange
        var hash1 = PasswordHash.Create("senha123");
        var hash2 = PasswordHash.Create("senha456");

        // Act & Assert
        hash1.Equals(hash2).Should().BeFalse();
        (hash1 != hash2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnRedacted()
    {
        // Arrange
        var passwordHash = PasswordHash.Create("senha123");

        // Act
        var result = passwordHash.ToString();

        // Assert
        result.Should().Be("[REDACTED]");
    }
}
