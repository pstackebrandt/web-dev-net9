# Test Best Practices

Guidelines for writing effective, maintainable tests for the Northwind project.

## Table of Contents

- [Test Best Practices](#test-best-practices)
  - [Table of Contents](#table-of-contents)
  - [General Principles](#general-principles)
  - [Arrange-Act-Assert Pattern](#arrange-act-assert-pattern)
  - [Error Handling](#error-handling)
  - [Test Data Management](#test-data-management)
  - [Test Performance](#test-performance)

## General Principles

1. **Single Responsibility**: Each test should verify one specific behavior or requirement.
2. **Independence**: Tests should not depend on other tests or execution order.
3. **Readability**: Test code should be as clear and readable as possible.
4. **Maintainability**: Prefer duplication in tests over complex abstractions.
5. **Reliability**: Tests should not be flaky or produce different results on different runs.

## Arrange-Act-Assert Pattern

Structure tests using the Arrange-Act-Assert pattern:

```csharp
[Fact]
public void CalculateTotal_WithValidItems_ReturnsSumOfPrices()
{
    // Arrange
    var calculator = new PriceCalculator();
    var items = new[] { new Item { Price = 10 }, new Item { Price = 20 } };
    
    // Act
    var result = calculator.CalculateTotal(items);
    
    // Assert
    Assert.Equal(30, result);
}
```

## Error Handling

Effective error handling in tests makes troubleshooting failures much easier:

1. **Descriptive Error Messages**:
   ```csharp
   // Less helpful
   Assert.True(db.Database.CanConnect());
   
   // More helpful
   Assert.True(db.Database.CanConnect(), 
       $"Failed to connect to database with connection string: {MaskPassword(connectionString)}");
   ```

2. **Password Masking**:
   - Never display actual passwords in error messages
   - Implement a password masking helper:
   ```csharp
   private string MaskPassword(string connectionString)
   {
       // Use regex to replace password with asterisks
       return Regex.Replace(
           connectionString,
           @"Password=([^;]*)", 
           "Password=********"
       );
   }
   ```

3. **Context Information**:
   - Include relevant context in error messages
   - Show state information without exposing sensitive data:
   ```csharp
   Assert.NotNull(user, $"User not found. Search criteria: {userName}, Role: {role}");
   ```

4. **Exception Testing**:
   - Test both expected exceptions and happy paths:
   ```csharp
   [Fact]
   public void Process_WithInvalidInput_ThrowsArgumentException()
   {
       var exception = Assert.Throws<ArgumentException>(
           () => processor.Process(null)
       );
       
       Assert.Contains("cannot be null", exception.Message);
   }
   ```

## Test Data Management

1. **Use Test-Specific Data**:
   - Create dedicated test data, don't rely on production data
   - Reset state between tests when using a shared database

2. **Use InMemoryTestBase for Isolation**:
   - Use `InMemoryTestBase` for tests that don't need a real database
   - Each test gets its own isolated in-memory database

3. **Seed Data Consistently**:
   - Use helper methods to seed test data consistently
   - Create factory methods for common test entities

## Test Performance

1. **Use Fast Tests Where Possible**:
   - Prefer in-memory tests for business logic
   - Use real database tests only when necessary

2. **Optimize Database Tests**:
   - Minimize database operations in tests
   - Consider using a shared test database instance for test runs

3. **Targeted Tests**:
   - Test specific behaviors, not entire workflows
   - Split large tests into smaller, focused tests