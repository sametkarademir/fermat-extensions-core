using System.Security.Cryptography;
using System.Text;

namespace Fermat.Extensions.Core.Test;

public class StringExtensionsTests
{
    [Fact]
    public void ReplaceToLatin_WithTurkishCharacters_ReplacesCorrectly()
    {
        // Arrange
        var text = "İstanbul";

        // Act
        var result = text.ReplaceToLatin();

        // Assert
        Assert.Equal("Istanbul", result);
    }

    [Fact]
    public void ReplaceToLatin_WithAllTurkishCharacters_ReplacesAll()
    {
        // Arrange
        var text = "İıĞğÖöÜüŞşÇç";

        // Act
        var result = text.ReplaceToLatin();

        // Assert
        Assert.Equal("IiGgOoUuSsCc", result);
    }

    [Fact]
    public void ReplaceToLatin_WithNull_ReturnsNull()
    {
        // Arrange
        string? text = null;

        // Act
        var result = text!.ReplaceToLatin();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ReplaceToLatin_WithEmptyString_ReturnsEmpty()
    {
        // Arrange
        var text = string.Empty;

        // Act
        var result = text.ReplaceToLatin();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GenerateBase64RandomId_ReturnsNonEmptyString()
    {
        // Act
        var result = StringExtensions.GenerateBase64RandomId();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GenerateBase64RandomId_ReturnsDifferentIds()
    {
        // Act
        var id1 = StringExtensions.GenerateBase64RandomId();
        var id2 = StringExtensions.GenerateBase64RandomId();

        // Assert
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void ToSlug_WithNormalString_ConvertsToSlug()
    {
        // Arrange
        var input = "Hello World";

        // Act
        var result = input.ToSlug();

        // Assert
        Assert.Equal("hello-world", result);
    }

    [Fact]
    public void ToSlug_WithTurkishCharacters_ConvertsToSlug()
    {
        // Arrange
        var input = "İstanbul Büyükşehir";

        // Act
        var result = input.ToSlug();

        // Assert
        Assert.Contains("istanbul", result);
        Assert.DoesNotContain("İ", result);
    }

    [Fact]
    public void ToSlug_WithSpecialCharacters_RemovesSpecialCharacters()
    {
        // Arrange
        var input = "Hello! @World# 123";

        // Act
        var result = input.ToSlug();

        // Assert
        Assert.DoesNotContain("!", result);
        Assert.DoesNotContain("@", result);
        Assert.DoesNotContain("#", result);
        Assert.Contains("123", result);
    }

    [Fact]
    public void ToSlug_WithNull_ReturnsEmpty()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input!.ToSlug();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToSlug_WithWhitespace_ReturnsEmpty()
    {
        // Arrange
        var input = "   ";

        // Act
        var result = input.ToSlug();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToGuidOrNull_WithValidGuid_ReturnsGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var input = guid.ToString();

        // Act
        var result = input.ToGuidOrNull();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guid, result);
    }

    [Fact]
    public void ToGuidOrNull_WithInvalidGuid_ReturnsNull()
    {
        // Arrange
        var input = "not-a-guid";

        // Act
        var result = input.ToGuidOrNull();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Truncate_WithLongString_Truncates()
    {
        // Arrange
        var input = "This is a very long string that needs to be truncated";

        // Act
        var result = input.Truncate(10);

        // Assert
        Assert.Equal("This is a ...", result);
    }

    [Fact]
    public void Truncate_WithShortString_ReturnsOriginal()
    {
        // Arrange
        var input = "Short";

        // Act
        var result = input.Truncate(10);

        // Assert
        Assert.Equal("Short", result);
    }

    [Fact]
    public void Truncate_WithCustomSuffix_UsesCustomSuffix()
    {
        // Arrange
        var input = "This is a very long string";

        // Act
        var result = input.Truncate(10, "---");

        // Assert
        Assert.Equal("This is a ---", result);
    }

    [Fact]
    public void RemoveHtmlTags_WithHtmlTags_RemovesTags()
    {
        // Arrange
        var input = "<p>Hello <b>World</b></p>";

        // Act
        var result = input.RemoveHtmlTags();

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void RemoveHtmlTags_WithReplaceWith_ReplacesWithString()
    {
        // Arrange
        var input = "<p>Hello</p>";

        // Act
        var result = input.RemoveHtmlTags(" ");

        // Assert
        Assert.Contains(" ", result);
    }

    [Fact]
    public void RemoveHtmlTags_WithNull_ReturnsEmpty()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input!.RemoveHtmlTags();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void IsValidEmail_WithValidEmail_ReturnsTrue()
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var result = email.IsValidEmail();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidEmail_WithInvalidEmail_ReturnsFalse()
    {
        // Arrange
        var email = "not-an-email";

        // Act
        var result = email.IsValidEmail();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidEmail_WithNull_ReturnsFalse()
    {
        // Arrange
        string? email = null;

        // Act
        var result = email!.IsValidEmail();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidEmail_WithWhitespace_ReturnsFalse()
    {
        // Arrange
        var email = "   ";

        // Act
        var result = email.IsValidEmail();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidUrl_WithValidHttpUrl_ReturnsTrue()
    {
        // Arrange
        var url = "http://example.com";

        // Act
        var result = url.IsValidUrl();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUrl_WithValidHttpsUrl_ReturnsTrue()
    {
        // Arrange
        var url = "https://example.com";

        // Act
        var result = url.IsValidUrl();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUrl_WithInvalidUrl_ReturnsFalse()
    {
        // Arrange
        var url = "not-a-url";

        // Act
        var result = url.IsValidUrl();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidUrl_WithNull_ReturnsFalse()
    {
        // Arrange
        string? url = null;

        // Act
        var result = url!.IsValidUrl();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ToMd5Hash_WithString_ReturnsHash()
    {
        // Arrange
        var input = "test";

        // Act
        var result = input.ToMd5Hash();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(32, result.Length); // MD5 hash is 32 characters
    }

    [Fact]
    public void ToMd5Hash_WithSameString_ReturnsSameHash()
    {
        // Arrange
        var input = "test";

        // Act
        var result1 = input.ToMd5Hash();
        var result2 = input.ToMd5Hash();

        // Assert
        Assert.Equal(result1, result2);
    }

    [Fact]
    public void ToMd5Hash_WithNull_ReturnsEmpty()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input!.ToMd5Hash();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToSha256Hash_WithString_ReturnsHash()
    {
        // Arrange
        var input = "test";

        // Act
        var result = input.ToSha256Hash();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(64, result.Length); // SHA256 hash is 64 characters
    }

    [Fact]
    public void ToSha256Hash_WithSameString_ReturnsSameHash()
    {
        // Arrange
        var input = "test";

        // Act
        var result1 = input.ToSha256Hash();
        var result2 = input.ToSha256Hash();

        // Assert
        Assert.Equal(result1, result2);
    }

    [Fact]
    public void ContainsIgnoreCase_WithSameCase_ReturnsTrue()
    {
        // Arrange
        var source = "Hello World";
        var value = "Hello";

        // Act
        var result = source.ContainsIgnoreCase(value);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsIgnoreCase_WithDifferentCase_ReturnsTrue()
    {
        // Arrange
        var source = "Hello World";
        var value = "HELLO";

        // Act
        var result = source.ContainsIgnoreCase(value);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsIgnoreCase_WithNotFound_ReturnsFalse()
    {
        // Arrange
        var source = "Hello World";
        var value = "Test";

        // Act
        var result = source.ContainsIgnoreCase(value);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsIgnoreCase_WithNullSource_ReturnsFalse()
    {
        // Arrange
        string? source = null;
        var value = "Test";

        // Act
        var result = source!.ContainsIgnoreCase(value);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ToTitleCase_ConvertsToTitleCase()
    {
        // Arrange
        var input = "hello world";

        // Act
        var result = input.ToTitleCase();

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void ToTitleCase_WithNull_ReturnsEmpty()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input!.ToTitleCase();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void StripNonAlphanumeric_RemovesNonAlphanumeric()
    {
        // Arrange
        var input = "Hello123!@#World";

        // Act
        var result = input.StripNonAlphanumeric();

        // Assert
        Assert.Equal("Hello123World", result);
    }

    [Fact]
    public void StripNonAlphanumeric_WithAllowSpace_KeepsSpaces()
    {
        // Arrange
        var input = "Hello 123 World!";

        // Act
        var result = input.StripNonAlphanumeric(allowSpace: true);

        // Assert
        Assert.Equal("Hello 123 World", result);
    }

    [Fact]
    public void SplitInParts_SplitsIntoParts()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.SplitInParts(4).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("1234", result[0]);
        Assert.Equal("5678", result[1]);
        Assert.Equal("90", result[2]);
    }

    [Fact]
    public void SplitInParts_WithNull_ThrowsException()
    {
        // Arrange
        string? input = null;

        // Act & Assert
        Assert.Throws<AggregateException>(() => input!.SplitInParts(4).ToList());
    }

    [Fact]
    public void SplitInParts_WithZeroPartLength_ThrowsException()
    {
        // Arrange
        var input = "1234567890";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => input.SplitInParts(0).ToList());
    }

    [Fact]
    public void SplitInParts_WithNegativePartLength_ThrowsException()
    {
        // Arrange
        var input = "1234567890";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => input.SplitInParts(-1).ToList());
    }
}
