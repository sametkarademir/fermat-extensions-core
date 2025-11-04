namespace Fermat.Extensions.Core.Test;

public class ObjectExtensionsTests
{
    [Fact]
    public void CastAs_WithValidCast_ReturnsCastObject()
    {
        // Arrange
        object obj = "test";

        // Act
        var result = obj.CastAs<string>();

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void CastAs_WithInterface_ReturnsInterface()
    {
        // Arrange
        var list = new List<int>();
        object obj = list;

        // Act
        var result = obj.CastAs<IList<int>>();

        // Assert
        Assert.NotNull(result);
        Assert.Same(list, result);
    }

    [Fact]
    public void ConvertTo_WithInt_ConvertsSuccessfully()
    {
        // Arrange
        object obj = "42";

        // Act
        var result = obj.ConvertTo<int>();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void ConvertTo_WithDouble_ConvertsSuccessfully()
    {
        // Arrange
        object obj = "3.14";

        // Act
        var result = obj.ConvertTo<double>();

        // Assert
        Assert.Equal(3.14, result);
    }

    [Fact]
    public void ConvertTo_WithGuid_ConvertsSuccessfully()
    {
        // Arrange
        var guid = Guid.NewGuid();
        object obj = guid.ToString();

        // Act
        var result = obj.ConvertTo<Guid>();

        // Assert
        Assert.Equal(guid, result);
    }

    [Fact]
    public void ConvertTo_WithDecimal_ConvertsSuccessfully()
    {
        // Arrange
        object obj = 3.14;

        // Act
        var result = obj.ConvertTo<decimal>();

        // Assert
        Assert.Equal(3.14m, result);
    }

    [Fact]
    public void ExistsInCollection_WithParams_WhenItemExists_ReturnsTrue()
    {
        // Arrange
        var item = 5;

        // Act
        var result = item.ExistsInCollection(1, 3, 5, 7, 9);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ExistsInCollection_WithParams_WhenItemNotExists_ReturnsFalse()
    {
        // Arrange
        var item = 6;

        // Act
        var result = item.ExistsInCollection(1, 3, 5, 7, 9);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ExistsInCollection_WithEnumerable_WhenItemExists_ReturnsTrue()
    {
        // Arrange
        var item = 42;
        var collection = new List<int> { 10, 20, 30, 42, 50 };

        // Act
        var result = item.ExistsInCollection(collection);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ExistsInCollection_WithEnumerable_WhenItemNotExists_ReturnsFalse()
    {
        // Arrange
        var item = 99;
        var collection = new List<int> { 10, 20, 30, 42, 50 };

        // Act
        var result = item.ExistsInCollection(collection);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DoIf_WithTrueCondition_AppliesFunction()
    {
        // Arrange
        var number = 10;
        var shouldMultiply = true;

        // Act
        var result = number.DoIf(shouldMultiply, n => n * 2);

        // Assert
        Assert.Equal(20, result);
    }

    [Fact]
    public void DoIf_WithFalseCondition_DoesNotApplyFunction()
    {
        // Arrange
        var number = 10;
        var shouldMultiply = false;

        // Act
        var result = number.DoIf(shouldMultiply, n => n * 2);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void DoIf_WithChainedCalls_AppliesConditionally()
    {
        // Arrange
        var number = 10;
        var shouldMultiply = true;
        var shouldAdd = false;

        // Act
        var result = number
            .DoIf(shouldMultiply, n => n * 2)
            .DoIf(shouldAdd, n => n + 5);

        // Assert
        Assert.Equal(20, result);
    }

    [Fact]
    public void DoIf_WithAction_WithTrueCondition_ExecutesAction()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        var shouldAdd = true;
        var added = false;

        // Act
        var result = list.DoIf(shouldAdd, l =>
        {
            l.Add(4);
            added = true;
        });

        // Assert
        Assert.True(added);
        Assert.Equal(4, list.Count);
        Assert.Same(list, result);
    }

    [Fact]
    public void DoIf_WithAction_WithFalseCondition_DoesNotExecuteAction()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        var shouldAdd = false;
        var added = false;

        // Act
        var result = list.DoIf(shouldAdd, l =>
        {
            l.Add(4);
            added = true;
        });

        // Assert
        Assert.False(added);
        Assert.Equal(3, list.Count);
        Assert.Same(list, result);
    }
}