using Northwind.Shared.Configuration;

namespace Northwind.UnitTests.Infrastructure;

/// <summary>
/// Base class for database tests that require a real database connection.
/// </summary>
/// <remarks>
/// This class overrides the default in-memory configuration to use file-based
/// configuration with user secrets for credentials.
/// Use this base class for tests that:
/// - Verify actual database connectivity
/// - Validate entity mapping against real database schema
/// - Test database-specific functionality
/// </remarks>
public abstract class DatabaseTestBase : TestBase
{
    /// <summary>
    /// Retrieves database connection settings from file-based configuration.
    /// </summary>
    /// <returns>Database connection settings from config files.</returns>
    /// <remarks>
    /// This override ensures database tests use real credentials from
    /// appsettings.json, appsettings.Testing.json and user secrets.
    /// </remarks>
    protected static new DatabaseConnectionSettings GetTestSettings()
    {
        return GetFileBasedTestSettings();
    }
} 