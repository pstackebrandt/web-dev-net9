using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Northwind.UnitTests.Infrastructure;

/// <summary>
/// Tests for the AddNorthwindContext extension methods.
/// </summary>
/// <remarks>
/// This test class focuses on the refactored extension methods:
/// - Tests for the new overload that accepts IConfiguration directly
/// - Tests comparing both overloads to ensure identical results
/// - Tests for connection string overrides
/// - Tests for service registration with the correct lifetime
/// </remarks>
public class NorthwindContextExtensionsTests
{
    private const string DbUserConfigKey = "Database:MY_SQL_USR";
    private const string DbUserPasswordConfigKey = "Database:MY_SQL_PWD";

    /// <summary>
    /// Tests that the new overload correctly configures NorthwindContext with IConfiguration.
    /// </summary>
    [Fact]
    public void AddNorthwindContext_WithIConfiguration_ConfiguresCorrectly()
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

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        // Act
        services.AddNorthwindContext(configuration);
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<NorthwindContext>();

        // Assert
        var connectionString = context.Database.GetConnectionString();
        var builder = new SqlConnectionStringBuilder(connectionString);
        
        Assert.Equal("test-server", builder.DataSource);
        Assert.Equal("TestDB", builder.InitialCatalog);
        Assert.Equal(5, builder.ConnectTimeout);
        Assert.Equal("testuser", builder.UserID);
    }

    /// <summary>
    /// Tests that both overloads produce identical results given the same input.
    /// </summary>
    [Fact]
    public void BothOverloads_WithSameInput_ProduceIdenticalResults()
    {
        // Arrange
        var configValues = new Dictionary<string, string?>
        {
            { "DatabaseConnection:DataSource", "test-server" },
            { "DatabaseConnection:InitialCatalog", "TestDB" },
            { "DatabaseConnection:ConnectTimeout", "5" },
            { DbUserConfigKey, "testuser" },
            { DbUserPasswordConfigKey, "testpass" }
        };
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();
        
        // Setup for original method (deprecated)
        var services1 = new ServiceCollection();
        services1.AddSingleton<IConfiguration>(configuration);
#pragma warning disable CS0618 // Type or member is obsolete
        services1.AddNorthwindContext();
#pragma warning restore CS0618
        var serviceProvider1 = services1.BuildServiceProvider();
        var context1 = serviceProvider1.GetRequiredService<NorthwindContext>();
        
        // Setup for new method
        var services2 = new ServiceCollection();
        services2.AddNorthwindContext(configuration);
        var serviceProvider2 = services2.BuildServiceProvider();
        var context2 = serviceProvider2.GetRequiredService<NorthwindContext>();
        
        // Get and compare connection strings
        var connectionString1 = context1.Database.GetConnectionString();
        var connectionString2 = context2.Database.GetConnectionString();
        
        Assert.Equal(connectionString1, connectionString2);
    }

    /// <summary>
    /// Tests that the direct connection string parameter takes precedence.
    /// </summary>
    [Fact]
    public void AddNorthwindContext_WithDirectConnectionString_UsesProvidedString()
    {
        // Arrange
        var services = new ServiceCollection();
        var connectionString = "Server=override-server;Database=OverrideDB;User ID=user;Password=pass;TrustServerCertificate=True";
        
        // Act
        services.AddNorthwindContext(connectionString);
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<NorthwindContext>();
        
        // Assert
        var actualConnectionString = context.Database.GetConnectionString();
        var builder = new SqlConnectionStringBuilder(actualConnectionString);
        
        Assert.Equal("override-server", builder.DataSource);
        Assert.Equal("OverrideDB", builder.InitialCatalog);
    }

    /// <summary>
    /// Tests that connection string overrides configuration when both are provided.
    /// </summary>
    [Fact]
    public void AddNorthwindContext_WithIConfigAndConnectionString_PrioritizesConnectionString()
    {
        // Arrange
        var services = new ServiceCollection();
        var configValues = new Dictionary<string, string?>
        {
            { "DatabaseConnection:DataSource", "config-server" },
            { "DatabaseConnection:InitialCatalog", "ConfigDB" },
            { DbUserConfigKey, "configuser" },
            { DbUserPasswordConfigKey, "configpass" }
        };
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();
        
        var connectionString = "Server=override-server;Database=OverrideDB;User ID=user;Password=pass;TrustServerCertificate=True";
        
        // Act
        services.AddNorthwindContext(configuration, connectionString);
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<NorthwindContext>();
        
        // Assert
        var actualConnectionString = context.Database.GetConnectionString();
        var builder = new SqlConnectionStringBuilder(actualConnectionString);
        
        Assert.Equal("override-server", builder.DataSource);
        Assert.Equal("OverrideDB", builder.InitialCatalog);
    }

    /// <summary>
    /// Tests that the context is registered with a transient lifetime.
    /// </summary>
    [Fact]
    public void AddNorthwindContext_RegistersWithTransientLifetime()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "DatabaseConnection:DataSource", "test-server" },
                { DbUserConfigKey, "user" },
                { DbUserPasswordConfigKey, "pass" }
            })
            .Build();
        
        // Act
        services.AddNorthwindContext(configuration);
        
        // Assert
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(NorthwindContext));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    /// <summary>
    /// Tests that defaults are used when optional configuration values are missing.
    /// </summary>
    [Fact]
    public void AddNorthwindContext_WithMissingOptionalValues_UsesDefaults()
    {
        // Arrange
        var services = new ServiceCollection();
        var configValues = new Dictionary<string, string?>
        {
            // Only provide required values
            { "DatabaseConnection:DataSource", "test-server" },
            { DbUserConfigKey, "testuser" },
            { DbUserPasswordConfigKey, "testpass" }
            // Missing InitialCatalog, ConnectTimeout, etc.
        };
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();
        
        // Act
        services.AddNorthwindContext(configuration);
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<NorthwindContext>();
        
        // Assert
        var connectionString = context.Database.GetConnectionString();
        var builder = new SqlConnectionStringBuilder(connectionString);
        
        // Should use the defaults from DatabaseConnectionSettings
        Assert.Equal("Northwind", builder.InitialCatalog);
        Assert.Equal(3, builder.ConnectTimeout); // Default is 3
        Assert.True(builder.TrustServerCertificate);
    }
} 