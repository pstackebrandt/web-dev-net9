# Configuration Restoration Plan

This document outlines the tasks required to restore file-based configuration for tests while maintaining in-memory configuration where appropriate.

**STATUS: COMPLETED** - All planned tasks have been successfully implemented and verified.

## Table of Contents

- [Configuration Restoration Plan](#configuration-restoration-plan)
  - [Table of Contents](#table-of-contents)
  - [Core Implementation](#core-implementation)
  - [Example Files](#example-files)
  - [Package References](#package-references)
  - [Test Base Classes](#test-base-classes)
  - [Test Class Updates](#test-class-updates)
  - [Test Method Enhancements](#test-method-enhancements)
  - [Test Documentation](#test-documentation)
  - [Implementation Notes](#implementation-notes)

## Core Implementation

- [x] Update `TestBase.cs` to support dual configuration methods:
  - [x] Create `GetInMemoryTestSettings()` method based on current implementation
  - [x] Create `GetFileBasedTestSettings()` method with standard .NET layered configuration:
    ```csharp
    var configuration = new ConfigurationBuilder()
        .SetBasePath(solutionRoot)
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile("appsettings.Testing.json", optional: false)
        .AddUserSecrets<NorthwindContext>(optional: true)
        .Build();
    ```
  - [x] Implement error messages that match existing patterns:
    ```csharp
    throw new InvalidOperationException(
        "Database credentials are missing. If in development, ensure user secrets are configured. " +
        "See docs/user-secrets-setup.md for details.", ex);
    ```
  - [x] Update `GetTestSettings()` to default to in-memory configuration
  - [x] Ensure `FindSolutionRoot()` method correctly locates the solution root

## Example Files

- [x] Update test example files to match the production files:
  - [x] Ensure `appsettings.Testing.Example.json` only contains override values
  - [x] Update examples to include proper placeholder values for credentials
  - [x] Ensure example files demonstrate the layered configuration pattern

## Package References

- [x] Verify necessary NuGet packages in test project:
  - [x] Check for `Microsoft.Extensions.Configuration.Json` for file loading
  - [x] Check for `Microsoft.Extensions.Configuration.UserSecrets` for secrets integration
  - [x] Check for `Microsoft.Extensions.Configuration.Binder` for binding to typed objects
  - [x] Add any missing packages to match the main project

## Test Base Classes

- [x] Create specialized base classes for different testing scenarios:
  - [x] Create `DatabaseTestBase` class for tests that need a real database
    ```csharp
    public abstract class DatabaseTestBase : TestBase
    {
        protected static new DatabaseConnectionSettings GetTestSettings()
        {
            return GetFileBasedTestSettings();
        }
    }
    ```
  - [x] Create `ConfigurationFileTestBase` class for tests that verify configuration loading

## Test Class Updates

- [x] Update test classes to use appropriate base classes:
  - [x] Update `EntityModelTests` to inherit from `DatabaseTestBase`
  - [x] Update `DockerDatabaseTests` to inherit from `DatabaseTestBase`
  - [x] Update `TestDbConfigurationTests` to inherit from `ConfigurationFileTestBase`
  - [x] Leave `ConfigurationLoadingTests` inheriting from base `TestBase`

## Test Method Enhancements

- [x] Create dedicated tests for both configuration approaches:
  - [x] Add test to verify file-based configuration loads correctly
    ```csharp
    [Fact]
    public void FileBasedConfiguration_LoadsCorrectly()
    {
        // Test implementation
    }
    ```
  - [x] Add test to verify in-memory configuration works correctly
    ```csharp
    [Fact]
    public void InMemoryConfiguration_WorksCorrectly()
    {
        // Test implementation
    }
    ```
  - [x] Update existing test methods to correctly verify configuration loading:
    - [x] Modify `TestCredentials_AreLoadedCorrectly` to test layered configuration
    - [x] Ensure assertions verify both base settings and overrides
    - [x] Match error message patterns with main application

## Test Documentation

- [x] Create documentation for running tests:
  - [x] Document how to configure user secrets for tests
  - [x] Explain the separation between file-based and in-memory tests
  - [x] Provide instructions for troubleshooting test failures
  - [x] Update [User Secrets Setup Guide](docs/user-secrets-setup.md) to mention test requirements

## Implementation Notes

- Tests that require file-based configuration will explicitly fail with clear error messages if files are missing or misconfigured
- We're using the standard .NET approach with base + environment-specific settings to match the main application pattern
- Error messages should be consistent with those in `NorthwindContext.cs` and `NorthwindContextExtensions.cs`
- User secrets are required for tests that need database access
- Skip attributes for tests that require actual database connections can be added in the future if needed