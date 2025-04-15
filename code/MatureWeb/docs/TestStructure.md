# Test Project Structure

This document outlines the test structure for the Northwind project, including the folder organization, base class \
hierarchy, and naming conventions.

## Table of Contents

- [Test Project Structure](#test-project-structure)
  - [Table of Contents](#table-of-contents)
  - [Folder Structure](#folder-structure)
  - [Additional Test Projects](#additional-test-projects)
  - [Organization Principles](#organization-principles)
  - [Base Class Hierarchy](#base-class-hierarchy)
  - [Class Descriptions](#class-descriptions)
  - [Naming Conventions](#naming-conventions)
  - [Migration Plan](#migration-plan)

## Folder Structure

> **Note:** This structure shows both existing files (**bold**) and recommended files (non-bold) that should be created as the project evolves.

```text
Northwind.UnitTests/
├── Infrastructure/         # Tests for config, connections, setup
│   ├── **TestDbConfigurationTests.cs**    # Database configuration validation
│   ├── **DatabaseTestBase.cs**      # Base class for DB tests 
│   ├── **ConfigurationFileTestBase.cs**   # Base class for config tests
│   ├── **DockerDatabaseTests.cs**    # Docker and DB connection tests
│   └── **ConfigurationLoadingTests.cs**   # Configuration loading tests
├── Repositories/           # Unit tests for data access logic
│   ├── CustomerRepositoryTests.cs    # To be implemented
│   ├── ProductRepositoryTests.cs     # To be implemented
│   └── OrderRepositoryTests.cs       # To be implemented
├── Services/               # Unit tests for business logic services
│   ├── OrderServiceTests.cs          # To be implemented
│   ├── InventoryServiceTests.cs      # To be implemented
│   └── UserServiceTests.cs           # To be implemented
├── Controllers/            # Unit tests for API/UI controllers
│   ├── CustomerControllerTests.cs    # To be implemented
│   └── OrderControllerTests.cs       # To be implemented
├── **TestBase.cs**              # Base class providing common test setup
└── TestHelpers/            # Utility classes for testing (mocks, fixtures)
    ├── DatabaseSeed.cs          # Test data generation (to be implemented)
    ├── TestFixtures.cs          # xUnit fixtures (to be implemented)
    └── MockBuilders.cs          # Helper for mocks (to be implemented)
```

## Additional Test Projects

> **Note:** These are planned/recommended test projects that may not yet exist.

```text
Northwind.IntegrationTests/
├── ApiTests/                    # End-to-end API tests
├── DataAccessTests/             # Repository tests using a real database
└── TestBase.cs

Northwind.PerformanceTests/      # Optional load/performance tests
```

## Organization Principles

1. `Test Type Separation`: Separate unit tests from integration and performance tests.
2. `Component-Based Organization`: Group tests by component type (repositories, services, controllers).
3. `Infrastructure Tests`: Keep tests that verify configuration and setup in a dedicated folder.
4. `Shared Test Utilities`: Common test code and helpers have their own location.

## Base Class Hierarchy

The test project uses a hierarchy of base classes to provide specialized functionality:

```mermaid
graph TD
    A[TestBase (abstract)] --> B(InMemoryTestBase);
    A --> C(DatabaseTestBase);
    A --> D(ConfigurationFileTestBase);
```

> **Note:** All of these base classes exist in the current codebase.

## Class Descriptions

- **`TestBase` (abstract)**
  - The fundamental abstract base class for all tests.
  - Provides common configuration loading capabilities via the `BuildConfiguration` method, ensuring consistent access \
  to application settings (like `appsettings.json` and user secrets).

- **`InMemoryTestBase`**
  - Inherits from `TestBase`.
  - Designed for tests that require an isolated in-memory database.
  - Provides the `CreateInMemoryContext()` method to easily set up an `ApplicationDbContext` configured for the EF Core In-Memory provider.
  - Requires the `Microsoft.EntityFrameworkCore.InMemory` NuGet package.

- **`DatabaseTestBase`**
  - Inherits from `TestBase`.
  - Designed for tests that need to interact with a real database (e.g., SQL Server, potentially running in Docker).
  - Provides the `CreateDatabaseContext()` method, which uses connection strings sourced from configuration files (managed by `TestBase`).
  - Often requires environment-specific setup (like connection strings in user secrets or environment variables).

- **`ConfigurationFileTestBase`**
  - Inherits from `TestBase`.
  - Provides utilities for testing configuration files.
  - Helps verify layered configuration behavior.
  - Use for configuration and settings tests.

See [Test Configuration Guide](./test-configuration-guide.md) for detailed usage.

## Naming Conventions

1. Test class names should end with `Tests` (e.g., `CustomerRepositoryTests`)
2. Test methods should follow the pattern `[MethodName]_[Scenario]_[ExpectedResult]`
3. Test projects should be named `[ProjectName].[TestType]` (e.g., `Northwind.UnitTests`)

## Migration Plan

1. Move existing configuration tests to the Infrastructure folder
2. Create appropriate subdirectories as new tests are added
3. Refactor existing tests to follow the new structure as time permits