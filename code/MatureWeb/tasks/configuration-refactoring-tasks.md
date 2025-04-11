# Configuration Refactoring Tasks

A checklist of tasks required to implement best practices for configuration management in the MatureWeb project.

## Table of Contents

- [Configuration Refactoring Tasks](#configuration-refactoring-tasks)
  - [Table of Contents](#table-of-contents)
  - [Core Configuration Structure](#core-configuration-structure)
  - [Credentials Management](#credentials-management)
  - [Environment-Specific Configuration](#environment-specific-configuration)
  - [Code Changes](#code-changes)
  - [Documentation](#documentation)
  - [Why User Secrets for Development](#why-user-secrets-for-development)

## Core Configuration Structure

- [x] Update `appsettings.json` to include full configuration structure:
  - [x] Add `DatabaseConnection` section to match structure in `appsettings.Testing.json`
  - [x] Replace sensitive credentials with placeholder values
  - [ ] Ensure all configuration options are documented with comments \
  (Comments not allowed in JSON, added documentation to README instead)

## Credentials Management

- [x] Set up User Secrets for development:
  - [x] Initialize user secrets for relevant projects
  ```bash
  dotnet user-secrets init --project code/MatureWeb/Northwind.DataContext
  ```
  - [x] Move database credentials to user secrets
  ```bash
  dotnet user-secrets set "Database:MY_SQL_USR" "sa" \
    --project code/MatureWeb/Northwind.DataContext
  dotnet user-secrets set "Database:MY_SQL_PWD" "yourpassword" \
    --project code/MatureWeb/Northwind.DataContext
  ```
  - [x] Update example files to show placeholder values
  - [x] Create documentation for new team members to set up secrets

## Environment-Specific Configuration

- [ ] Update `appsettings.Testing.json` to only contain overrides:
  - [ ] Remove duplicate settings that match base file values
  - [ ] Keep different timeout setting (`ConnectTimeout: 1`)
  - [ ] Remove database credentials (will be in User Secrets)

## Code Changes

- [ ] Update `NorthwindContext.cs` to load secrets in development:
  - [ ] Add environment-specific configuration loading
  - [ ] Explicitly merge settings from `DatabaseConnection` section
  - [ ] Add appropriate error handling for missing configuration

- [ ] Update `NorthwindContextExtensions.cs`:
  - [ ] Get connection parameters from configuration instead of hardcoding
  - [ ] Use strongly-typed settings objects consistently

- [ ] Create unit tests for configuration:
  - [ ] Test that environment-specific overrides work correctly
  - [ ] Test behavior with missing credentials
  - [ ] Verify secrets loading behavior in development

## Documentation

- [ ] Add README section on configuration management
- [ ] Document credential management for development and production
- [ ] Update example configuration files

## Why User Secrets for Development

User Secrets was chosen because:

1. **Simplicity**: Built into .NET SDK, no external dependencies
2. **Security**: Stored outside the project directory, not in source control
3. **Development-focused**: Designed specifically for local development
4. **Seamless integration**: Works automatically with ASP.NET Core configuration
5. **No code changes**: Same code works in all environments

For production, a more robust solution like environment variables or Azure Key
Vault would be recommended, but User Secrets provides the simplest secure
option for development without adding complexity.