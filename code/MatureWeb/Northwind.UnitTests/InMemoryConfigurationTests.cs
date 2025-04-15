using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Northwind.Shared.Configuration;
using Xunit;

namespace Northwind.UnitTests;

/// <summary>
/// Tests for in-memory database configuration.
/// </summary>
/// <remarks>
/// These tests verify the in-memory database configuration settings.
/// </remarks>
public class InMemoryConfigurationTests : InMemoryTestBase
{
    [Fact]
    public void InMemoryConfiguration_LoadsCorrectly()
    {
        // Act
        var settings = GetInMemoryTestSettings();

        // Assert
        Assert.Equal("Northwind", settings.InitialCatalog);
        Assert.Equal(1, settings.ConnectTimeout);
        Assert.Equal("sa", settings.UserID);
        Assert.False(string.IsNullOrEmpty(settings.Password));
        
        // Verify the connection string builds correctly
        var connectionString = DatabaseConnectionBuilder.CreateBuilder(settings).ConnectionString;
        var builder = new SqlConnectionStringBuilder(connectionString);
        
        Assert.Equal("Northwind", builder.InitialCatalog);
        Assert.Equal(1, builder.ConnectTimeout);
        Assert.Equal("sa", builder.UserID);
        Assert.False(string.IsNullOrEmpty(builder.Password));
    }
    
    [Fact]
    public void CreateInMemoryContext_CreatesIsolatedDatabases()
    {
        // Arrange
        // Create two contexts
        var context1 = CreateInMemoryContext();
        var context2 = CreateInMemoryContext();
        
        // Act
        // Add a test entity to the first context
        context1.Categories.Add(new Northwind.EntityModels.Category 
        { 
            CategoryName = "Test Category", 
            Description = "Created in first context" 
        });
        context1.SaveChanges();
        
        // Assert
        // Verify the second context doesn't see the entity from the first context
        Assert.Equal(0, context2.Categories.Count());
        
        // Clean up
        context1.Dispose();
        context2.Dispose();
    }
} 