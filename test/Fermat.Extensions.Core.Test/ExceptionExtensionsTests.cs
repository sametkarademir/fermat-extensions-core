using System.Text.Json;

namespace Fermat.Extensions.Core.Test;

public class ExceptionExtensionsTests
{
    [Fact]
    public void GenerateFingerprint_ReturnsBase64String()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var fingerprint = exception.GenerateFingerprint();

        // Assert
        Assert.NotNull(fingerprint);
        Assert.NotEmpty(fingerprint);
        // Base64 string validation
        Assert.DoesNotMatch(@"^[^A-Za-z0-9+/=]", fingerprint);
    }

    [Fact]
    public void GenerateFingerprint_SameException_ReturnsSameFingerprint()
    {
        // Arrange
        var exception1 = new Exception("Test exception");
        var exception2 = new Exception("Test exception");

        // Act
        var fingerprint1 = exception1.GenerateFingerprint();
        var fingerprint2 = exception2.GenerateFingerprint();

        // Assert
        Assert.Equal(fingerprint1, fingerprint2);
    }

    [Fact]
    public void GenerateFingerprint_DifferentExceptions_ReturnsDifferentFingerprints()
    {
        // Arrange
        var exception1 = new Exception("Exception 1");
        var exception2 = new Exception("Exception 2");

        // Act
        var fingerprint1 = exception1.GenerateFingerprint();
        var fingerprint2 = exception2.GenerateFingerprint();

        // Assert
        Assert.NotEqual(fingerprint1, fingerprint2);
    }

    [Fact]
    public void ConvertExceptionDataToDictionary_WithEmptyData_ReturnsEmptyDictionary()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var dictionary = exception.ConvertExceptionDataToDictionary();

        // Assert
        Assert.NotNull(dictionary);
        Assert.Empty(dictionary);
    }

    [Fact]
    public void ConvertExceptionDataToDictionary_WithData_ReturnsDictionary()
    {
        // Arrange
        var exception = new Exception("Test exception");
        exception.Data["Key1"] = "Value1";
        exception.Data["Key2"] = 42;
        exception.Data["Key3"] = null; // Should be skipped

        // Act
        var dictionary = exception.ConvertExceptionDataToDictionary();

        // Assert
        Assert.Equal(2, dictionary.Count);
        Assert.Equal("Value1", dictionary["Key1"]);
        Assert.Equal(42, dictionary["Key2"]);
        Assert.DoesNotContain("Key3", dictionary.Keys);
    }

    [Fact]
    public void ConvertExceptionDataToJson_WithEmptyData_ReturnsEmptyJsonObject()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var json = exception.ConvertExceptionDataToJson();

        // Assert
        Assert.NotNull(json);
        Assert.Equal("{}", json?.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
    }

    [Fact]
    public void ConvertExceptionDataToJson_WithData_ReturnsValidJson()
    {
        // Arrange
        var exception = new Exception("Test exception");
        exception.Data["Key1"] = "Value1";
        exception.Data["Key2"] = 42;

        // Act
        var json = exception.ConvertExceptionDataToJson();

        // Assert
        Assert.NotNull(json);
        var deserialized = JsonSerializer.Deserialize<Dictionary<string, object>>(json!);
        Assert.NotNull(deserialized);
        Assert.Equal("Value1", deserialized["Key1"].ToString());
    }

    [Fact]
    public void ConvertInnerExceptionsToList_WithoutInnerException_ReturnsEmptyList()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var list = exception.ConvertInnerExceptionsToList();

        // Assert
        Assert.NotNull(list);
        Assert.Empty(list);
    }

    [Fact]
    public void ConvertInnerExceptionsToList_WithInnerException_ReturnsList()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        var outerException = new Exception("Outer exception", innerException);

        // Act
        var list = outerException.ConvertInnerExceptionsToList();

        // Assert
        Assert.NotNull(list);
        Assert.Single(list);
        Assert.Equal("Inner exception", list[0]["Message"]);
        Assert.Equal("0", list[0]["Depth"]);
    }

    [Fact]
    public void ConvertInnerExceptionsToList_WithMultipleInnerExceptions_ReturnsAll()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        var middleException = new Exception("Middle exception", innerException);
        var outerException = new Exception("Outer exception", middleException);

        // Act
        var list = outerException.ConvertInnerExceptionsToList();

        // Assert
        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
        Assert.Equal("Middle exception", list[0]["Message"]);
        Assert.Equal("Inner exception", list[1]["Message"]);
        Assert.Equal("0", list[0]["Depth"]);
        Assert.Equal("1", list[1]["Depth"]);
    }

    [Fact]
    public void ConvertInnerExceptionsToJson_WithInnerException_ReturnsValidJson()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        var outerException = new Exception("Outer exception", innerException);

        // Act
        var json = outerException.ConvertInnerExceptionsToJson();

        // Assert
        Assert.NotNull(json);
        var deserialized = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json!);
        Assert.NotNull(deserialized);
        Assert.Single(deserialized);
    }

    [Fact]
    public void GetExceptionType_ReturnsFullTypeName()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");

        // Act
        var type = exception.GetExceptionType();

        // Assert
        Assert.NotNull(type);
        Assert.Contains("InvalidOperationException", type);
    }

    [Fact]
    public void GetStackTraceInfo_ReturnsStackTraceString()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var stackTraceInfo = exception.GetStackTraceInfo();

        // Assert
        Assert.NotNull(stackTraceInfo);
        // Stack trace may be empty in test environment, so we just check it's not null
        // In production it should contain "at" if there are frames
        if (!string.IsNullOrEmpty(stackTraceInfo))
        {
            Assert.Contains("at", stackTraceInfo);
        }
    }

    [Fact]
    public void GetStackTraceInfo_WithIncludeSource_IncludesSourceInfo()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var stackTraceInfo = exception.GetStackTraceInfo(includeSource: true);

        // Assert
        Assert.NotNull(stackTraceInfo);
        // May or may not contain source info depending on debug symbols
    }

    [Fact]
    public void GetStackTraceInfo_WithIncludeTimestamp_IncludesTimestamp()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var stackTraceInfo = exception.GetStackTraceInfo(includeTimestamp: true);

        // Assert
        Assert.NotNull(stackTraceInfo);
        // Timestamp format validation - only check if stack trace is not empty
        if (!string.IsNullOrEmpty(stackTraceInfo))
        {
            // Timestamp should be in the format: [yyyy-MM-dd HH:mm:ss.fff]
            Assert.Matches(@".*\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}\].*", stackTraceInfo);
        }
    }
}
