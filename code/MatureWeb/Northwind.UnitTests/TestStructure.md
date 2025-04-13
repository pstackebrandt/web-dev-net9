# Test Project Structure

This document outlines the recommended folder and file structure for the
Northwind test projects.

## Table of Contents

- [Test Project Structure](#test-project-structure)
  - [Table of Contents](#table-of-contents)
  - [Folder Structure](#folder-structure)
  - [Additional Test Projects](#additional-test-projects)
  - [Organization Principles](#organization-principles)
  - [Base Class Hierarchy](#base-class-hierarchy)
  - [Naming Conventions](#naming-conventions)
  - [Migration Plan](#migration-plan)

## Folder Structure

```text
Northwind.UnitTests/
├── Infrastructure/         # Tests for config, connections, setup
│   ├── TestDbConfigurationTests.cs    # Database configuration validation
│   ├── DatabaseTestBase.cs      # Base class for DB tests 
│   ├── ConfigurationFileTestBase.cs   # Base class for config tests
│   ├── DockerDatabaseTests.cs    # Docker and DB connection tests
│   └── ConfigurationLoadingTests.cs   # Configuration loading tests
├── Repositories/           # Unit tests for data access logic
│   ├── CustomerRepositoryTests.cs
│   ├── ProductRepositoryTests.cs
│   └── OrderRepositoryTests.cs
├── Services/               # Unit tests for business logic services
│   ├── OrderServiceTests.cs
│   ├── InventoryServiceTests.cs
│   └── UserServiceTests.cs
├── Controllers/            # Unit tests for API/UI controllers
│   ├── CustomerControllerTests.cs
│   └── OrderControllerTests.cs
├── TestBase.cs                  # Base class providing common test setup
└── TestHelpers/            # Utility classes for testing (mocks, fixtures)
    ├── DatabaseSeed.cs          # Test data generation
    ├── TestFixtures.cs          # xUnit fixtures
    └── MockBuilders.cs          # Helper for mocks
```

## Additional Test Projects

```text
Northwind.IntegrationTests/
├── ApiTests/                    # End-to-end API tests
├── DataAccessTests/             # Repository tests using a real database
└── TestBase.cs

Northwind.PerformanceTests/      # Optional load/performance tests
```

## Organization Principles

1. `Test Type Separation`: Separate unit tests from integration and
   performance tests.
2. `Component-Based Organization`: Group tests by component type
   (repositories, services, controllers).
3. `Infrastructure Tests`: Keep tests that verify configuration and setup
   in a dedicated folder.
4. `Shared Test Utilities`: Common test code and helpers have their own location.

## Base Class Hierarchy

The test project uses a hierarchy of base classes to provide specialized functionality:

```
           ┌─────────┐
           │ TestBase│
           └────┬────┘
                │
     ┌──────────┴───────────┐
     │                      │
┌────┴─────┐       ┌────────┴──────────┐
│DatabaseTestBase│ │ConfigurationFileTestBase│
└──────────┘       └───────────────────┘
```

1. **TestBase**: Primary base class that provides:
   - Basic test configuration (in-memory by default)
   - Common context creation methods
   - Shared test utilities

2. **DatabaseTestBase**: For tests requiring real database connections:
   - Overrides configuration to use file-based settings
   - Expects database credentials in user secrets
   - Use for entity model tests and data access tests

3. **ConfigurationFileTestBase**: For tests verifying configuration loading:
   - Provides utilities for testing configuration files
   - Helps verify layered configuration behavior
   - Use for configuration and settings tests

See [Test Configuration Guide](../docs/test-configuration-guide.md) for detailed usage.

## Naming Conventions

1. Test class names should end with `Tests` (e.g., `CustomerRepositoryTests`)
2. Test methods should follow the pattern `[MethodName]_[Scenario]_[ExpectedResult]`
3. Test projects should be named `[ProjectName].[TestType]` (e.g., `Northwind.UnitTests`)

## Migration Plan

1. Move existing configuration tests to the Infrastructure folder
2. Create appropriate subdirectories as new tests are added
3. Refactor existing tests to follow the new structure as time permits