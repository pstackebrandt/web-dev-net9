# Test Configuration Guide

A guide to the configuration approaches used in Northwind test projects.

## Table of Contents

- [Test Configuration Guide](#test-configuration-guide)
  - [Table of Contents](#table-of-contents)
  - [Configuration Approaches Overview](#configuration-approaches-overview)
  - [In-Memory Configuration](#in-memory-configuration)
  - [File-Based Configuration](#file-based-configuration)
  - [Choosing the Right Approach](#choosing-the-right-approach)
  - [Specialized Test Base Classes](#specialized-test-base-classes)
  - [Troubleshooting Test Failures](#troubleshooting-test-failures)
    - [Database Connection Failures](#database-connection-failures)
    - [Configuration Loading Failures](#configuration-loading-failures)
    - [Test Execution Environment](#test-execution-environment)

## Configuration Approaches Overview

The Northwind test suite supports two configuration approaches:

1. **In-Memory Configuration**: Default approach that doesn't rely on external files
2. **File-Based Configuration**: Uses the same .NET layered configuration as the main application

This dual approach ensures tests can run in both isolated and real-world scenarios.

## In-Memory Configuration

In-memory configuration is the default approach and offers several advantages:

- **Independence**: Tests don't rely on configuration files
- **Isolation**: Each test runs with a predictable configuration
- **Simplicity**: No need to set up user secrets for basic tests
- **CI/CD friendly**: Works in automated build environments without special setup

This approach uses hardcoded values suitable for testing:

```csharp
var configValues = new Dictionary<string, string>
{
    { "DatabaseConnection:DataSource", "tcp:127.0.0.1,1433" },
    { "DatabaseConnection:InitialCatalog", "Northwind" },
    { "DatabaseConnection:ConnectTimeout", "1" }, // Testing timeout value
    { "Database:MY_SQL_USR", "sa" },
    { "Database:MY_SQL_PWD", "Password123!" } // Test password, not a real one
};
```

## File-Based Configuration

File-based configuration matches how the main application loads settings:

- **Real-world testing**: Tests with actual database connections
- **Configuration verification**: Tests that verify configuration loading
- **User secrets integration**: Securely accesses credentials
- **Layered approach**: Base settings + environment-specific overrides

This approach uses:

- `appsettings.json`: Base configuration
- `appsettings.Testing.json`: Test-specific overrides
- `User Secrets`: Credentials for database access

## Choosing the Right Approach

| Use In-Memory Configuration when...   | Use File-Based Configuration when...        |
| ------------------------------------- | ------------------------------------------- |
| You need isolation from environment   | You need to test with a real database       |
| Running in CI/CD pipelines            | Testing configuration loading mechanisms    |
| Writing unit tests for business logic | Testing database connections                |
| You want faster test execution        | Testing with actual credentials             |
| Testing error handling                | Tests depend on environment-specific values |

## Specialized Test Base Classes

The test project provides specialized base classes for each approach:

- **TestBase**: Base class with default in-memory configuration
  - Use for most unit tests that don't require a real database

- **DatabaseTestBase**: Uses file-based configuration for database tests
  - Inherits from TestBase
  - Override of GetTestSettings() to use file-based configuration
  - Use for tests that need to connect to a real database

- **ConfigurationFileTestBase**: Utilities for testing configuration files
  - Inherits from TestBase
  - Additional methods for configuration file testing
  - Use for tests that verify configuration loading

Example usage:

```csharp
// Tests that need real database
public class EntityModelTests : DatabaseTestBase
{
    [Fact]
    public void DatabaseConnectTest()
    {
        using NorthwindContext db = CreateTestContext();
        Assert.True(db.Database.CanConnect());
    }
}

// Tests that verify configuration
public class ConfigTests : ConfigurationFileTestBase
{
    [Fact]
    public void FileBasedConfiguration_LoadsCorrectly()
    {
        var settings = GetFileBasedTestSettings();
        Assert.Equal("Northwind", settings.InitialCatalog);
    }
}
```

## Troubleshooting Test Failures

Common issues and solutions:

### Database Connection Failures

- **Docker not running**: Start Docker Desktop
- **SQL Server container not running**: Run `docker start sql`
- **Wrong credentials**: Check user secrets are set up correctly
- **Connection timeout**: May indicate server is unreachable
- **Authentication failure**: Verify credentials in user secrets

### Configuration Loading Failures

- **Missing files**: Ensure appsettings.json/appsettings.Testing.json exist
- **Format errors**: Check JSON syntax in configuration files
- **Path issues**: Verify FindSolutionRoot() is working correctly
- **User secrets**: Check if required user secrets are set

### Test Execution Environment

- **Wrong environment**: Set ASPNETCORE_ENVIRONMENT to "Testing"
- **Base class issues**: Ensure you're using the right test base class
- **Missing resources**: Check if test requires specific external resources

When a test fails with a message about missing credentials, run:

```bash
dotnet user-secrets set "Database:MY_SQL_USR" "your_test_username" --project Northwind.UnitTests
dotnet user-secrets set "Database:MY_SQL_PWD" "your_test_password" --project Norhwind.UnitTests
```

See [User Secrets Setup Guide](user-secrets-setup.md) for detailed instructions. 