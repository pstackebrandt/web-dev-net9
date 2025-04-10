using Microsoft.Data.SqlClient;

namespace Northwind.Shared.Configuration;

public class DatabaseConnectionBuilder
{
    public static SqlConnectionStringBuilder CreateBuilder(DatabaseConnectionSettings settings)
    {
        if (string.IsNullOrEmpty(settings.UserID) || string.IsNullOrEmpty(settings.Password))
        {
            throw new InvalidOperationException("Database credentials are missing");
        }

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