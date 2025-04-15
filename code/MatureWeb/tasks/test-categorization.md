# Test Categorization

**STATUS: DEPRECATED** - This planning document is no longer needed. The information has been:
1. Implemented in the codebase through proper inheritance hierarchies
2. Documented in `docs/test-configuration-guide.md` under the "Test Categorization Examples" section

This file can be safely deleted.

## A) Tests Requiring Real Database

These tests need an actual connection to a real database and should use settings files and user secrets:

1. **EntityModelTests**:
   - `DatabaseConnectTest` - Tests actual database connection
   - `CategoriesCountTest` - Tests querying real data
   - `ProductId1IsChaiTest` - Tests retrieving a specific product

2. **DockerDatabaseTests**:
   - `DatabaseConnectionFailureReason` - Tests details of database connection failures

## B) Tests for Connection Creation Using External Settings

These tests verify that connection strings are properly created from external configuration (settings files & user secrets):

1. **TestDbConfigurationTests**:
   - `GetTestSettings_LoadsCorrectDatabaseName` - Verifies database name from settings file
   - `GetTestSettings_LoadsCorrectTimeout` - Verifies timeout value from settings file
   - `GetTestSettings_LoadsDatabaseUserName` - Verifies username from settings file/secrets
   - `GetTestSettings_LoadsDBUserNamePassword` - Verifies password loading from settings file/secrets
   - `CreateTestContext_UsesTestDatabase` - Verifies context creation with proper database name
   - `CreateTestContext_UsesTestTimeout` - Verifies context creation with proper timeout
   - `TestCredentials_AreLoadedCorrectly` - Verifies credentials are loaded from settings file/secrets
   - `ConnectionString_UsesTestCredentials` - Verifies credentials are included in connection string

## C) Tests That Don't Need Real Database

These tests verify configuration behavior without needing a real database connection and can use in-memory configuration:

1. **ConfigurationLoadingTests**:
   - `Environment_SpecificSettings_OverrideBaseSettings` - Tests environment-specific overrides
   - `NorthwindContextExtensions_UsesConnectionSettingsFromConfiguration` - Tests DI configuration
   - `Missing_Credentials_ThrowsInformativeException` - Tests error handling

## Summary

- **For categories A and B**:
  - Must restore the ability to load configuration from settings files and user secrets
  - Need real credentials to connect to the database
  - Should properly handle environment-specific settings

- **For category C**:
  - Can continue using in-memory configuration
  - Don't need real credentials or database connections
  - Useful for testing configuration logic in isolation