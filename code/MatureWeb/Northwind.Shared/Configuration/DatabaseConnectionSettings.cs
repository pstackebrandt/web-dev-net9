namespace Northwind.Shared.Configuration;

/// <summary>
/// Represents the configuration settings for SQL Server database connections.
/// Used both in production and test environments.
/// </summary>
/// <remarks>
/// This class provides a strongly-typed configuration for database connections.
/// Default values are set for common scenarios but can be overridden through configuration.
/// Used by DatabaseConnectionBuilder to create connection strings.
/// </remarks>
public class DatabaseConnectionSettings
{
    /// <summary>
    /// Gets or sets the SQL Server instance address.
    /// Default is localhost SQL Edge in Docker.
    /// </summary>
    public string DataSource { get; set; } = "tcp:127.0.0.1,1433";

    /// <summary>
    /// Gets or sets the target database name.
    /// Default is the main Northwind database.
    /// </summary>
    public string InitialCatalog { get; set; } = "Northwind";

    /// <summary>
    /// Gets or sets whether to trust the SQL Server certificate.
    /// Default is true for development environments.
    /// </summary>
    public bool TrustServerCertificate { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the connection supports multiple active result sets.
    /// Default is true to support concurrent queries.
    /// </summary>
    public bool MultipleActiveResultSets { get; set; } = true;

    /// <summary>
    /// Gets or sets the connection timeout in seconds.
    /// Default is 3 seconds for faster failure detection.
    /// </summary>
    public int ConnectTimeout { get; set; } = 3;

    /// <summary>
    /// Gets or sets the SQL Server authentication username.
    /// Must be provided through configuration.
    /// </summary>
    public string UserID { get; set; } = null!;

    /// <summary>
    /// Gets or sets the SQL Server authentication password.
    /// Must be provided through configuration.
    /// </summary>
    public string Password { get; set; } = null!;
} 