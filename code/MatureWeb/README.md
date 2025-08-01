# Mature Web

## Overview

This project is a training and example mvc web application
that uses the Northwind database.

It intends to follow the book "Real-World Web Development with .NET 9" by M.J.Price.
But it will also contain my own ideas and common best practices.

## Technologies Used

- C#, .NET 9
- ASP.NET Core MVC
- Entity Framework Core
- Edge SQL Server
- Docker

## Used SDK
see global.json

## Configuration and Credentials

### Development Setup

1. Configure user secrets for storing database credentials:
   ```bash
   # Initialize user secrets for the project
   dotnet user-secrets init --project Northwind.DataContext
   
   # Set your database credentials
   dotnet user-secrets set "Database:MY_SQL_USR" "your_username" --project Northwind.DataContext
   dotnet user-secrets set "Database:MY_SQL_PWD" "your_password" --project Northwind.DataContext
   ```

2. For detailed instructions, see [User Secrets Setup Guide](docs/user-secrets-setup.md)

3. Copy `appsettings.Example.json` to `appsettings.json` if `appsettings.json` doesn't exist (no credentials needed in this file)

### Configuration Options

#### Database Section
- **MY_SQL_USR**: Username for SQL Server authentication
- **MY_SQL_PWD**: Password for SQL Server authentication

#### DatabaseConnection Section
- **DataSource**: SQL Server connection string (default: tcp:127.0.0.1,1433)
- **InitialCatalog**: Database name to connect to (default: Northwind)
- **TrustServerCertificate**: Whether to trust the server certificate without validation (default: true)
- **MultipleActiveResultSets**: Enables multiple active result sets (MARS) (default: true)
- **ConnectTimeout**: Connection timeout in seconds
  - Default: 8 seconds for development
  - 1 second for testing environment

#### Environment-Specific Configuration
- Development environment uses the base settings in `appsettings.json`
- Testing environment overrides certain settings via `appsettings.Testing.json`
- Sensitive credentials are stored in User Secrets during development, not in configuration files
- The application automatically loads configuration in this order:
  1. Base settings from `appsettings.json`
  2. Environment overrides from `appsettings.{Environment}.json`
  3. User Secrets (in Development environment only)
  4. Environment variables (used in production)

For comprehensive security and configuration guidelines, see [Configuration Best Practices](docs/configuration-best-practices.md).