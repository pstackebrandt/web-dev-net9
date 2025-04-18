using Microsoft.Data.SqlClient; // To use SqlConnectionStringBuilder.
using Microsoft.EntityFrameworkCore; // To use UseSqlServer.
using Microsoft.Extensions.DependencyInjection; // To useIServiceCollection
using Microsoft.Extensions.Configuration; // To use IConfiguration
using Microsoft.Extensions.Options; // To use IOptions
using Northwind.Shared.Configuration; // For DatabaseConnectionSettings and DatabaseConnectionBuilder
using System; // For Obsolete attribute

namespace Northwind.EntityModels;

public static class NorthwindContextExtensions
{
    /// <summary>
    /// Adds NorthwindContext to the specified IServiceCollection.
    /// Uses the SqlServer database provider.
    /// </summary>
    /// <remarks>
    /// This is the preferred way to configure NorthwindContext because:
    /// - It integrates with ASP.NET Core's dependency injection
    /// - Can access all configured services and configuration providers
    /// - Supports multiple configuration sources (not just appsettings.json)
    /// - Proper integration with ASP.NET Core's configuration system
    /// 
    /// Note: This coexists with OnConfiguring in NorthwindContext.cs which serves
    /// as a fallback for standalone scenarios where DI is not available.
    /// </remarks>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration to use for database settings.</param>
    /// <param name="connectionString">Set to override the default.</param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    public static IServiceCollection AddNorthwindContext(
        this IServiceCollection services,
        IConfiguration configuration,
        string? connectionString = null)
    {
        if (connectionString is null)
        {
            // Create connection settings from configuration
            var connectionSettings = new DatabaseConnectionSettings();
            configuration.GetSection("DatabaseConnection").Bind(connectionSettings);
            
            try
            {
                // Get credentials from configuration (which includes user secrets in dev)
                connectionSettings.UserID = configuration["Database:MY_SQL_USR"] ?? 
                    throw new InvalidOperationException("Database username not found in configuration");
                connectionSettings.Password = configuration["Database:MY_SQL_PWD"] ?? 
                    throw new InvalidOperationException("Database password not found in configuration");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(
                    "Database credentials are missing. If in development, ensure user secrets are configured. " +
                    "See docs/user-secrets-setup.md for details.", ex);
            }

            // Build connection string
            var builder = DatabaseConnectionBuilder.CreateBuilder(connectionSettings);
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

    /// <summary>
    /// Adds NorthwindContext to the specified IServiceCollection.
    /// Uses the SqlServer database provider.
    /// </summary>
    /// <remarks>
    /// This is the preferred way to configure NorthwindContext because:
    /// - It integrates with ASP.NET Core's dependency injection
    /// - Can access all configured services and configuration providers
    /// - Supports multiple configuration sources (not just appsettings.json)
    /// - Proper integration with ASP.NET Core's configuration system
    /// 
    /// Note: This coexists with OnConfiguring in NorthwindContext.cs which serves
    /// as a fallback for standalone scenarios where DI is not available.
    /// </remarks>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionString">Set to override the default.</param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    [Obsolete("This method builds a service provider during configuration, which is an anti-pattern. Use the overload that accepts IConfiguration instead: AddNorthwindContext(IServiceCollection, IConfiguration, string?).")]
    public static IServiceCollection AddNorthwindContext(
        this IServiceCollection services, // The type to extend.
        string? connectionString = null)
    {
        // This is a specific implementation by Peter an AI.
        // It's different from the book at p. 38, which hardcoded many values.

        if (connectionString is null)
        {
            // Get configuration from the service provider
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            if (configuration == null)
            {
                throw new InvalidOperationException("Configuration service is not available");
            }

            // Use the new overload that accepts IConfiguration directly
            return AddNorthwindContext(services, configuration, connectionString);
        }

        // If connection string is provided, use it directly with the new overload
        // Pass null for configuration since we're not using it
        // Build a minimal configuration to avoid null checks
        var emptyConfiguration = new ConfigurationBuilder().Build();
        return AddNorthwindContext(services, emptyConfiguration, connectionString);
    }
}