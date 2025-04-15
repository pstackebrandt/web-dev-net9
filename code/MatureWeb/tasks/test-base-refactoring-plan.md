# Test Base Classes Refactoring Implementation Plan

This document outlines the specific steps to refactor the test base classes to achieve better separation of concerns between in-memory tests and database tests.

## Current Structure Analysis

Before implementing changes, we need to analyze the current structure:

1. Read `TestBase.cs` to identify:
   - Common configuration loading code
   - In-memory database setup code
   - Any methods or properties shared by all tests

2. Read `DatabaseTestBase.cs` to identify:
   - How it currently inherits from `TestBase`
   - Any methods that override or hide base class methods with `new` keyword
   - Database-specific functionality

## Implementation Steps

### 1. Refactor TestBase into Abstract Base Class

- Modify `TestBase.cs`:
  ```csharp
  public abstract class TestBase
  {
      // Keep only truly common code here
      
      // Add shared configuration builder method
      protected static IConfigurationRoot BuildConfiguration()
      {
          // Code to build configuration from appsettings.json, 
          // appsettings.Testing.json, and user secrets
      }
      
      // Remove in-memory specific methods
      // Remove GetFileBasedTestSettings() and GetInMemoryTestSettings()
  }
  ```

### 2. Create InMemoryTestBase

- Create new file `InMemoryTestBase.cs`:
  ```csharp
  public class InMemoryTestBase : TestBase
  {
      // Add in-memory specific methods
      protected NorthwindDbSettings GetInMemoryTestSettings()
      {
          // Implementation to create settings for in-memory database
          // May use BuildConfiguration() if needed for some settings
      }
      
      // Add helper to create context with in-memory provider
      protected NorthwindContext CreateInMemoryContext()
      {
          var settings = GetInMemoryTestSettings();
          
          var options = new DbContextOptionsBuilder<NorthwindContext>()
              .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
              .Options;
              
          return new NorthwindContext(options);
      }
  }
  ```

### 3. Update DatabaseTestBase

- Modify `DatabaseTestBase.cs`:
  ```csharp
  public class DatabaseTestBase : TestBase
  {
      // Add file-based settings method (possibly moving from TestBase)
      protected ConnectionSettings GetFileBasedTestSettings()
      {
          var configuration = BuildConfiguration();
          
          // Extract connection settings from configuration
          // Return settings object
      }
      
      // Add helper to create context with real database
      protected NorthwindContext CreateDatabaseContext()
      {
          var settings = GetFileBasedTestSettings();
          var builder = DatabaseConnectionBuilder.CreateBuilder(settings);
          
          var options = new DbContextOptionsBuilder<NorthwindContext>()
              .UseSqlServer(builder.ConnectionString)
              .Options;
              
          return new NorthwindContext(options);
      }
      
      // Update existing methods that might have used 'new' keyword
      // to hide TestBase implementations that no longer exist
  }
  ```

### 4. Build and Verify Base Class Structure

- Compile project to ensure no syntax errors
- Check for compiler warnings related to inheritance
- Run a small subset of existing tests (without moving them yet) to ensure base functionality works

## Migration Strategy (Next Phase)

After base class refactoring is complete:

1. Identify which existing test classes should use `InMemoryTestBase` vs `DatabaseTestBase`
2. For each test class:
   - Update inheritance to use appropriate base class
   - Modify test setup code to use the new helper methods
   - Run tests to verify functionality is preserved

## Potential Issues and Mitigations

- **Concern**: Existing tests might directly call methods being moved or removed
  - **Mitigation**: Review test code for direct calls to TestBase methods before refactoring

- **Concern**: Some tests might need both in-memory and file-based functionality
  - **Mitigation**: Identify these edge cases during analysis; consider creating utility classes to provide the needed functionality without multiple inheritance

- **Concern**: Configuration loading might be needed in both base classes
  - **Mitigation**: Ensure `BuildConfiguration()` in `TestBase` is accessible to both derived classes, or consider making it `protected internal static` to allow broader access

## Completion Criteria

The refactoring is complete when:

1. `TestBase` is abstract and contains only shared functionality
2. `InMemoryTestBase` provides specific in-memory database testing functionality
3. `DatabaseTestBase` provides specific file-based database testing functionality
4. No code duplication exists between the classes
5. The class structure compiles without errors or warnings
6. Existing tests still work without modification (until the migration phase)