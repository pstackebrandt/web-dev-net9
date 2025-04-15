# Test Configuration Guide

A guide to the configuration approaches used in Northwind test projects.

## Table of Contents

- [Test Configuration Guide](#test-configuration-guide)
  - [Table of Contents](#table-of-contents)
  - [Overview of Test Base Classes](#overview-of-test-base-classes)
  - [TestBase (Abstract)](#testbase-abstract)
  - [InMemoryTestBase](#inmemorytestbase)
  - [DatabaseTestBase](#databasetestbase)
  - [Choosing the Right Base Class](#choosing-the-right-base-class)
  - [Example Usage](#example-usage)
  - [Troubleshooting Test Failures](#troubleshooting-test-failures)
    - [Database Connection Failures](#database-connection-failures)
    - [Configuration Loading Failures](#configuration-loading-failures)
    - [Test Execution Environment](#test-execution-environment)

## Overview of Test Base Classes

The Northwind test project uses a hierarchy of base classes to provide common functionality and configuration for different types of tests. This structure promotes code reuse and ensures consistency.

- **TestBase**: An abstract base class providing core utilities.
- **InMemoryTestBase**: Derived from TestBase, for tests using an in-memory database.
- **DatabaseTestBase**: Derived from TestBase, for tests requiring a real database connection.

## TestBase (Abstract)

`TestBase` serves as the foundation for other test base classes. It is declared `abstract` and cannot be used directly.

- **Purpose**: Provides common, shared utilities needed by different test types.
- **Key Functionality**:
  - `BuildConfiguration()`: A static method that builds an `IConfigurationRoot` object by loading settings from `appsettings.json`, `appsettings.Testing.json`, and user secrets. This ensures consistent configuration loading.
  - `FindSolutionRoot()`: A utility to locate the solution root directory, necessary for finding configuration files.
- **Inheritance**: Both `InMemoryTestBase` and `DatabaseTestBase` inherit from `TestBase`.

## InMemoryTestBase

`InMemoryTestBase` is designed for tests that need an isolated, in-memory database environment.

- **Purpose**: To facilitate unit tests that should run quickly and independently of external database resources.
- **Inheritance**: Inherits from `TestBase`.
- **Key Functionality**:
  - `GetInMemoryTestSettings()`: Returns a `DatabaseConnectionSettings` object populated with hardcoded values suitable for basic testing, independent of configuration files.
  - `CreateInMemoryContext()`: Creates a `NorthwindContext` instance configured to use the EF Core in-memory database provider. Each call generates a unique database name (e.g., `NorthwindTest_<GUID>`) to ensure test isolation.
- **Dependencies**: Requires the `Microsoft.EntityFrameworkCore.InMemory` NuGet package.
- **Use Cases**: Ideal for testing business logic, validation rules, or components that interact with the `DbContext` without needing actual database persistence or features.

## DatabaseTestBase

`DatabaseTestBase` is used for tests that need to interact with a real, configured database (typically SQL Server running in Docker for this project).

- **Purpose**: To enable integration tests that verify database connectivity, entity mappings, and SQL-specific behavior.
- **Inheritance**: Inherits from `TestBase`.
- **Key Functionality**:
  - `GetFileBasedTestSettings()`: Retrieves `DatabaseConnectionSettings` by calling `BuildConfiguration()` from `TestBase` and extracting necessary values (connection string components, user ID, password) from the loaded configuration.
  - `CreateDatabaseContext()`: Creates a `NorthwindContext` instance configured to connect to the database specified in the configuration files and user secrets (using `UseSqlServer`).
- **Dependencies**: Relies on correctly configured `appsettings.Testing.json` and user secrets for database credentials. Requires the database server (e.g., SQL Server Docker container) to be running and accessible.
- **Use Cases**: Essential for testing database migrations, repository logic involving complex queries, end-to-end scenarios, and verifying interactions with the actual database schema.

## Choosing the Right Base Class

| Inherit from `InMemoryTestBase` when... | Inherit from `DatabaseTestBase` when...       |
| --------------------------------------- | --------------------------------------------- |
| Testing logic independent of database   | Testing actual database connectivity          |
| Needing fast, isolated tests            | Testing entity mappings against schema        |
| Running tests in CI/CD easily           | Testing repository logic / complex queries    |
| Unit testing services or controllers    | Testing database-specific features/functions  |
| Avoiding external dependencies          | Need data persistence between test operations |

## Example Usage

```csharp
// Example: Test using in-memory database
public class ProductServiceTests : InMemoryTestBase
{
    [Fact]
    public void AddProduct_WithValidData_Succeeds()
    {
        using var context = CreateInMemoryContext();
        var service = new ProductService(context);
        
        // Act: Add a product
        service.AddProduct(/* ... */);
        
        // Assert: Verify product was added in memory
        Assert.Equal(1, context.Products.Count());
    }
}

// Example: Test using real database
public class EntityModelTests : DatabaseTestBase
{
    [Fact]
    public void DatabaseConnectTest()
    {
        using NorthwindContext db = CreateDatabaseContext();
        Assert.True(db.Database.CanConnect());
    }
}
```

## Troubleshooting Test Failures

Common issues and solutions:

### Database Connection Failures

- **Docker not running**: Start Docker Desktop.
- **SQL Server container not running**: Run `docker start sql`.
- **Wrong credentials**: Check user secrets are set up correctly ([User Secrets Setup Guide](user-secrets-setup.md)).
- **Connection timeout**: May indicate server is unreachable or firewall issue.
- **Authentication failure**: Verify credentials in user secrets match the database setup.

### Configuration Loading Failures

- **Missing files**: Ensure `appsettings.json` & `appsettings.Testing.json` exist at the solution root.
- **Format errors**: Check JSON syntax in configuration files.
- **User secrets**: Check if required user secrets (`Database:MY_SQL_USR`, `Database:MY_SQL_PWD`) are set for the `Northwind.UnitTests` project.

### Test Execution Environment

- **Wrong environment**: While less critical now for base class selection, ensure environment variables aren't causing unexpected configuration issues.
- **Missing packages**: Ensure `Microsoft.EntityFrameworkCore.InMemory` is installed for `InMemoryTestBase` tests and `Microsoft.EntityFrameworkCore.SqlServer` for `DatabaseTestBase` tests.

When a test fails with a message about missing credentials, run:

```bash
dotnet user-secrets set "Database:MY_SQL_USR" "your_test_username" --project Northwind.UnitTests
dotnet user-secrets set "Database:MY_SQL_PWD" "your_test_password" --project Northwind.UnitTests
```

See [User Secrets Setup Guide](user-secrets-setup.md) for detailed instructions.