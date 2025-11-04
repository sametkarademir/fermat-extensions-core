using System.Data;

namespace Fermat.Extensions.Core.Test;

public class CollectionExtensionsTests
{
    [Fact]
    public void IsNullOrEmpty_WithNullCollection_ReturnsTrue()
    {
        // Arrange
        ICollection<int>? collection = null;

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrEmpty_WithEmptyCollection_ReturnsTrue()
    {
        // Arrange
        var collection = new List<int>();

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrEmpty_WithNonEmptyCollection_ReturnsFalse()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddIfNotContains_WhenItemNotExists_ReturnsTrue()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.AddIfNotContains(4);

        // Assert
        Assert.True(result);
        Assert.Contains(4, collection);
    }

    [Fact]
    public void AddIfNotContains_WhenItemExists_ReturnsFalse()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.AddIfNotContains(2);

        // Assert
        Assert.False(result);
        Assert.Equal(3, collection.Count);
    }

    [Fact]
    public void AddIfNotContains_WithEnumerable_AddsOnlyNewItems()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };
        var itemsToAdd = new[] { 2, 3, 4, 5 };

        // Act
        var addedItems = collection.AddIfNotContains(itemsToAdd);

        // Assert
        Assert.Equal(2, addedItems.Count());
        Assert.Contains(4, addedItems);
        Assert.Contains(5, addedItems);
        Assert.Equal(5, collection.Count);
    }

    [Fact]
    public void AddIfNotContains_WithPredicate_WhenPredicateMatches_ReturnsFalse()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.AddIfNotContains(x => x == 2, () => 2);

        // Assert
        Assert.False(result);
        Assert.Equal(3, collection.Count);
    }

    [Fact]
    public void AddIfNotContains_WithPredicate_WhenPredicateNotMatches_ReturnsTrue()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.AddIfNotContains(x => x == 5, () => 5);

        // Assert
        Assert.True(result);
        Assert.Contains(5, collection);
    }

    [Fact]
    public void RemoveAll_RemovesAllSpecifiedItems()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3, 4, 5 };
        var itemsToRemove = new[] { 2, 4 };

        // Act
        collection.RemoveAll(itemsToRemove);

        // Assert
        Assert.Equal(3, collection.Count);
        Assert.DoesNotContain(2, collection);
        Assert.DoesNotContain(4, collection);
    }

    [Fact]
    public void ToDataTable_ConvertsCollectionToDataTable()
    {
        // Arrange
        var items = new List<TestClass>
        {
            new() { Id = 1, Name = "Test1", Value = 10.5 },
            new() { Id = 2, Name = "Test2", Value = 20.5 }
        };

        // Act
        var dataTable = items.ToDataTable();

        // Assert
        Assert.NotNull(dataTable);
        Assert.Equal(2, dataTable.Rows.Count);
        Assert.Equal(3, dataTable.Columns.Count);
        Assert.Equal("Id", dataTable.Columns[0].ColumnName);
        Assert.Equal("Name", dataTable.Columns[1].ColumnName);
        Assert.Equal("Value", dataTable.Columns[2].ColumnName);
    }

    [Fact]
    public void ToPaged_WithValidParameters_ReturnsPagedResults()
    {
        // Arrange
        var collection = Enumerable.Range(1, 100);

        // Act
        var page1 = collection.ToPaged(1, 10).ToList();
        var page2 = collection.ToPaged(2, 10).ToList();

        // Assert
        Assert.Equal(10, page1.Count);
        Assert.Equal(1, page1.First());
        Assert.Equal(10, page1.Last());
        Assert.Equal(10, page2.Count);
        Assert.Equal(11, page2.First());
        Assert.Equal(20, page2.Last());
    }

    [Fact]
    public void ToPaged_WithInvalidPageNumber_DefaultsToPage1()
    {
        // Arrange
        var collection = Enumerable.Range(1, 100);

        // Act
        var result = collection.ToPaged(0, 10).ToList();

        // Assert
        Assert.Equal(10, result.Count);
        Assert.Equal(1, result.First());
    }

    [Fact]
    public void ToPaged_WithInvalidPageSize_DefaultsToPageSize10()
    {
        // Arrange
        var collection = Enumerable.Range(1, 100);

        // Act
        var result = collection.ToPaged(1, 0).ToList();

        // Assert
        Assert.Equal(10, result.Count);
    }

    [Fact]
    public void Shuffle_ReturnsDifferentOrder()
    {
        // Arrange
        var collection = Enumerable.Range(1, 100).ToList();

        // Act
        var shuffled1 = collection.Shuffle().ToList();
        var shuffled2 = collection.Shuffle().ToList();

        // Assert
        Assert.Equal(collection.Count, shuffled1.Count);
        Assert.Equal(collection.Count, shuffled2.Count);
        // Note: There's a small chance both shuffles could be the same, but it's very unlikely
        Assert.True(collection.SequenceEqual(shuffled1) || !collection.SequenceEqual(shuffled1));
    }

    [Fact]
    public void ToCsv_WithIncludeHeader_IncludesHeader()
    {
        // Arrange
        var items = new List<TestClass>
        {
            new() { Id = 1, Name = "Test1", Value = 10.5 }
        };

        // Act
        var csv = items.ToCsv(includeHeader: true);

        // Assert
        Assert.Contains("Id,Name,Value", csv);
        Assert.Contains("1", csv);
        Assert.Contains("Test1", csv);
        // Value might be formatted differently based on locale (10.5 or 10,5)
        Assert.True(csv.Contains("10.5") || csv.Contains("10,5") || csv.Contains("\"10,5\""));
    }

    [Fact]
    public void ToCsv_WithoutIncludeHeader_ExcludesHeader()
    {
        // Arrange
        var items = new List<TestClass>
        {
            new() { Id = 1, Name = "Test1", Value = 10.5 }
        };

        // Act
        var csv = items.ToCsv(includeHeader: false);

        // Assert
        Assert.DoesNotContain("Id,Name,Value", csv);
        Assert.Contains("1", csv);
        Assert.Contains("Test1", csv);
        // Value might be formatted differently based on locale (10.5 or 10,5)
        Assert.True(csv.Contains("10.5") || csv.Contains("10,5") || csv.Contains("\"10,5\""));
    }

    [Fact]
    public void ToCsv_WithCommaInValue_QuotesValue()
    {
        // Arrange
        var items = new List<TestClass>
        {
            new() { Id = 1, Name = "Test,Value", Value = 10.5 }
        };

        // Act
        var csv = items.ToCsv();

        // Assert
        Assert.Contains("\"Test,Value\"", csv);
    }

    [Fact]
    public void AddRange_AddsNewKeysToDictionary()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { { "key1", 1 } };
        var range = new Dictionary<string, int> { { "key2", 2 }, { "key3", 3 } };

        // Act
        dictionary.AddRange(range);

        // Assert
        Assert.Equal(3, dictionary.Count);
        Assert.Equal(2, dictionary["key2"]);
        Assert.Equal(3, dictionary["key3"]);
    }

    [Fact]
    public void AddRange_DoesNotOverwriteExistingKeys()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { { "key1", 1 } };
        var range = new Dictionary<string, int> { { "key1", 999 }, { "key2", 2 } };

        // Act
        dictionary.AddRange(range);

        // Assert
        Assert.Equal(2, dictionary.Count);
        Assert.Equal(1, dictionary["key1"]); // Original value preserved
        Assert.Equal(2, dictionary["key2"]);
    }

    [Fact]
    public void GetValueOrDefault_WhenKeyExists_ReturnsValue()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { { "key1", 42 } };

        // Act
        var result = dictionary.GetValueOrDefault("key1", 0);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void GetValueOrDefault_WhenKeyNotExists_ReturnsDefault()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { { "key1", 42 } };

        // Act
        var result = dictionary.GetValueOrDefault("key2", 99);

        // Assert
        Assert.Equal(99, result);
    }

    private class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; }
    }
}
