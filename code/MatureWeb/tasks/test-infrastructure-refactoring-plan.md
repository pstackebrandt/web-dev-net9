# Test Infrastructure Refactoring Plan

A checklist of tasks for improving the test infrastructure, focusing on consistency and separation of concerns.
Last updated: After Test Base Classes Redesign.

## Table of Contents

- [Test Infrastructure Refactoring Plan](#test-infrastructure-refactoring-plan)
  - [Table of Contents](#table-of-contents)
  - [Immediate Infrastructure Fixes](#immediate-infrastructure-fixes)
  - [DockerDatabaseTests Consistency Issues](#dockerdatabasetests-consistency-issues)
  - [Test Base Classes Redesign](#test-base-classes-redesign)
  - [Documentation Updates](#documentation-updates)
  - [Testing and Verification](#testing-and-verification)
  - [Implementation Sequence](#implementation-sequence)
  - [Progress Notes](#progress-notes)
    - [DockerDatabaseTests Refactoring (Completed)](#dockerdatabasetests-refactoring-completed)
    - [Documentation Updates (In Progress)](#documentation-updates-in-progress)
    - [Test Base Classes Redesign (Completed)](#test-base-classes-redesign-completed)

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

- [x] Make database connection tests use consistent context creation approach:
  - [x] Update `CanConnectToDatabase` to use the same approach as `OpenDatabaseConnection` (completed)
  - [x] Ensure both tests use file-based settings for real database tests (completed)
  - [x] Remove duplicate code by extracting common setup into helper methods (created `CreateFileBasedTestContext()`)

- [x] Implement consistent error reporting:
  - [x] Add more descriptive error information to `CanConnectToDatabase` while keeping it simple (added connection string to error message)
  - [x] Add a TODO comment to restore password masking with explanation (added to both methods)

- [x] Adopt consistent naming convention for test methods:
  - [x] Document naming convention guidelines in `test-naming-conventions.md` (created in docs folder)
  - [x] Review current test method names (`DockerDesktopIsRunning`, `SqlServerContainerIsRunning`, etc.) (completed)
  - [x] Establish pattern (`[MethodUnderTest]_[Scenario]_[ExpectedResult]`) (completed)
  - [x] Rename methods in `DockerDatabaseTests.cs` for consistency (completed)

## Test Base Classes Redesign

> **Note**: A detailed implementation plan for this section is available in [test-base-refactoring-plan.md](test-base-refactoring-plan.md).

- [x] Refactor `TestBase` into a true abstract base class:
  - [x] Move in-memory specific functionality out of TestBase
  - [x] Keep only shared functionality needed by all test types (like `BuildConfiguration()`)
  - [x] Make class abstract and remove specialized settings methods

- [x] Create `InMemoryTestBase` class:
  - [x] Inherit from the redesigned `TestBase`
  - [x] Implement in-memory specific configuration methods (`GetInMemoryTestSettings()`)
  - [x] Add helper method for creating in-memory context (`CreateInMemoryContext()`)
  - [x] Add documentation explaining when to use this base class

- [x] Update `DatabaseTestBase` class:
  - [x] Inherit from the redesigned `TestBase`
  - [x] Implement file-based settings method (`GetFileBasedTestSettings()`)
  - [x] Add helper method for creating database context (`CreateDatabaseContext()`)
  - [x] Remove `new` keyword overrides which are no longer needed
  - [x] Improve documentation on database connection requirements

- [x] Update test classes to use appropriate base:
  - [x] Identify tests that use in-memory configuration
  - [x] Update those tests to inherit from `InMemoryTestBase` (created `InMemoryConfigurationTests.cs`)
  - [x] Verify database tests correctly inherit from `DatabaseTestBase` (updated `EntityModelTests.cs`)

## Documentation Updates

- [ ] Update test documentation files:
  - [x] Create `test-naming-conventions.md` with guidelines for naming test methods and classes
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
  - [x] After DockerDatabaseTests consistency changes (verified tests run with better diagnostics)
  - [x] After applying new naming conventions to `DockerDatabaseTests.cs`
  - [x] After TestBase abstraction and class hierarchy redesign (verified build succeeded)
  - [ ] After migrating tests to new infrastructure
  - [ ] After addressing any remaining issues

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
2. ✅ Fix DbContext creation in Docker-related tests (completed)
3. ✅ Continue with the DockerDatabaseTests consistency fixes (completed)
4. ✅ Document test naming conventions (completed)
5. ✅ Rename tests in `DockerDatabaseTests.cs` (completed)
6. ✅ Create the abstract base class structure (completed)
   - ↳ See [test-base-refactoring-plan.md](test-base-refactoring-plan.md) for detailed steps
7. ✅ Implement the new derived test base classes (completed)
8. ✅ Gradually migrate existing tests to the new structure (completed)
9. Update documentation to reflect changes
10. Run all tests to ensure functionality is preserved

## Progress Notes

### DockerDatabaseTests Refactoring (Completed)
- Created `CreateFileBasedTestContext()` helper method to eliminate duplicate code
- Enhanced error reporting in `CanConnectToDatabase` with connection string info
- Added TODOs for password masking in both test methods
- Ensured both tests use file-based settings consistently
- Applied new naming convention `[MethodUnderTest]_[Scenario]_[ExpectedResult]` to all test methods in this class

### Documentation Updates (In Progress)
- Created `test-naming-conventions.md` in the docs folder
- Established consistent pattern for test method naming: `[MethodUnderTest]_[Scenario]_[ExpectedResult]`
- Documented class naming convention with `Tests` suffix and folder organization recommendations
- Created detailed implementation plan for test base classes redesign in `test-base-refactoring-plan.md`

### Test Base Classes Redesign (Completed)
- Refactored TestBase into a true abstract class with only shared functionality (BuildConfiguration and FindSolutionRoot)
- Created new InMemoryTestBase class for tests requiring in-memory database
- Updated DatabaseTestBase to use the new abstract TestBase
- Fixed issues with test class dependencies (EntityModelTests.cs)
- Created a specialized InMemoryConfigurationTests class
- Added the required EF Core InMemory package
- Verified successful build after all changes