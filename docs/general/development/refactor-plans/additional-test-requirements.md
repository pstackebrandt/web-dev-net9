# Additional Test Requirements for AddNorthwindContext Refactoring

This document outlines the additional test requirements identified during the review of existing test coverage for the `AddNorthwindContext` extension method refactoring.

## Current Test Coverage

The current test coverage for `AddNorthwindContext` is primarily in `ConfigurationLoadingTests.cs` and includes:

1. **`NorthwindContextExtensions_UsesConnectionSettingsFromConfiguration`**
   - Verifies that configuration values are correctly passed to the database context
   - Tests that connection settings like server name, database name, timeout, and credentials are properly applied

2. **`Missing_Credentials_ThrowsInformativeException`**
   - Verifies appropriate error handling for missing credentials
   - Tests that an informative exception is thrown when database credentials are missing

## Test Gaps Identified

The following gaps were identified in the existing test coverage:

1. **No tests for the direct connection string parameter overload**
   - The extension method accepts an optional connection string parameter, but there are no dedicated tests for this path

2. **No tests for connection string building logic**
   - No tests specifically verifying the construction of the connection string from individual settings

3. **No tests for specific database provider options**
   - No verification of the SQL Server provider options being set correctly

4. **No tests for transient service registration**
   - No verification that the context is registered with the correct lifetime (transient)

## Additional Tests Required

### 1. New Method Overload Tests

These tests will focus on the new overload that accepts `IConfiguration` directly:

```csharp
[Fact]
public void AddNorthwindContext_WithIConfiguration_ConfiguresCorrectly()
{
    // Arrange
    var services = new ServiceCollection();
    var configValues = new Dictionary<string, string?>
    {
        { "DatabaseConnection:DataSource", "test-server" },
        { "DatabaseConnection:InitialCatalog", "TestDB" },
        { "DatabaseConnection:ConnectTimeout", "5" },
        { "Database:MY_SQL_USR", "testuser" },
        { "Database:MY_SQL_PWD", "testpass" }
    };

    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(configValues)
        .Build();

    // Act
    services.AddNorthwindContext(configuration);
    var serviceProvider = services.BuildServiceProvider();
    var context = serviceProvider.GetRequiredService<NorthwindContext>();

    // Assert
    var connectionString = context.Database.GetConnectionString();
    var builder = new SqlConnectionStringBuilder(connectionString);
    
    Assert.Equal("test-server", builder.DataSource);
    Assert.Equal("TestDB", builder.InitialCatalog);
    Assert.Equal(5, builder.ConnectTimeout);
    Assert.Equal("testuser", builder.UserID);
}
```

### 2. Equivalence Tests

Tests to verify both overloads produce identical results given the same input:

```csharp
[Fact]
public void BothOverloads_WithSameInput_ProduceIdenticalResults()
{
    // Arrange
    var configValues = new Dictionary<string, string?>
    {
        { "DatabaseConnection:DataSource", "test-server" },
        { "DatabaseConnection:InitialCatalog", "TestDB" },
        { "DatabaseConnection:ConnectTimeout", "5" },
        { "Database:MY_SQL_USR", "testuser" },
        { "Database:MY_SQL_PWD", "testpass" }
    };
    
    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(configValues)
        .Build();
    
    // Setup for original method
    var services1 = new ServiceCollection();
    services1.AddSingleton<IConfiguration>(configuration);
    services1.AddNorthwindContext();
    var serviceProvider1 = services1.BuildServiceProvider();
    var context1 = serviceProvider1.GetRequiredService<NorthwindContext>();
    
    // Setup for new method
    var services2 = new ServiceCollection();
    services2.AddNorthwindContext(configuration);
    var serviceProvider2 = services2.BuildServiceProvider();
    var context2 = serviceProvider2.GetRequiredService<NorthwindContext>();
    
    // Get and compare connection strings
    var connectionString1 = context1.Database.GetConnectionString();
    var connectionString2 = context2.Database.GetConnectionString();
    
    Assert.Equal(connectionString1, connectionString2);
}
```

### 3. Connection String Override Tests

Tests for explicit connection string override in both methods:

```csharp
[Fact]
public void AddNorthwindContext_WithDirectConnectionString_UsesProvidedString()
{
    // Arrange
    var services = new ServiceCollection();
    var connectionString = "Server=override-server;Database=OverrideDB;User ID=user;Password=pass;TrustServerCertificate=True";
    
    // Act
    services.AddNorthwindContext(connectionString);
    var serviceProvider = services.BuildServiceProvider();
    var context = serviceProvider.GetRequiredService<NorthwindContext>();
    
    // Assert
    var actualConnectionString = context.Database.GetConnectionString();
    var builder = new SqlConnectionStringBuilder(actualConnectionString);
    
    Assert.Equal("override-server", builder.DataSource);
    Assert.Equal("OverrideDB", builder.InitialCatalog);
}

[Fact]
public void AddNorthwindContext_WithIConfigAndConnectionString_PrioritizesConnectionString()
{
    // Arrange
    var services = new ServiceCollection();
    var configValues = new Dictionary<string, string?>
    {
        { "DatabaseConnection:DataSource", "config-server" },
        { "DatabaseConnection:InitialCatalog", "ConfigDB" }
    };
    
    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(configValues)
        .Build();
    
    var connectionString = "Server=override-server;Database=OverrideDB;User ID=user;Password=pass;TrustServerCertificate=True";
    
    // Act
    services.AddNorthwindContext(configuration, connectionString);
    var serviceProvider = services.BuildServiceProvider();
    var context = serviceProvider.GetRequiredService<NorthwindContext>();
    
    // Assert
    var actualConnectionString = context.Database.GetConnectionString();
    var builder = new SqlConnectionStringBuilder(actualConnectionString);
    
    Assert.Equal("override-server", builder.DataSource);
    Assert.Equal("OverrideDB", builder.InitialCatalog);
}
```

### 4. Service Lifetime Tests

Test to verify the context is registered with the correct lifetime:

```csharp
[Fact]
public void AddNorthwindContext_RegistersWithTransientLifetime()
{
    // Arrange
    var services = new ServiceCollection();
    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "DatabaseConnection:DataSource", "test-server" },
            { "Database:MY_SQL_USR", "user" },
            { "Database:MY_SQL_PWD", "pass" }
        })
        .Build();
    
    // Act
    services.AddNorthwindContext(configuration);
    
    // Assert
    var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(NorthwindContext));
    Assert.NotNull(descriptor);
    Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
}
```

### 5. Missing Optional Values Tests

Test handling of configurations with missing optional values:

```csharp
[Fact]
public void AddNorthwindContext_WithMissingOptionalValues_UsesDefaults()
{
    // Arrange
    var services = new ServiceCollection();
    var configValues = new Dictionary<string, string?>
    {
        // Only provide required values
        { "DatabaseConnection:DataSource", "test-server" },
        { "Database:MY_SQL_USR", "testuser" },
        { "Database:MY_SQL_PWD", "testpass" }
        // Missing InitialCatalog, ConnectTimeout, etc.
    };
    
    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(configValues)
        .Build();
    
    // Act
    services.AddNorthwindContext(configuration);
    var serviceProvider = services.BuildServiceProvider();
    var context = serviceProvider.GetRequiredService<NorthwindContext>();
    
    // Assert
    var connectionString = context.Database.GetConnectionString();
    var builder = new SqlConnectionStringBuilder(connectionString);
    
    // Should use the defaults from DatabaseConnectionSettings
    Assert.Equal("Northwind", builder.InitialCatalog);
    Assert.Equal(3, builder.ConnectTimeout); // Default is 3
    Assert.True(builder.TrustServerCertificate);
}
```

## Implementation Plan

1. Create a new test class `NorthwindContextExtensionsTests.cs` in the same namespace as the existing tests
2. Implement the tests outlined above
3. Run tests after each implementation step to ensure backward compatibility
4. Update existing tests to use the new overload once it's stable

## Expected Outcomes

- Comprehensive test coverage for both old and new method overloads
- Verification that both implementations produce identical results
- Confirmation of correct error handling for edge cases
- Validation of service registration with proper lifetime