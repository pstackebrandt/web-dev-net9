using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;
using System.IO;
using System.Collections.Generic;
using Northwind.UnitTests.Infrastructure;

namespace Northwind.UnitTests;

/// <summary>
/// This are tests to check the configuration settings for the test database.
/// The tests verify that the settings are loaded correctly from the configuration file
/// and that the database context is created with the expected settings.
/// </summary>
public class TestDbConfigurationTests : ConfigurationFileTestBase
{
    [Fact]
    public void GetTestSettings_LoadsCorrectDatabaseName()
    {
        // Act
        var settings = GetTestSettings();

        // Assert
        Assert.Equal("Northwind", settings.InitialCatalog);
    }

    [Fact]
    public void GetTestSettings_LoadsCorrectTimeout()
    {
        // Act
        var settings = GetTestSettings();

        // Assert
        Assert.Equal(1, settings.ConnectTimeout);
    }

     [Fact]
    public void GetTestSettings_LoadsDatabaseUserName()
    {
        // Act
        var settings = GetTestSettings();

        // Assert
        Assert.False(string.IsNullOrEmpty(settings.UserID));
        Assert.Equal("sa", settings.UserID);
    }

    [Fact]
    public void GetTestSettings_LoadsDBUserNamePassword()
    {
        // Act
        var settings = GetTestSettings();

        // Assert
        Assert.False(string.IsNullOrEmpty(settings.UserID));
        // We don't check the actual password value for security reasons
    }

    [Fact]
    public void CreateTestContext_UsesTestDatabase()
    {
        // Act
        using  NorthwindContext? context = CreateTestContext();
        var connectionString = context.Database.GetConnectionString();

        // Parse connection string to extract properties
        var builder = new SqlConnectionStringBuilder(connectionString);

        // Assert
        Assert.Equal("Northwind", builder.InitialCatalog);
    }

    [Fact]
    public void CreateTestContext_UsesTestTimeout()
    {
        // Act
        using NorthwindContext? context = CreateTestContext();

        var connectionString = context.Database.GetConnectionString();

        // Parse connection string to extract properties
        var builder = new SqlConnectionStringBuilder(connectionString);

        // Assert
        Assert.Equal(1, builder.ConnectTimeout);
    }

    [Fact]
    public void FileBasedConfiguration_LoadsCorrectly()
    {
        // Act
        var settings = GetFileBasedTestSettings();

        // Assert
        Assert.Equal("Northwind", settings.InitialCatalog);
        Assert.Equal(1, settings.ConnectTimeout); // This should be from appsettings.Testing.json override
        Assert.False(string.IsNullOrEmpty(settings.UserID));
        Assert.False(string.IsNullOrEmpty(settings.Password));
        
        // Verify the connection string builds correctly
        var connectionString = DatabaseConnectionBuilder.CreateBuilder(settings).ConnectionString;
        var builder = new SqlConnectionStringBuilder(connectionString);
        
        Assert.Equal("Northwind", builder.InitialCatalog);
        Assert.Equal(1, builder.ConnectTimeout);
        Assert.False(string.IsNullOrEmpty(builder.UserID));
        Assert.False(string.IsNullOrEmpty(builder.Password));
    }

    [Fact]
    public void InMemoryConfiguration_WorksCorrectly()
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
    public void TestCredentials_AreLoadedCorrectly()
    {
        // Create configuration with layered approach
        var baseValues = new Dictionary<string, string?>
        {
            { "DatabaseConnection:DataSource", "base-server" },
            { "DatabaseConnection:InitialCatalog", "BaseDB" },
            { "DatabaseConnection:ConnectTimeout", "10" },
            { "Database:MY_SQL_USR", "baseuser" },
            { "Database:MY_SQL_PWD", "basepass" }
        };
        
        var overrideValues = new Dictionary<string, string?>
        {
            { "DatabaseConnection:ConnectTimeout", "1" }, // Override from testing settings
            { "Database:MY_SQL_USR", "sa" },
            { "Database:MY_SQL_PWD", "Password123!" }
        };
        
        // Build layered configuration (simulating base + testing + secrets)
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(baseValues)
            .AddInMemoryCollection(overrideValues) // Overrides previous values
            .Build();

        // Create settings
        var settings = new DatabaseConnectionSettings();
        configuration.GetSection("DatabaseConnection").Bind(settings);
        
        // Get credentials
        try
        {
            settings.UserID = configuration["Database:MY_SQL_USR"] ?? 
                throw new InvalidOperationException("Database username not found in configuration");
            settings.Password = configuration["Database:MY_SQL_PWD"] ?? 
                throw new InvalidOperationException("Database password not found in configuration");
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(
                "Database credentials are missing. If in development, ensure user secrets are configured. " +
                "See docs/user-secrets-setup.md for details.", ex);
        }

        // Verify base settings
        Assert.Equal("base-server", settings.DataSource);
        Assert.Equal("BaseDB", settings.InitialCatalog);
        
        // Verify overridden settings
        Assert.Equal(1, settings.ConnectTimeout); // Should be overridden from 10 to 1
        Assert.Equal("sa", settings.UserID); // Should be overridden
        Assert.False(string.IsNullOrEmpty(settings.Password)); // Should not be empty
    }

    [Fact]
    public void ConnectionString_UsesTestCredentials()
    {
        // Act
        using  NorthwindContext? context = CreateTestContext();
        var connectionString = context.Database.GetConnectionString();

        // Parse connection string to extract properties
        var builder = new SqlConnectionStringBuilder(connectionString);

        // Assert
        Assert.Equal("sa", builder.UserID);
        // We don't check the actual password value for security reasons
        Assert.False(string.IsNullOrEmpty(builder.Password));
    }
}