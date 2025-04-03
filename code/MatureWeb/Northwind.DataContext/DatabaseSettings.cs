namespace Northwind.EntityModels;

/// <summary>
/// Strongly-typed configuration for database settings
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// SQL Server username
    /// </summary>
    public string MY_SQL_USR { get; set; } = string.Empty;
    
    /// <summary>
    /// SQL Server password
    /// </summary>
    public string MY_SQL_PWD { get; set; } = string.Empty;
} 