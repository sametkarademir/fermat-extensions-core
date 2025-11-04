using System.Linq.Expressions;

namespace Fermat.Extensions.Core.Test;

public class LinqExtensionsTests
{
    [Fact]
    public void WhereIf_WithTrueCondition_AppliesFilter()
    {
        // Arrange
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = query.WhereIf(true, x => x > 3).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public void WhereIf_WithFalseCondition_DoesNotApplyFilter()
    {
        // Arrange
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = query.WhereIf(false, x => x > 3).ToList();

        // Assert
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void WhereIf_WithEnumerable_WithTrueCondition_AppliesFilter()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = source.WhereIf(true, x => x > 3).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public void WhereIf_WithEnumerable_WithFalseCondition_DoesNotApplyFilter()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = source.WhereIf(false, x => x > 3).ToList();

        // Assert
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void OrderByIf_WithTrueCondition_Ascending_OrdersAscending()
    {
        // Arrange
        var query = new[] { 3, 1, 4, 2, 5 }.AsQueryable();

        // Act
        var result = query.OrderByIf(true, x => x, ascending: true).ToList();

        // Assert
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result);
    }

    [Fact]
    public void OrderByIf_WithTrueCondition_Descending_OrdersDescending()
    {
        // Arrange
        var query = new[] { 3, 1, 4, 2, 5 }.AsQueryable();

        // Act
        var result = query.OrderByIf(true, x => x, ascending: false).ToList();

        // Assert
        Assert.Equal(new[] { 5, 4, 3, 2, 1 }, result);
    }

    [Fact]
    public void OrderByIf_WithFalseCondition_DoesNotOrder()
    {
        // Arrange
        var query = new[] { 3, 1, 4, 2, 5 }.AsQueryable();

        // Act
        var result = query.OrderByIf(false, x => x, ascending: true).ToList();

        // Assert
        Assert.Equal(new[] { 3, 1, 4, 2, 5 }, result);
    }

    [Fact]
    public void ThenByIf_WithTrueCondition_Ascending_OrdersAscending()
    {
        // Arrange
        var query = new[] { new { A = 1, B = 3 }, new { A = 1, B = 1 }, new { A = 2, B = 2 } }
            .AsQueryable()
            .OrderBy(x => x.A);

        // Act
        var result = query.ThenByIf(true, x => x.B, ascending: true).ToList();

        // Assert
        Assert.Equal(1, result[0].A);
        Assert.Equal(1, result[0].B);
        Assert.Equal(1, result[1].A);
        Assert.Equal(3, result[1].B);
    }

    [Fact]
    public void ThenByIf_WithFalseCondition_DoesNotOrder()
    {
        // Arrange
        var query = new[] { new { A = 1, B = 3 }, new { A = 1, B = 1 }, new { A = 2, B = 2 } }
            .AsQueryable()
            .OrderBy(x => x.A);

        // Act
        var result = query.ThenByIf(false, x => x.B, ascending: true).ToList();

        // Assert
        Assert.Equal(1, result[0].A);
        Assert.Equal(3, result[0].B); // Original order preserved
    }

    [Fact]
    public void SkipIf_WithTrueCondition_SkipsElements()
    {
        // Arrange
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = query.SkipIf(true, 2).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 3, 4, 5 }, result);
    }

    [Fact]
    public void SkipIf_WithFalseCondition_DoesNotSkip()
    {
        // Arrange
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = query.SkipIf(false, 2).ToList();

        // Assert
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void TakeIf_WithTrueCondition_TakesElements()
    {
        // Arrange
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = query.TakeIf(true, 3).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    [Fact]
    public void TakeIf_WithFalseCondition_DoesNotTake()
    {
        // Arrange
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = query.TakeIf(false, 3).ToList();

        // Assert
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void SelectIf_WithTrueCondition_UsesFirstSelector()
    {
        // Arrange
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = query.SelectIf(true, x => x * 2, x => x * 3).ToList();

        // Assert
        Assert.Equal(new[] { 2, 4, 6, 8, 10 }, result);
    }

    [Fact]
    public void SelectIf_WithFalseCondition_UsesAlternativeSelector()
    {
        // Arrange
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = query.SelectIf(false, x => x * 2, x => x * 3).ToList();

        // Assert
        Assert.Equal(new[] { 3, 6, 9, 12, 15 }, result);
    }
}
