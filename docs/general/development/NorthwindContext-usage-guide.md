# NorthwindContext Usage Guide

## Table of Contents

- [NorthwindContext Usage Guide](#northwindcontext-usage-guide)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Recommended Usage](#recommended-usage)
    - [Parameters](#parameters)
    - [Complete Example](#complete-example)
  - [Configuration Requirements](#configuration-requirements)
  - [Default Values](#default-values)
  - [Error Handling](#error-handling)
  - [Deprecated Alternative](#deprecated-alternative)

## Overview

The `AddNorthwindContext` extension method configures and adds the `NorthwindContext` database context to an ASP.NET Core application's service collection. This enables Entity Framework Core to connect to the Northwind database.

## Recommended Usage

The recommended way to use `AddNorthwindContext` is with the overload that accepts an `IConfiguration` parameter:

```csharp
services.AddNorthwindContext(configuration);
```

### Parameters

- `services`: The `IServiceCollection` to add services to
- `configuration`: The `IConfiguration` containing database connection settings
- `connectionString` (optional): A connection string that overrides settings from configuration

### Complete Example

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Northwind.EntityModels;

// In Program.cs or Startup.cs
var builder = WebApplication.CreateBuilder(args);

// Add NorthwindContext to services
try
{
    builder.Services.AddNorthwindContext(builder.Configuration);
}
catch (InvalidOperationException ex)
{
    // Handle database configuration errors
    Console.WriteLine($"Database configuration error: {ex.Message}");
}
```

## Configuration Requirements

For the method to work correctly, your configuration should include:

1. **Database Connection Settings**:

```json
{
  "DatabaseConnection": {
    "DataSource": "your-server",
    "InitialCatalog": "Northwind",
    "TrustServerCertificate": true,
    "MultipleActiveResultSets": true,
    "ConnectTimeout": 30
  }
}
```

2. **Database Credentials**:

```json
{
  "Database": {
    "MY_SQL_USR": "your-username",
    "MY_SQL_PWD": "your-password"
  }
}
```

For security reasons, credentials should be stored in user secrets or environment variables in development, and secure storage mechanisms (like Azure Key Vault) in production.

## Default Values

If optional configuration values are missing, the following defaults will be used:

- `InitialCatalog`: "Northwind"
- `DataSource`: "tcp:127.0.0.1,1433"
- `TrustServerCertificate`: true
- `MultipleActiveResultSets`: true
- `ConnectTimeout`: 3 seconds

## Error Handling

The method will throw an `InvalidOperationException` if:

1. Required database credentials are missing
2. Configuration cannot be found or is invalid

Always wrap calls to `AddNorthwindContext` in a try/catch block to handle potential configuration errors gracefully.

## Deprecated Alternative

> **Note**: The overload that doesn't accept `IConfiguration` is deprecated and will be removed in a future version.

```csharp
// Deprecated - Do not use
services.AddNorthwindContext();
```

The deprecated version builds a temporary service provider during configuration, which is an anti-pattern in ASP.NET Core and can lead to memory leaks and other issues.