using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Northwind.UnitTests.Infrastructure;

/// <summary>
/// Base class for tests that verify configuration loading mechanisms.
/// </summary>
/// <remarks>
/// This class provides utilities for testing configuration file loading,
/// layered configuration, and configuration binding.
/// Use this class for tests that:
/// - Validate configuration file parsing
/// - Test configuration section binding
/// - Verify environment-specific configuration overrides
/// </remarks>
public abstract class ConfigurationFileTestBase : TestBase
{
    /// <summary>
    /// Creates a configuration builder for testing file-based configuration.
    /// </summary>
    /// <returns>A pre-configured ConfigurationBuilder.</returns>
    /// <remarks>
    /// The builder is set up to load from both base and testing configuration files.
    /// </remarks>
    protected static IConfigurationBuilder CreateConfigurationBuilder()
    {
        string solutionRoot = FindSolutionRoot();
        
        return new ConfigurationBuilder()
            .SetBasePath(solutionRoot)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Testing.json", optional: false);
    }
    
    /// <summary>
    /// Creates a configuration object from the test configuration files.
    /// </summary>
    /// <returns>The built configuration object.</returns>
    protected static IConfiguration GetTestConfiguration()
    {
        return CreateConfigurationBuilder().Build();
    }
    
    /// <summary>
    /// Gets the path to a test configuration file.
    /// </summary>
    /// <param name="fileName">The name of the configuration file.</param>
    /// <returns>The full path to the configuration file.</returns>
    protected static string GetConfigurationFilePath(string fileName)
    {
        return Path.Combine(FindSolutionRoot(), fileName);
    }
} 