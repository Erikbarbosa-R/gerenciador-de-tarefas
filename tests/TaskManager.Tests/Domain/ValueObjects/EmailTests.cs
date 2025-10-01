using FluentAssertions;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Tests.Domain.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("teste@teste.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("user+tag@example.org")]
    public void Constructor_ValidEmail_ShouldCreateEmail(string emailValue)
    {
        // Act
        var email = new Email(emailValue);

        // Assert
        email.Value.Should().Be(emailValue.ToLowerInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_InvalidEmail_ShouldThrowException(string emailValue)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Email(emailValue));
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user.domain.com")]
    public void Constructor_MalformedEmail_ShouldThrowException(string emailValue)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Email(emailValue));
    }

    [Fact]
    public void Equals_SameEmail_ShouldReturnTrue()
    {
        // Arrange
        var email1 = new Email("teste@teste.com");
        var email2 = new Email("TESTE@TESTE.COM");

        // Act & Assert
        email1.Equals(email2).Should().BeTrue();
        (email1 == email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentEmail_ShouldReturnFalse()
    {
        // Arrange
        var email1 = new Email("teste1@teste.com");
        var email2 = new Email("teste2@teste.com");

        // Act & Assert
        email1.Equals(email2).Should().BeFalse();
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldWork()
    {
        // Arrange
        var email = new Email("teste@teste.com");

        // Act
        string emailString = email;

        // Assert
        emailString.Should().Be("teste@teste.com");
    }

    [Fact]
    public void ExplicitConversion_FromString_ShouldWork()
    {
        // Act
        Email email = (Email)"teste@teste.com";

        // Assert
        email.Value.Should().Be("teste@teste.com");
    }
}
