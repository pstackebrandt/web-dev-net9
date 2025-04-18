# AddNorthwindContext Call Sites Analysis

This document identifies all locations in the codebase that call the `AddNorthwindContext` extension method and analyzes whether these locations have access to an `IConfiguration` instance.

## Call Sites

### 1. Northwind.Mvc/Program.cs

```csharp
// Add NorthwindContext to the services container
if (sqlServerConnectionString != null)
{
    builder.Services.AddNorthwindContext(sqlServerConnectionString);
} else {
    Console.WriteLine("NorthwindConnection string is missing from configuration");
}
```

**Access to IConfiguration?** ✅ Yes
- Has direct access to `builder.Configuration` (WebApplicationBuilder.Configuration property)
- Can be easily modified to pass the configuration instance

### 2. Northwind.UnitTests/Infrastructure/ConfigurationLoadingTests.cs

There are two calls to `AddNorthwindContext` in this file:

**Test: NorthwindContextExtensions_UsesConnectionSettingsFromConfiguration**
```csharp
// Create configuration
var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(configValues)
    .Build();

services.AddSingleton<IConfiguration>(configuration);

// Act
services.AddNorthwindContext();
```

**Test: Missing_Credentials_ThrowsInformativeException**
```csharp
// Create configuration
var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(configValues)
    .Build();

services.AddSingleton<IConfiguration>(configuration);

// Act & Assert
var exception = Assert.Throws<InvalidOperationException>(() =>
{
    services.AddNorthwindContext();
    var serviceProvider = services.BuildServiceProvider();
    _ = serviceProvider.GetRequiredService<NorthwindContext>();
});
```

**Access to IConfiguration?** ✅ Yes
- Both test methods create a configuration object
- The configuration is added to the services collection
- Tests could be easily modified to pass the configuration directly

## Summary

Total call sites: 3

| Location                              | Has IConfiguration Access | Modification Complexity |
| ------------------------------------- | ------------------------- | ----------------------- |
| Program.cs                            | Yes                       | Simple                  |
| ConfigurationLoadingTests.cs (Test 1) | Yes                       | Simple                  |
| ConfigurationLoadingTests.cs (Test 2) | Yes                       | Simple                  |

## Conclusion

All identified call sites of `AddNorthwindContext` have direct access to an `IConfiguration` instance. Refactoring the extension method to accept an `IConfiguration` parameter would require only minor changes to the calling code.

The refactoring can proceed with minimal risk, as all call sites can be easily updated to pass the configuration instance directly instead of relying on the extension method to build a service provider.

## Recommendations

1. Create the new overload of `AddNorthwindContext` that accepts an `IConfiguration` parameter
2. Update all call sites to use the new method signature
3. Deprecate the original method 