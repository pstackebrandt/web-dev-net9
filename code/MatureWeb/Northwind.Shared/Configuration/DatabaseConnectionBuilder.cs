using Microsoft.Data.SqlClient;

namespace Northwind.Shared.Configuration;

/// <summary>
/// Utility class for building SQL Server connection strings from configuration settings.
/// Follows the configuration best practices by separating connection parameters from credentials.
/// </summary>
public class DatabaseConnectionBuilder
{
    /// <summary>
    /// Creates a SQL connection string builder with properties initialized from configuration settings.
    /// </summary>
    /// <param name="settings">The database connection settings containing connection parameters and credentials.</param>
    /// <returns>A configured SqlConnectionStringBuilder instance ready to produce a connection string.</returns>
    /// <exception cref="InvalidOperationException">Thrown when database credentials are missing or empty.</exception>
    public static SqlConnectionStringBuilder CreateBuilder(DatabaseConnectionSettings settings)
    {
        // Validate that required credentials are provided
        if (string.IsNullOrEmpty(settings.UserID) || string.IsNullOrEmpty(settings.Password))
        {
            throw new InvalidOperationException("Database credentials are missing");
        }

        // Create and configure the connection string builder with all settings
        return new SqlConnectionStringBuilder
        {
            DataSource = settings.DataSource,
            InitialCatalog = settings.InitialCatalog,
            TrustServerCertificate = settings.TrustServerCertificate,
            MultipleActiveResultSets = settings.MultipleActiveResultSets,
            ConnectTimeout = settings.ConnectTimeout,
            UserID = settings.UserID,
            Password = settings.Password
        };
    }
} 