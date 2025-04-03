using Microsoft.Data.SqlClient; // To use SqlConnectionStringBuilder.
using Microsoft.EntityFrameworkCore; // To use UseSqlServer.
using Microsoft.Extensions.DependencyInjection; // To useIServiceCollection
using Microsoft.Extensions.Configuration; // To use IConfiguration
using Microsoft.Extensions.Options; // To use IOptions

namespace Northwind.EntityModels;
public static class NorthwindContextExtensions
{
    /// <summary>
    /// Adds NorthwindContext to the specified IServiceCollection.
    /// Uses the SqlServer database provider.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionString">Set to override the default.</param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    public static IServiceCollection AddNorthwindContext(
        this IServiceCollection services, // The type to extend.
    string? connectionString = null)
    {
        if (connectionString is null)
        {
            // Get configuration from the service provider
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            
            if (configuration == null)
            {
                throw new InvalidOperationException("Configuration service is not available");
            }
            
            // Get database settings directly from configuration
            var username = configuration["Database:MY_SQL_USR"];
            var password = configuration["Database:MY_SQL_PWD"];
            
            // Validate required settings
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Database credentials are missing in configuration");
            }

            SqlConnectionStringBuilder builder = new();
            builder.DataSource = "tcp:127.0.0.1,1433"; // SQL Edge in Docker.
            builder.InitialCatalog = "Northwind";
            builder.TrustServerCertificate = true;
            builder.MultipleActiveResultSets = true;

            // Because we want to fail faster. Default is 15 seconds.
            //Introducing Web Development Using Controllers
            builder.ConnectTimeout = 3;

            // SQL Server authentication.
            // Get credentials from configuration
            builder.UserID = username;
            builder.Password = password;
            connectionString = builder.ConnectionString;
        }

        services.AddDbContext<NorthwindContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.LogTo(
                NorthwindContextLogger.WriteLine,
                new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting }
            );
        },
        
        // Register with a transient lifetime to avoid concurrency
        // issues with Blazor Server projects.
        contextLifetime: ServiceLifetime.Transient,
        optionsLifetime: ServiceLifetime.Transient);
        return services;
    }
}