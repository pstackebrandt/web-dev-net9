using Northwind.EntityModels;

namespace Northwind.UnitTests;

/// <summary>
/// Tests for Northwind entity models and database access.
/// Inherits from TestBase to use test-specific database configuration.
/// </summary>
public class EntityModelTests : TestBase
{
    [Fact]
    public void DatabaseConnectTest()
    {
        using NorthwindContext db = CreateTestContext();
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public void CategoriesCountTest()
    {
        using NorthwindContext db = CreateTestContext();
        int expected = 8;
        int actual = db.Categories.Count();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ProductId1IsChaiTest()
    {
        using NorthwindContext db = CreateTestContext();
        string expected = "Chai";
        Product? product = db.Products.Find(keyValues: 1);
        string actual = product?.ProductName ?? string.Empty;
        Assert.Equal(expected, actual);
    }
}
