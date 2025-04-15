using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Northwind.UnitTests;

/// <summary>
/// Base class for tests using in-memory database.
/// </summary>
/// <remarks>
/// This class provides functionality for tests that need an isolated, in-memory database.
/// Use this base class for tests that:
/// - Need to test business logic without database dependencies
/// - Require predictable data state without side effects
/// - Need to run quickly without external dependencies
/// </remarks>
public class InMemoryTestBase : TestBase
{
    /// <summary>
    /// Retrieves database connection settings from in-memory test configuration.
    /// </summary>
    /// <returns>Database connection settings configured for in-memory testing.</returns>
    /// <remarks>
    /// This method creates an in-memory configuration with test values
    /// instead of relying on physical configuration files.
    /// </remarks>
    protected static DatabaseConnectionSettings GetInMemoryTestSettings()
    {
        // Create in-memory test configuration
        var configValues = new Dictionary<string, string?>
        {
            { "DatabaseConnection:DataSource", "tcp:127.0.0.1,1433" },
            { "DatabaseConnection:InitialCatalog", "Northwind" },
            { "DatabaseConnection:TrustServerCertificate", "true" },
            { "DatabaseConnection:MultipleActiveResultSets", "true" },
            { "DatabaseConnection:ConnectTimeout", "1" }, // Testing timeout value
            { "Database:MY_SQL_USR", "sa" },
            { "Database:MY_SQL_PWD", "Password123!" } // Test password, not a real one
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        var settings = new DatabaseConnectionSettings();
        configuration.GetSection("DatabaseConnection").Bind(settings);

        // Set credentials from configuration
        settings.UserID = configuration["Database:MY_SQL_USR"] ??
            throw new InvalidOperationException("Database username not found in configuration");
        settings.Password = configuration["Database:MY_SQL_PWD"] ??
            throw new InvalidOperationException("Database password not found in configuration");

        return settings;
    }

    /// <summary>
    /// Creates a new instance of NorthwindContext configured with in-memory database.
    /// </summary>
    /// <returns>A NorthwindContext instance with in-memory database.</returns>
    /// <remarks>
    /// This method creates an isolated, in-memory database for testing.
    /// Each call creates a database with a unique name to ensure test isolation.
    /// </remarks>
    protected static NorthwindContext CreateInMemoryContext()
    {
        // Generate a unique database name to ensure isolation between tests
        string uniqueDatabaseName = $"NorthwindTest_{Guid.NewGuid()}";
        
        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseInMemoryDatabase(databaseName: uniqueDatabaseName)
            .Options;
            
        return new NorthwindContext(options);
    }
} 