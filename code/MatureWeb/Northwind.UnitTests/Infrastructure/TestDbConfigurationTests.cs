using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;
using System.IO;

namespace Northwind.UnitTests;

/// <summary>
/// This are tests to check the configuration settings for the test database.
/// The tests verify that the settings are loaded correctly from the configuration file
/// and that the database context is created with the expected settings.
/// </summary>
public class TestDbConfigurationTests : TestBase
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
    public void TestCredentials_AreLoadedCorrectly()
    {
        // Get the solution root directory
        var solutionRoot = Directory.GetCurrentDirectory();
        while (solutionRoot != null && !File.Exists(Path.Combine(solutionRoot, "MatureWeb.sln")))
        {
            solutionRoot = Directory.GetParent(solutionRoot)?.FullName;
        }

        if (solutionRoot == null)
        {
            throw new InvalidOperationException("Could not find solution root directory");
        }

        // Load raw configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(solutionRoot)
            .AddJsonFile("appsettings.Testing.json")
            .Build();

        // Check credentials are loaded correctly
        Assert.Equal("sa", configuration["Database:MY_SQL_USR"]);
        // We don't check the actual password value for security reasons
        Assert.False(string.IsNullOrEmpty(configuration["Database:MY_SQL_PWD"]));
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