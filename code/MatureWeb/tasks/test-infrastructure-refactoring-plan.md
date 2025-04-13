# Test Infrastructure Refactoring Plan

A checklist of tasks for improving the test infrastructure, focusing on consistency and separation of concerns.
Last updated: After Docker restart.

## Table of Contents

- [Test Infrastructure Refactoring Plan](#test-infrastructure-refactoring-plan)
  - [Table of Contents](#table-of-contents)
  - [Immediate Infrastructure Fixes](#immediate-infrastructure-fixes)
  - [DockerDatabaseTests Consistency Issues](#dockerdatabasetests-consistency-issues)
  - [Test Base Classes Redesign](#test-base-classes-redesign)
  - [Documentation Updates](#documentation-updates)
  - [Testing and Verification](#testing-and-verification)
  - [Implementation Sequence](#implementation-sequence)

## Immediate Infrastructure Fixes

- [ ] Start and configure SQL Server container:
  - [ ] Run `docker start sql` or equivalent command to start the container
  - [ ] Verify SQL Server is accepting connections with `docker logs sql`
  - [ ] Check port mapping with `docker port sql` to ensure 1433 is properly mapped
  - [ ] Configure proper credentials to match test expectations

- [ ] Configure user secrets for test project:
  - [ ] Run `dotnet user-secrets init --project Northwind.UnitTests` if not already initialized
  - [ ] Set database credentials with correct values:
    ```bash
    dotnet user-secrets set "Database:MY_SQL_USR" "sa" --project Northwind.UnitTests
    dotnet user-secrets set "Database:MY_SQL_PWD" "YourStrongPassword" --project Northwind.UnitTests
    ```
  - [ ] Verify secrets are correctly set with `dotnet user-secrets list --project Northwind.UnitTests`

## DockerDatabaseTests Consistency Issues

- [ ] Make database connection tests use consistent context creation approach:
  - [ ] Update `CanConnectToDatabase` to use the same approach as `OpenDatabaseConnection`
  - [ ] Ensure both tests use file-based settings for real database tests
  - [ ] Remove duplicate code by extracting common setup into helper methods

- [ ] Implement consistent error reporting:
  - [ ] Add more descriptive error information to `CanConnectToDatabase` while keeping it simple
  - [ ] Add a TODO comment to restore password masking with explanation:
    ```csharp
    // TODO: Restore password masking before committing to shared repositories
    // Password = "********" // Mask the password for security
    ```

- [ ] Adopt consistent naming convention for test methods:
  - [ ] Review current test method names (`DockerDesktopIsRunning`, `SqlServerContainerIsRunning`, etc.)
  - [ ] Establish pattern (e.g., all begin with "Test" or "Can" or "Should")
  - [ ] Rename methods for consistency while preserving meaningful descriptive names

## Test Base Classes Redesign

- [ ] Refactor `TestBase` into a true abstract base class:
  - [ ] Move in-memory specific functionality out of TestBase
  - [ ] Keep only shared functionality needed by all test types
  - [ ] Make methods abstract where derived classes must provide implementation

- [ ] Create `InMemoryTestBase` class:
  - [ ] Inherit from the redesigned `TestBase`
  - [ ] Implement in-memory specific configuration methods
  - [ ] Add documentation explaining when to use this base class

- [ ] Update `DatabaseTestBase` class:
  - [ ] Inherit from the redesigned `TestBase`
  - [ ] Remove `new` keyword overrides which are no longer needed
  - [ ] Improve documentation on database connection requirements

- [ ] Update test classes to use appropriate base:
  - [ ] Identify tests that use in-memory configuration
  - [ ] Update those tests to inherit from `InMemoryTestBase`
  - [ ] Verify database tests correctly inherit from `DatabaseTestBase`

## Documentation Updates

- [ ] Update test documentation files:
  - [ ] Revise test-configuration-guide.md to reflect new class structure
  - [ ] Update TestStructure.md with new inheritance diagram
  - [ ] Add documentation on when to use each test base class

- [ ] Document design decisions:
  - [ ] Create a document explaining the separation of concerns
  - [ ] Include rationale for the inheritance hierarchy
  - [ ] Document naming conventions chosen for tests

## Testing and Verification

- [x] Establish baseline before changes:
  - [x] Document currently passing and failing tests
  - [x] Record specific error messages for failing tests

- [ ] Implement verification after each refactoring stage:
  - [ ] After SQL Server container and credential setup
  - [ ] After DockerDatabaseTests consistency changes
  - [ ] After TestBase abstraction
  - [ ] After implementing new base classes
  - [ ] After migrating tests to new infrastructure

- [ ] Manage expectations for temporary test failures:
  - [ ] Identify tests expected to temporarily fail during transition
  - [ ] Use `[Fact(Skip = "Temporarily disabled during refactoring")]` for problematic tests
  - [ ] Create checklist of tests to re-enable after infrastructure changes

- [ ] Final verification:
  - [ ] Ensure all tests that passed before refactoring still pass
  - [ ] Verify that previously failing tests now pass or fail with more informative messages
  - [ ] Run performance comparison to ensure test execution time is not adversely affected

## Implementation Sequence

1. Start SQL Server container and configure credentials
2. Fix DbContext creation in Docker-related tests
3. Continue with the DockerDatabaseTests consistency fixes
4. Create the abstract base class structure but don't move existing tests yet
5. Implement the new derived test base classes
6. Gradually migrate existing tests to the new structure
7. Update documentation to reflect changes
8. Run all tests to ensure functionality is preserved