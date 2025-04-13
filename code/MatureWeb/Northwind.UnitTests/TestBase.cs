using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;

namespace Northwind.UnitTests;

/// <summary>
/// Base class for Northwind database tests providing common functionality
/// for database context creation and configuration.
/// </summary>
/// <remarks>
/// This class:
/// - Provides centralized test database configuration
/// - Handles test-specific connection settings
/// - Ensures consistent database context setup across all tests
/// - Supports both in-memory and file-based configuration
/// </remarks>
public abstract class TestBase
{
    /// <summary>
    /// Retrieves database connection settings from test configuration.
    /// </summary>
    /// <returns>Database connection settings configured for testing.</returns>
    /// <remarks>
    /// This method defaults to in-memory configuration for most tests.
    /// Override in derived classes to use file-based configuration when needed.
    /// </remarks>
    protected static DatabaseConnectionSettings GetTestSettings()
    {
        return GetInMemoryTestSettings();
    }

    /// <summary>
    /// Retrieves database connection settings from in-memory test configuration.
    /// </summary>
    /// <returns>Database connection settings configured for testing.</returns>
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
    /// Retrieves database connection settings from file-based configuration.
    /// </summary>
    /// <returns>Database connection settings from config files.</returns>
    /// <remarks>
    /// Uses standard .NET layered configuration approach with:
    /// - Base appsettings.json
    /// - Environment-specific settings
    /// - User secrets for sensitive data
    /// </remarks>
    protected static DatabaseConnectionSettings GetFileBasedTestSettings()
    {
        string solutionRoot = FindSolutionRoot();
        
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(solutionRoot)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Testing.json", optional: false)
                .AddUserSecrets<NorthwindContext>(optional: true)
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
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Database credentials are missing. If in development, ensure user secrets are configured. " +
                "See docs/user-secrets-setup.md for details.", ex);
        }
    }

    /// <summary>
    /// Finds the solution root directory by searching for a solution file.
    /// </summary>
    /// <returns>Path to the solution root directory.</returns>
    /// <remarks>
    /// Walks up from current directory until it finds a solution file or reaches 
    /// the drive root. Looks for Northwind.sln or any .sln file as fallback.
    /// </remarks>
    protected static string FindSolutionRoot()
    {
        // Start with the directory of the executing assembly
        string? currentDir = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location);
        
        if (currentDir == null)
        {
            throw new InvalidOperationException(
                "Could not determine the current directory.");
        }

        // Walk up directories looking for a solution file
        while (currentDir != null && Directory.Exists(currentDir))
        {
            // First look for Northwind.sln specifically
            if (File.Exists(Path.Combine(currentDir, "Northwind.sln")))
            {
                return currentDir;
            }

            // Then check for any .sln file as a fallback
            if (Directory.GetFiles(currentDir, "*.sln").Length > 0)
            {
                return currentDir;
            }

            // Move up to parent directory
            DirectoryInfo? parentDir = Directory.GetParent(currentDir);
            if (parentDir == null)
            {
                break; // We've reached the root
            }
            
            currentDir = parentDir.FullName;
        }

        throw new InvalidOperationException(
            "Could not find solution root. Solution file (.sln) not found in any parent directory.");
    }

    /// <summary>
    /// Creates a new instance of NorthwindContext configured for testing.
    /// </summary>
    /// <returns>A NorthwindContext instance ready for testing.</returns>
    /// <remarks>
    /// This method:
    /// - Gets test-specific database settings
    /// - Builds connection string using shared configuration
    /// - Creates context with test configuration
    /// 
    /// Use this method in tests that need database access.
    /// </remarks>
    protected static NorthwindContext CreateTestContext()
    {
        var settings = GetTestSettings();
        var builder = DatabaseConnectionBuilder.CreateBuilder(settings);

        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseSqlServer(builder.ConnectionString)
            .Options;

        return new NorthwindContext(options);
    }
}