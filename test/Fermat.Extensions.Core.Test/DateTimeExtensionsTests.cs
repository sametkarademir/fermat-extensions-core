namespace Fermat.Extensions.Core.Test;

public class DateTimeExtensionsTests
{
    [Fact]
    public void ToUnixTimestamp_ConvertsDateTimeToUnixTimestamp()
    {
        // Arrange
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(1234567890);

        // Act
        var timestamp = dateTime.ToUnixTimestamp();

        // Assert
        Assert.Equal(1234567890, timestamp);
    }

    [Fact]
    public void FromUnixTimestamp_ConvertsUnixTimestampToDateTime()
    {
        // Arrange
        var timestamp = 1234567890L;

        // Act
        var dateTime = timestamp.FromUnixTimestamp();

        // Assert
        Assert.Equal(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(1234567890), dateTime);
    }

    [Fact]
    public void ToUnixTimestamp_And_FromUnixTimestamp_AreReversible()
    {
        // Arrange
        var originalDateTime = new DateTime(2023, 6, 15, 14, 30, 0, DateTimeKind.Utc);

        // Act
        var timestamp = originalDateTime.ToUnixTimestamp();
        var convertedDateTime = timestamp.FromUnixTimestamp();

        // Assert
        Assert.Equal(originalDateTime, convertedDateTime);
    }

    [Fact]
    public void StartOfWeek_WithDefaultStartOfWeek_ReturnsMonday()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 15, 14, 30, 0); // Thursday

        // Act
        var startOfWeek = dateTime.StartOfWeek();

        // Assert
        Assert.Equal(DayOfWeek.Monday, startOfWeek.DayOfWeek);
        Assert.Equal(new DateTime(2023, 6, 12, 0, 0, 0), startOfWeek);
    }

    [Fact]
    public void StartOfWeek_WithCustomStartOfWeek_ReturnsCorrectDay()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 15, 14, 30, 0); // Thursday

        // Act
        var startOfWeek = dateTime.StartOfWeek(DayOfWeek.Sunday);

        // Assert
        Assert.Equal(DayOfWeek.Sunday, startOfWeek.DayOfWeek);
        Assert.Equal(new DateTime(2023, 6, 11, 0, 0, 0), startOfWeek);
    }

    [Fact]
    public void StartOfMonth_ReturnsFirstDayOfMonth()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 15, 14, 30, 0);

        // Act
        var startOfMonth = dateTime.StartOfMonth();

        // Assert
        Assert.Equal(new DateTime(2023, 6, 1, 0, 0, 0, dateTime.Kind), startOfMonth);
    }

    [Fact]
    public void EndOfMonth_ReturnsLastDayOfMonth()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 15, 14, 30, 0);

        // Act
        var endOfMonth = dateTime.EndOfMonth();

        // Assert
        Assert.Equal(new DateTime(2023, 6, 30, 23, 59, 59, 999, dateTime.Kind), endOfMonth);
    }

    [Fact]
    public void EndOfMonth_WithFebruary_ReturnsCorrectLastDay()
    {
        // Arrange
        var dateTime = new DateTime(2024, 2, 15, 14, 30, 0); // Leap year

        // Act
        var endOfMonth = dateTime.EndOfMonth();

        // Assert
        Assert.Equal(new DateTime(2024, 2, 29, 23, 59, 59, 999, dateTime.Kind), endOfMonth);
    }

    [Fact]
    public void IsWeekend_WithSaturday_ReturnsTrue()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 10); // Saturday

        // Act
        var result = dateTime.IsWeekend();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsWeekend_WithSunday_ReturnsTrue()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 11); // Sunday

        // Act
        var result = dateTime.IsWeekend();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsWeekend_WithWeekday_ReturnsFalse()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 12); // Monday

        // Act
        var result = dateTime.IsWeekend();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsBetween_WithInclusiveRange_IncludesBoundaries()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 15);
        var startDate = new DateTime(2023, 6, 15);
        var endDate = new DateTime(2023, 6, 20);

        // Act
        var result = dateTime.IsBetween(startDate, endDate, inclusive: true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsBetween_WithExclusiveRange_ExcludesBoundaries()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 15);
        var startDate = new DateTime(2023, 6, 15);
        var endDate = new DateTime(2023, 6, 20);

        // Act
        var result = dateTime.IsBetween(startDate, endDate, inclusive: false);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsBetween_WithDateInMiddle_ReturnsTrue()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 17);
        var startDate = new DateTime(2023, 6, 15);
        var endDate = new DateTime(2023, 6, 20);

        // Act
        var result = dateTime.IsBetween(startDate, endDate, inclusive: true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsBetween_WithDateBeforeRange_ReturnsFalse()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 10);
        var startDate = new DateTime(2023, 6, 15);
        var endDate = new DateTime(2023, 6, 20);

        // Act
        var result = dateTime.IsBetween(startDate, endDate, inclusive: true);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsBetween_WithDateAfterRange_ReturnsFalse()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 25);
        var startDate = new DateTime(2023, 6, 15);
        var endDate = new DateTime(2023, 6, 20);

        // Act
        var result = dateTime.IsBetween(startDate, endDate, inclusive: true);

        // Assert
        Assert.False(result);
    }
}
