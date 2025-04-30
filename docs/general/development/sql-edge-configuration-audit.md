# SQL Edge Configuration Audit

Comprehensive audit of Azure SQL Edge configuration locations and usage throughout the codebase.

> **Note**: This is a companion document to the [SQL Edge Container Management](./sql-edge-container-management.md) 
> guide, which contains the overall strategy and implementation plan.

## Table of Contents
- [SQL Edge Configuration Audit](#sql-edge-configuration-audit)
  - [Table of Contents](#table-of-contents)
  - [Configuration Locations](#configuration-locations)
    - [1. Aspire Configuration](#1-aspire-configuration)
    - [2. Database Connection Settings](#2-database-connection-settings)
    - [3. Test Environment Setup](#3-test-environment-setup)
    - [4. Example Configuration Files](#4-example-configuration-files)
    - [5. Test Configuration](#5-test-configuration)
    - [6. Database Context Configuration](#6-database-context-configuration)
  - [Configuration Dependencies](#configuration-dependencies)
  - [Improvement Areas](#improvement-areas)
  - [Next Steps](#next-steps)
  - [References](#references)

## Configuration Locations

### 1. Aspire Configuration
**File**: `code/MatureWeb/MatureWeb.AppHost/Program.cs`
```csharp
builder.AddContainer(name: "azuresqledge", image: "mcr.microsoft.com/azure-sql-edge")
    .WithLifetime(ContainerLifetime.Persistent);
```
**Usage**: Primary container configuration for the application  
**Status**: Incomplete - Missing essential parameters

### 2. Database Connection Settings
**File**: `code/MatureWeb/Northwind.Shared/Configuration/DatabaseConnectionSettings.cs`
```csharp
public string DataSource { get; set; } = "tcp:127.0.0.1,1433";
```
**Usage**: Default connection configuration  
**Status**: Working - Used across application

### 3. Test Environment Setup
**File**: `docs/general/development/testing/test-environment-setup.md`
```powershell
# PS: repo-root/.
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrongPassword' -p 1433:1433 `
    --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
```
**Usage**: Manual container setup instructions  
**Status**: Working - Complete configuration

### 4. Example Configuration Files
**Files**:
- `code/MatureWeb/appsettings.Example.json`
- `code/MatureWeb/Northwind.Mvc/appsettings.Example.json`
```json
{
  "DatabaseConnection": {
    "DataSource": "tcp:127.0.0.1,1433"
  }
}
```
**Usage**: Template for application configuration
**Status**: Working - Provides example structure

### 5. Test Configuration
**File**: `code/MatureWeb/Northwind.UnitTests/InMemoryTestBase.cs`
```csharp
{ "DatabaseConnection:DataSource", "tcp:127.0.0.1,1433" }
```
**Usage**: Test database configuration
**Status**: Working - Used in test environment

### 6. Database Context Configuration
**File**: `code/MatureWeb/Northwind.DataContext/NorthwindContext.cs`
**Usage**: Database context setup and connection management
**Status**: Working - Handles connection string building

## Configuration Dependencies

### User Secrets
- Required for database credentials
- Used in both development and test environments
- Referenced in multiple configuration files

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`
- Database credentials (`Database:MY_SQL_USR`, `Database:MY_SQL_PWD`)

## Improvement Areas

1. **Configuration Consistency**
   - Different connection string formats across files
   - Varying approaches to credential management
   - Inconsistent container setup methods

2. **Security Considerations**
   - Credential management in tests
   - Container security settings
   - Connection string security

3. **Documentation Gaps**
   - Container initialization process
   - Configuration override hierarchy
   - Environment-specific settings

## Next Steps

1. **Standardization**
   - [ ] Unify connection string format
   - [ ] Standardize container configuration
   - [ ] Create consistent credential management

2. **Security Enhancement**
   - [ ] Review credential handling
   - [ ] Audit security settings
   - [ ] Document security best practices

3. **Documentation**
   - [ ] Create configuration guide
   - [ ] Document override hierarchy
   - [ ] Add security guidelines 

## References

- [SQL Edge Container Management](./sql-edge-container-management.md) - Main management strategy and implementation plan
- [Test Environment Setup](./testing/test-environment-setup.md) - Current test environment configuration
- [Database Connection Settings](../../code/MatureWeb/Northwind.Shared/Configuration/DatabaseConnectionSettings.cs) - Core connection settings implementation
- [Aspire Host Configuration](../../code/MatureWeb/MatureWeb.AppHost/Program.cs) - Aspire container configuration 