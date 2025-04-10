using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
/// - Ensures consistent database context setup across all testsF
/// - Loads configuration from solution root directory
/// </remarks>
public abstract class TestBase
{
    /// <summary>
    /// Retrieves database connection settings from the test configuration file.
    /// </summary>
    /// <returns>Database connection settings configured for testing.</returns>
    /// <remarks>
    /// This method:
    /// - Locates the solution root by finding MatureWeb.sln
    /// - Loads appsettings.Testing.json from solution root
    /// - Maps configuration to strongly-typed settings
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the solution root directory cannot be found.
    /// </exception>
    protected static DatabaseConnectionSettings GetTestSettings()
    {
        // Get the solution root directory (parent of the test project)
        var solutionRoot = Directory.GetCurrentDirectory();
        while (solutionRoot != null && !File.Exists(Path.Combine(solutionRoot, "MatureWeb.sln")))
        {
            solutionRoot = Directory.GetParent(solutionRoot)?.FullName;
        }

        if (solutionRoot == null)
        {
            throw new InvalidOperationException("Could not find solution root directory");
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(solutionRoot)
            .AddJsonFile("appsettings.Testing.json")
            .Build();

        var settings = new DatabaseConnectionSettings();
        configuration.GetSection("DatabaseConnection").Bind(settings);

        settings.UserID = configuration["Database:MY_SQL_USR"] ??
            throw new InvalidOperationException("Database username not found in configuration");
        settings.Password = configuration["Database:MY_SQL_PWD"] ??
            throw new InvalidOperationException("Database password not found in configuration");

        return settings;
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