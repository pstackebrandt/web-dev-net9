# Test Naming Conventions

Adopting a consistent naming convention for software tests improves readability, clarity, and maintainability across the codebase, particularly when using frameworks like xUnit for .NET projects.

## Test Method Naming

### Recommended Structure

A clear test method name should consist of three core components, separated by underscores. Test methods follow standard C# PascalCase naming conventions for the overall name.

`[MethodUnderTest]_[Scenario]_[ExpectedResult]`

*Note: Test frameworks like xUnit identify test methods using attributes (e.g., `[Fact]`, `[Theory]`), so an explicit `_Test` suffix on the method name is generally redundant and not recommended.*

### Component Explanation

* **MethodUnderTest**: The name of the method or logical unit of functionality being tested (PascalCase).
* **Scenario**: The specific condition, input, or state being tested (PascalCase, describing the 'when' or 'given').
* **ExpectedResult**: The expected outcome or state after the test executes (PascalCase, describing the 'then' or expected behavior).

### Examples

Using PascalCase for each component separated by underscores:

#### Unit Test Examples

* `CalculateSum_WithPositiveIntegers_ReturnsCorrectSum`
* `Login_WithInvalidCredentials_ReturnsAuthenticationError`
* `GetProduct_ById_ReturnsProductDto`
* `ProcessPayment_WithValidCreditCard_ProcessesSuccessfully`
* `CalculateAverage_WithEmptyArray_ThrowsArgumentException`

#### Integration Test Examples

* `GetUserDetails_WhenUserExists_ReturnsUserInformation`
* `SaveOrder_WhenDatabaseUnavailable_ThrowsDbUpdateException`
* `UpdateInventory_WhenProductSold_DecrementsStockCount`

### Case Study: DockerDatabaseTests Refactoring

During the test infrastructure refactoring, test methods in `DockerDatabaseTests.cs` were renamed to follow the convention. Here are examples of the transformation:

| Before                        | After                                          |
| ----------------------------- | ---------------------------------------------- |
| `DockerDesktopIsRunning`      | `CheckDocker_WhenRunning_ReturnsTrue`          |
| `SqlServerContainerIsRunning` | `CheckSqlContainer_WhenRunning_ReturnsTrue`    |
| `OpenDatabaseConnection`      | `OpenConnection_WithValidCredentials_Succeeds` |
| `CanConnectToDatabase`        | `ConnectToDatabase_WithValidSettings_Succeeds` |

The refactored names make it immediately clear:
1. What functionality is being tested
2. Under what conditions the test runs
3. What the expected outcome is

## Test Class Naming and Organization

### Naming Convention for Test Classes

Test classes should clearly correspond to the class or functional module they test, appended with the suffix `Tests` and following PascalCase:

`[ClassOrModuleName]Tests`

#### Examples

* `UserServiceTests`
* `OrderRepositoryTests`
* `PaymentProcessorTests`
* `ProductControllerTests`

### Organization Recommendations

* Place test classes within a dedicated test project (e.g., `ProjectName.Tests`).
* Mirror the source code's namespace structure within the test project where practical.
* Maintain a clear separation between different types of tests, commonly using folders:
  * `Tests/Unit/`
  * `Tests/Integration/`
  * *(Optional: `Tests/Acceptance/`, `Tests/Performance/` etc., as needed)*

## General Recommendations

* **Consistency**: Apply the chosen naming convention uniformly across the entire test suite. The underscore pattern (`Method_Scenario_Result`) is specific to this test naming style for descriptive purposes. Standard C# naming conventions apply otherwise.
* **Readability**: Prioritize names that are easy to read and understand at a glance.
* **Clarity over Brevity**: While aiming for concise names, do not sacrifice clarity. Ensure the name accurately reflects the test's specific purpose.

By following these conventions, the test suite becomes a valuable form of documentation, making the codebase easier to understand, maintain, and extend.