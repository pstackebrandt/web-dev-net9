# Test Baseline Report

This document establishes the baseline test status before refactoring the test infrastructure.
Last updated: After `dotnet test` run (Docker started, SQL container not running).

## Table of Contents

- [Test Baseline Report](#test-baseline-report)
  - [Table of Contents](#table-of-contents)
  - [Infrastructure Tests](#infrastructure-tests)
    - [DockerDatabaseTests](#dockerdatabasetests)
    - [ConfigurationLoadingTests](#configurationloadingtests)
    - [TestDbConfigurationTests](#testdbconfigurationtests)
  - [Entity Model Tests](#entity-model-tests)
  - [Test Performance Metrics](#test-performance-metrics)
  - [Common Error Patterns](#common-error-patterns)

## Infrastructure Tests

### DockerDatabaseTests

| Test Method                 | Status    | Error Message (if failing)                                                                                                                                                                                                                                                                                                                                                 |
| --------------------------- | --------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| DockerDesktopIsRunning      | ✅ Passing |                                                                                                                                                                                                                                                                                                                                                                            |
| SqlServerContainerIsRunning | ❌ Failing | "SQL Server container is not running. Please start the container." (Note: This test might pass in the IDE Test Explorer due to different execution context, but fails consistently with `dotnet test`)                                                                                                                                                                     |
| CanConnectToDatabase        | ❌ Failing | "Database connection failed"                                                                                                                                                                                                                                                                                                                                               |
| OpenDatabaseConnection      | ❌ Failing | "Database connection failed: A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: TCP Provider, error: 40 - Could not open a connection to SQL Server)" |

### ConfigurationLoadingTests

| Test Method                                                        | Status    | Error Message (if failing) |
| ------------------------------------------------------------------ | --------- | -------------------------- |
| Environment_SpecificSettings_OverrideBaseSettings                  | ✅ Passing |                            |
| NorthwindContextExtensions_UsesConnectionSettingsFromConfiguration | ✅ Passing |                            |
| Missing_Credentials_ThrowsInformativeException                     | ✅ Passing |                            |

### TestDbConfigurationTests

| Test Method                              | Status    | Error Message (if failing)                     |
| ---------------------------------------- | --------- | ---------------------------------------------- |
| GetTestSettings_LoadsCorrectDatabaseName | ✅ Passing |                                                |
| GetTestSettings_LoadsCorrectTimeout      | ✅ Passing |                                                |
| GetTestSettings_LoadsDatabaseUserName    | ✅ Passing |                                                |
| GetTestSettings_LoadsDBUserNamePassword  | ✅ Passing |                                                |
| CreateTestContext_UsesTestDatabase       | ✅ Passing |                                                |
| CreateTestContext_UsesTestTimeout        | ✅ Passing |                                                |
| FileBasedConfiguration_LoadsCorrectly    | ❌ Failing | "Database username not found in configuration" |
| InMemoryConfiguration_WorksCorrectly     | ✅ Passing |                                                |
| TestCredentials_AreLoadedCorrectly       | ✅ Passing |                                                |
| ConnectionString_UsesTestCredentials     | ✅ Passing |                                                |

## Entity Model Tests

| Test Method          | Status    | Error Message (if failing)                           |
| -------------------- | --------- | ---------------------------------------------------- |
| DatabaseConnectTest  | ❌ Failing | "Assert.True() Failure Expected: True Actual: False" |
| CategoriesCountTest  | ❌ Failing | "Login failed for user 'sa'."                        |
| ProductId1IsChaiTest | ❌ Failing | "Login failed for user 'sa'."                        |

## Test Performance Metrics

Current execution times from `dotnet test`:

- Total Duration: 62.6s
- Test Count: 20 total, 4 failed, 16 passed

## Common Error Patterns

1. **Docker Infrastructure Issues**
   - ✅ Docker Desktop running correctly
   - ❌ SQL Server container not running (likely causing network/TCP errors)
   - These affect all actual database connection tests

2. **Database Connection Issues**
   - **Primary:** Login failure for 'sa' user (seen in Entity Model Tests). Suggests wrong password in configuration/secrets or SQL Server configuration issue.
   - **Secondary:** Network/TCP connection errors (seen in `OpenDatabaseConnection`). Likely due to the container not running.
   - Missing user secrets (still a possible issue, especially for `FileBasedConfiguration_LoadsCorrectly`)
   - Test `DatabaseConnectTest` returning `false` for `CanConnect()` confirms basic connectivity failure.

3. **Configuration Loading Issues**
   - Missing user secrets is still the likely cause for the failure in `TestDbConfigurationTests.FileBasedConfiguration_LoadsCorrectly`.
   - Need to verify tests inherit from the correct base class after refactoring.
   - Need to ensure consistent credential handling.