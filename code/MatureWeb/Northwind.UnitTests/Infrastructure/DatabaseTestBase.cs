using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;

namespace Northwind.UnitTests.Infrastructure;

/// <summary>
/// Base class for database tests that require a real database connection.
/// </summary>
/// <remarks>
/// This class provides functionality for tests requiring a real database connection.
/// Use this base class for tests that:
/// - Verify actual database connectivity
/// - Validate entity mapping against real database schema
/// - Test database-specific functionality
/// - Need to test with real SQL Server features
/// </remarks>
public abstract class DatabaseTestBase : TestBase
{
    /// <summary>
    /// Retrieves database connection settings from file-based configuration.
    /// </summary>
    /// <returns>Database connection settings from config files.</returns>
    /// <remarks>
    /// Uses the BuildConfiguration method from TestBase to load settings from:
    /// - appsettings.json
    /// - appsettings.Testing.json
    /// - User secrets
    /// </remarks>
    protected static DatabaseConnectionSettings GetFileBasedTestSettings()
    {
        var configuration = BuildConfiguration();
        
        try
        {
            var settings = new DatabaseConnectionSettings();
            configuration.GetSection("DatabaseConnection").Bind(settings);

            // Set credentials from configuration
            settings.UserID = configuration["Database:MY_SQL_USR"] ?? 
                throw new InvalidOperationException("Database username not found in configuration");
            settings.Password = configuration["Database:MY_SQL_PWD"] ?? 
                throw new InvalidOperationException("Database password not found in configuration");

            return settings;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Database credentials are missing. Ensure user secrets are configured. " +
                "See docs/user-secrets-setup.md for details.", ex);
        }
    }
    
    /// <summary>
    /// Creates a new instance of NorthwindContext configured for a real database connection.
    /// </summary>
    /// <returns>A NorthwindContext instance connected to a real database.</returns>
    /// <remarks>
    /// This method creates a context that connects to a real SQL Server database
    /// using connection settings from configuration files and user secrets.
    /// </remarks>
    protected static NorthwindContext CreateDatabaseContext()
    {
        var settings = GetFileBasedTestSettings();
        var builder = DatabaseConnectionBuilder.CreateBuilder(settings);
        
        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseSqlServer(builder.ConnectionString)
            .Options;
            
        return new NorthwindContext(options);
    }
} 