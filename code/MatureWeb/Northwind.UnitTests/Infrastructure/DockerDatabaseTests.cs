using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;
using Xunit;

namespace Northwind.UnitTests.Infrastructure;

/// <summary>
/// Tests to verify Docker and SQL Server container prerequisites.
/// These tests help diagnose common infrastructure issues before
/// running actual database tests.
/// </summary>
public class DockerDatabaseTests : TestBase
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
    
    [Fact]
    public void DatabaseConnectionFailureReason()
    {
        using var context = CreateTestContext();
        
        try
        {
            bool canConnect = context.Database.CanConnect();
            Assert.True(canConnect, "Database connection succeeded");
        }
        catch (SqlException ex)
        {
            // First check Docker status
            if (!IsDockerProcessRunning())
            {
                throw new XunitException($"Connection failed because Docker is not running. Error: {ex.Message}");
            }
            
            // Then check container status
            if (!IsSqlContainerRunning())
            {
                throw new XunitException($"Connection failed because SQL container is not running. Error: {ex.Message}");
            }
            
            // If both are running, it's another issue
            throw new XunitException($"Connection failed for another reason: {ex.Message}");
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