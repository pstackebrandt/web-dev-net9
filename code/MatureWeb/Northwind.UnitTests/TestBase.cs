using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using Northwind.EntityModels;

namespace Northwind.UnitTests;

/// <summary>
/// Abstract base class for Northwind tests providing common functionality
/// for test configuration.
/// </summary>
/// <remarks>
/// This class:
/// - Provides centralized test configuration building
/// - Contains only functionality common to all test types
/// - Serves as a foundation for specialized test base classes
/// </remarks>
public abstract class TestBase
{
    /// <summary>
    /// Builds a configuration object from standard configuration sources.
    /// </summary>
    /// <returns>A built IConfigurationRoot object.</returns>
    /// <remarks>
    /// Uses standard .NET layered configuration approach with:
    /// - Base appsettings.json
    /// - Environment-specific settings (appsettings.Testing.json)
    /// - User secrets for sensitive data
    /// </remarks>
    protected static IConfigurationRoot BuildConfiguration()
    {
        string solutionRoot = FindSolutionRoot();
        
        try
        {
            return new ConfigurationBuilder()
                .SetBasePath(solutionRoot)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Testing.json", optional: false)
                .AddUserSecrets<NorthwindContext>(optional: true)
                .Build();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Configuration could not be loaded. " +
                "See docs/test-configuration-guide.md for details.", ex);
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
        // TODO: Do we need this function in TestBase.cs. It looks like it may be useful in different cases and would better be placed in a more general utility class.

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
}