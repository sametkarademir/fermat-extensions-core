namespace Fermat.Extensions.Core.Test;

public class NormalizationExtensionsTests
{
    [Fact]
    public void NormalizeValue_WithNullString_ReturnsNull()
    {
        // Arrange
        string? value = null;

        // Act
        var result = value!.NormalizeValue();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void NormalizeValue_WithEmptyString_ReturnsEmpty()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var result = value.NormalizeValue();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void NormalizeValue_WithDiacritics_RemovesDiacritics()
    {
        // Arrange
        var value = "café";

        // Act
        var result = value.NormalizeValue();

        // Assert
        Assert.Equal("CAFE", result);
    }

    [Fact]
    public void NormalizeValue_WithAccentedCharacters_RemovesAccents()
    {
        // Arrange
        var value = "résumé";

        // Act
        var result = value.NormalizeValue();

        // Assert
        Assert.Equal("RESUME", result);
    }

    [Fact]
    public void NormalizeValue_WithTurkishCharacters_RemovesDiacritics()
    {
        // Arrange
        var value = "İstanbul";

        // Act
        var result = value.NormalizeValue();

        // Assert
        Assert.Equal("ISTANBUL", result);
    }

    [Fact]
    public void NormalizeValue_ConvertsToUpperCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.NormalizeValue();

        // Assert
        Assert.Equal("HELLO WORLD", result);
    }

    [Fact]
    public void NormalizeValue_WithMixedCase_ConvertsToUpperCase()
    {
        // Arrange
        var value = "Hello WoRLd";

        // Act
        var result = value.NormalizeValue();

        // Assert
        Assert.Equal("HELLO WORLD", result);
    }

    [Fact]
    public void NormalizeValue_WithSpecialCharacters_PreservesWhenNotDiacritics()
    {
        // Arrange
        var value = "Test123!@#";

        // Act
        var result = value.NormalizeValue();

        // Assert
        Assert.Contains("TEST", result);
        Assert.Contains("123", result);
    }
}
