# .NET Configuration Best Practices

A guide to implementing structured, secure, and environment-specific configuration in .NET applications
with focus on the layering pattern and credentials management.

## Table of Contents

- [.NET Configuration Best Practices](#net-configuration-best-practices)
  - [Table of Contents](#table-of-contents)
  - [Configuration Layering Pattern](#configuration-layering-pattern)
  - [Key Principles](#key-principles)
    - [1. Base Configuration as Documentation](#1-base-configuration-as-documentation)
    - [2. Environment-Specific Overrides](#2-environment-specific-overrides)
    - [3. Strongly-Typed Configuration](#3-strongly-typed-configuration)
    - [4. Secret Management](#4-secret-management)
  - [User Secrets for Development](#user-secrets-for-development)
    - [Project-Specific Secret Storage](#project-specific-secret-storage)
  - [Configuration Loading Order](#configuration-loading-order)
  - [Security Best Practices](#security-best-practices)
  - [Static Web Assets in Production Mode](#static-web-assets-in-production-mode)
    - [Problem](#problem)
    - [Solution](#solution)
    - [Benefits](#benefits)
    - [Important Note](#important-note)

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

## Security Best Practices

Follow these practices to maintain security of configuration data:

- Never commit real credentials to source control
- Store development credentials in User Secrets
- Use environment variables or secure vaults for production credentials
- Different credentials should be used for development and production
- Regularly rotate credentials (especially after team member departures)
- Use the principle of least privilege for database access
- Avoid hardcoding connection strings or credentials in code
- Keep sensitive settings (like connection strings) out of logs
- Encrypt sensitive configuration values when possible
- Audit access to sensitive configuration regularly

## Static Web Assets in Production Mode

### Problem

When running an ASP.NET Core application in Production mode for local testing, static web assets
(CSS, JavaScript, images) may fail to load with errors like:

```
FileNotFoundException: Could not find file 'wwwroot/Identity/lib/bootstrap/dist/css/bootstrap.min.css'
```

This occurs because static web assets are automatically enabled only in Development environment by default.

### Solution

Add the following code to manually enable static web assets in Production mode by using the `StaticWebAssetsLoader`:

```csharp
// Enable static web assets in Production mode
if (app.Environment.IsProduction())
{
    StaticWebAssetsLoader.UseStaticWebAssets(app.Environment, app.Configuration);
}
```

This code should be placed right after app initialization (`var app = builder.Build();`) and requires adding the namespace:
```csharp
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
```

### Benefits

- Correctly serves static assets with proper compression and caching
- Reduces file sizes by 80-90% through Brotli/Gzip compression
- Provides content-based ETags for efficient caching

### Important Note

This approach is suitable for testing purposes only. For real production deployments, properly
publishing the application is recommended. When an application is published, static assets are
automatically optimized and don't require this workaround.