using Northwind.EntityModels;
using Northwind.UnitTests.Infrastructure;

namespace Northwind.UnitTests;

/// <summary>
/// Tests for Northwind entity models and database access.
/// Inherits from DatabaseTestBase to use file-based configuration for real database tests.
/// </summary>
public class EntityModelTests : DatabaseTestBase
{
    [Fact]
    public void DatabaseConnectTest()
    {
        using NorthwindContext db = CreateDatabaseContext();
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public void CategoriesCountTest()
    {
        using NorthwindContext db = CreateDatabaseContext();
        int expected = 8;
        int actual = db.Categories.Count();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ProductId1IsChaiTest()
    {
        using NorthwindContext db = CreateDatabaseContext();
        string expected = "Chai";
        Product? product = db.Products.Find(keyValues: 1);
        string actual = product?.ProductName ?? string.Empty;
        Assert.Equal(expected, actual);
    }
}
