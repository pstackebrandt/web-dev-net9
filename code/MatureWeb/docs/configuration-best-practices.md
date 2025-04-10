# .NET Configuration Best Practices

A guide to implementing structured, secure, and environment-specific configuration in .NET applications
with focus on the layering pattern and credentials management.

## Table of Contents

- [Configuration Layering Pattern](#configuration-layering-pattern)
- [Key Principles](#key-principles)
  - [1. Base Configuration as Documentation](#1-base-configuration-as-documentation)
  - [2. Environment-Specific Overrides](#2-environment-specific-overrides)
  - [3. Strongly-Typed Configuration](#3-strongly-typed-configuration)
  - [4. Secret Management](#4-secret-management)
- [User Secrets for Development](#user-secrets-for-development)
  - [Project-Specific Secret Storage](#project-specific-secret-storage)
- [Configuration Loading Order](#configuration-loading-order)

## Configuration Layering Pattern

.NET Core and later versions use a configuration system that supports layering
multiple sources. This approach enables:

- **Base configuration** defined in `appsettings.json`
- **Environment-specific overrides** in files like `appsettings.Development.json`
  or `appsettings.Testing.json`
- **Secret management** outside of source control

## Key Principles

### 1. Base Configuration as Documentation

Your `appsettings.json` file should:
- Include **all** available configuration options
- Set reasonable default values where possible
- Serve as documentation for the configuration structure
- Avoid containing sensitive information (use placeholder values)

### 2. Environment-Specific Overrides

Files like `appsettings.Testing.json` should:
- Only contain settings that differ from the base configuration
- Maintain the same structure as the base file
- Be automatically applied based on the current environment

Example:
```json
// appsettings.json (base)
{
  "Database": {
    "MY_SQL_USR": "placeholder",
    "MY_SQL_PWD": "placeholder"
  },
  "DatabaseConnection": {
    "DataSource": "tcp:127.0.0.1,1433",
    "InitialCatalog": "Northwind",
    "TrustServerCertificate": true,
    "MultipleActiveResultSets": true,
    "ConnectTimeout": 3
  }
}

// appsettings.Testing.json (override)
{
  "DatabaseConnection": {
    "ConnectTimeout": 1
  }
}
```

### 3. Strongly-Typed Configuration

Always use strongly-typed configuration classes:

```csharp
// Register in Program.cs/Startup.cs
services.Configure<DatabaseSettings>(
    Configuration.GetSection("Database"));
services.Configure<DatabaseConnectionSettings>(
    Configuration.GetSection("DatabaseConnection"));

// Inject in services
public class MyService
{
    private readonly DatabaseSettings _dbSettings;
    
    public MyService(IOptions<DatabaseSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }
}
```

### 4. Secret Management

Sensitive data should never be committed to source control:

- **Development**: Use User Secrets
- **Production**: Use environment variables or a secure vault

## User Secrets for Development

.NET provides a built-in "User Secrets" feature for development:

```bash
# Initialize user secrets for a project
dotnet user-secrets init --project code/MatureWeb/Northwind.DataContext

# Add a secret
dotnet user-secrets set "Database:MY_SQL_USR" "sa" \
  --project code/MatureWeb/Northwind.DataContext
dotnet user-secrets set "Database:MY_SQL_PWD" "yourpassword" \
  --project code/MatureWeb/Northwind.DataContext
```

These secrets are stored outside your project directory in:
- Windows: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
- macOS/Linux: `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`

### Project-Specific Secret Storage

Store secrets in the specific project that uses them directly (e.g., Northwind.DataContext) rather 
than the solution root. This follows best practices because:

- **Direct Usage Point**: Keeps secrets with the code that consumes them
- **Proper Isolation**: Each project has its own user-secrets-id in the project file
- **Least Privilege**: Only projects that need credentials have access to them
- **Standard Practice**: Follows Microsoft's recommended pattern for secret management

## Configuration Loading Order

The ASP.NET Core configuration system loads settings in this order
(later sources override earlier ones):

1. Base `appsettings.json`
2. Environment-specific `appsettings.{Environment}.json`
3. User Secrets (in Development environment)
4. Environment variables
5. Command-line arguments

This allows for flexible, environment-specific configuration without modifying code.