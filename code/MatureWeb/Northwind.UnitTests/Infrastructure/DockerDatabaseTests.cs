using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;
using Xunit.Sdk;
using Xunit;

namespace Northwind.UnitTests.Infrastructure;

/// <summary>
/// Tests to verify Docker and SQL Server container prerequisites.
/// These tests help diagnose common infrastructure issues before
/// running actual database tests.
/// </summary>
public class DockerDatabaseTests : DatabaseTestBase
{
    [Fact]
    public void DockerDesktopIsRunning()
    {
        // Check if Docker process is running
        bool isDockerRunning = IsDockerProcessRunning();
        
        // Use standard assert with descriptive message
        Assert.True(isDockerRunning, "Docker Desktop is not running. Please start Docker Desktop.");
    }
    
    [Fact]
    public void SqlServerContainerIsRunning()
    {
        // First check if Docker is running and report as part of this test
        bool isDockerRunning = IsDockerProcessRunning();
        
        // If Docker isn't running, this test should fail with a clear message
        if (!isDockerRunning)
        {
            Assert.True(isDockerRunning, "Docker Desktop is not running. SQL Server container check cannot proceed.");
            return; // No need to continue
        }
        
        // If we get here, Docker is running, so now check the container
        bool isContainerRunning = IsSqlContainerRunning();
        
        // Use standard assert with descriptive message
        Assert.True(isContainerRunning, "SQL Server container is not running. Please start the container.");
    }
    
    /// <summary>
    /// Tests if a database connection can be established.
    /// </summary>
    /// <remarks>
    /// Verifies basic connectivity without detailed diagnostics.
    /// </remarks>
    [Fact]
    public void CanConnectToDatabase()
    {
        using var context = CreateTestContext();
        bool canConnect = context.Database.CanConnect();
        Assert.True(canConnect, "Database connection failed");
    }
    
    /// <summary>
    /// Attempts to open a database connection and reports native error messages.
    /// </summary>
    /// <remarks>
    /// Provides detailed SQL error information when connection fails.
    /// Uses file-based configuration with user secrets.
    /// </remarks>
    [Fact]
    public void OpenDatabaseConnection()
    {
        // Explicitly use file-based settings to ensure user secrets are used
        var settings = GetFileBasedTestSettings();
        var builder = DatabaseConnectionBuilder.CreateBuilder(settings);
        
        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseSqlServer(builder.ConnectionString)
            .Options;
            
        using var context = new NorthwindContext(options);
        
        // Get connection string before attempting connection
        var connectionString = context.Database.GetConnectionString();
        // Create a copy that masks the password for safe display
        var displayConnectionString = new SqlConnectionStringBuilder(connectionString)
        {
            //Password = "********" // Mask the password
        }.ToString();
        
        try
        {
            context.Database.OpenConnection();
            context.Database.CloseConnection();
            Assert.True(true, "Database connection succeeded");
        }
        catch (SqlException ex)
        {
            throw new XunitException(
                $"Database connection failed: {ex.Message}\n" +
                $"Error Number: {ex.Number}\n" +
                $"Connection: {displayConnectionString}\n" + 
                $"State: {ex.State}");
        }
    }
    
    private bool IsDockerProcessRunning()
    {
        try
        {
            // Try to run docker info command
            var startInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "info",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            using var process = Process.Start(startInfo);
            process?.WaitForExit(3000); // Wait 3 seconds max
            
            return process?.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
    
    private bool IsSqlContainerRunning()
    {
        try
        {
            // Try to get SQL container status 
            var startInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "ps --filter \"name=sql\" --format \"{{.Status}}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            using var process = Process.Start(startInfo);
            if (process != null)
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit(3000);
                
                // If we get a line that starts with "Up", container is running
                return output.Trim().StartsWith("Up");
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
}