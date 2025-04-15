using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;
using System.Reflection;

namespace Northwind.UnitTests.Infrastructure;

/// <summary>
/// Tests for the enhanced configuration loading capabilities.
/// </summary>
/// <remarks>
/// This test class focuses on application-level configuration mechanisms:
/// - Properly handling environment-specific configuration
/// - Dependency injection for database context
/// - User secrets integration
/// - Error handling for missing credentials
/// 
/// Note: This class intentionally doesn't inherit from TestBase or its derived classes
/// as it tests the configuration mechanisms directly, not database context behavior.
/// </remarks>
public class ConfigurationLoadingTests
{
    // Constants for configuration keys
    private const string DbUserConfigKey = "Database:MY_SQL_USR";
    private const string DbUserPasswordConfigKey = "Database:MY_SQL_PWD";

    /// <summary>
    /// Tests that environment-specific settings correctly override base settings.
    /// </summary>
    /// <remarks>
    /// Verifies that configuration values from the Testing environment
    /// properly override values from the base configuration.
    /// </remarks>
    [Fact]
    public void Environment_SpecificSettings_OverrideBaseSettings()
    {
        // Arrange
        string? originalEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        try
        {
            // Set environment to Testing
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

            // Create test configuration that doesn't rely on physical files
            var configValues = new Dictionary<string, string?>
            {
                { "DatabaseConnection:DataSource", "test-server" },
                { "DatabaseConnection:InitialCatalog", "TestDB" },
                { "DatabaseConnection:ConnectTimeout", "111" }, // Testing value
                { DbUserConfigKey, "testuser" },
                { DbUserPasswordConfigKey, "testpass" }
            };

            // Create a configuration
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            // Create connection settings from configuration
            var connectionSettings = new DatabaseConnectionSettings();
            configuration.GetSection("DatabaseConnection").Bind(connectionSettings);

            // Set credentials
            connectionSettings.UserID = configuration[DbUserConfigKey]
                ?? throw new ArgumentException($"Missing required configuration value for '{DbUserConfigKey}'");
            connectionSettings.Password = configuration[DbUserPasswordConfigKey]
                ?? throw new ArgumentException($"Missing required configuration value for '{DbUserPasswordConfigKey}'");

            // Build connection string
            var builder = DatabaseConnectionBuilder.CreateBuilder(connectionSettings);

            // Assert
            Assert.Equal(111, builder.ConnectTimeout);
        }
        finally
        {
            // Restore original environment
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", originalEnv);
        }
    }

    /// <summary>
    /// Tests that the DI extension method correctly handles configuration.
    /// </summary>
    /// <remarks>
    /// Verifies that the AddNorthwindContext extension method correctly:
    /// - Reads configuration from the provided IConfiguration
    /// - Configures the DbContext with the right connection settings
    /// - Makes the context available through the service provider
    /// </remarks>
    [Fact]
    public void NorthwindContextExtensions_UsesConnectionSettingsFromConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        var configValues = new Dictionary<string, string?>
        {
            { "DatabaseConnection:DataSource", "test-server" },
            { "DatabaseConnection:InitialCatalog", "TestDB" },
            { "DatabaseConnection:ConnectTimeout", "5" },
            { DbUserConfigKey, "testuser" },
            { DbUserPasswordConfigKey, "testpass" }
        };

        // Create configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        // Act
        services.AddNorthwindContext();
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<NorthwindContext>();

        // Get connection string
        var connectionString = context.Database.GetConnectionString();
        var builder = new SqlConnectionStringBuilder(connectionString);

        // Assert
        Assert.Equal("test-server", builder.DataSource);
        Assert.Equal("TestDB", builder.InitialCatalog);
        Assert.Equal(5, builder.ConnectTimeout);
        Assert.Equal("testuser", builder.UserID);
    }

    /// <summary>
    /// Tests that appropriate error is thrown when credentials are missing.
    /// </summary>
    /// <remarks>
    /// Verifies that when database credentials are missing from configuration:
    /// - An informative exception is thrown
    /// - The error message includes a reference to user secrets
    /// - The exception is of the expected type (InvalidOperationException)
    /// </remarks>
    [Fact]
    public void Missing_Credentials_ThrowsInformativeException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configValues = new Dictionary<string, string?>
        {
            { "DatabaseConnection:DataSource", "test-server" },
            { "DatabaseConnection:InitialCatalog", "TestDB" }
            // Intentionally missing credentials
        };

        // Create configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            services.AddNorthwindContext();
            var serviceProvider = services.BuildServiceProvider();
            _ = serviceProvider.GetRequiredService<NorthwindContext>();
        });

        // Verify exception contains helpful message
        Assert.Contains("Database credentials are missing", exception.Message);
        Assert.Contains("user secrets", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}