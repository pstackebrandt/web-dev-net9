# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is **web-dev-net9**, a comprehensive .NET 9 web development training project based on "Real-World Web Development with .NET 9" by M.J. Price. The project demonstrates modern ASP.NET Core MVC patterns using the Northwind database and includes multiple related applications.

**Key Technologies:**
- .NET 9.0 (SDK 9.0.202)
- ASP.NET Core MVC 9.0.5
- Entity Framework Core 9.0.5
- SQL Server (via Docker with SQL Edge)
- xUnit for testing
- Bootstrap for UI

## Project Structure

```
code/MatureWeb/
├── MatureWeb.sln                    # Main solution file
├── global.json                      # .NET SDK version specification
├── Directory.Packages.props         # Centralized package version management
├── Northwind.EntityModels/          # EF Core entity models
├── Northwind.DataContext/           # EF Core DbContext and configuration
├── Northwind.Shared/                # Shared utilities and configuration
├── Northwind.Mvc/                   # Main MVC web application
├── ExploringBootstrap/              # Bootstrap demonstration project
├── Northwind.UnitTests/             # Unit tests
├── MatureWeb.AppHost/               # .NET Aspire orchestration
└── MatureWeb.ServiceDefaults/       # Service defaults for .NET Aspire
```

## Development Commands

### Essential Commands (run from `code/MatureWeb/` directory)

```bash
# Build the entire solution
dotnet build

# Run the main MVC application
dotnet run --project Northwind.Mvc

# Run unit tests
dotnet test

# Run specific project
dotnet run --project ExploringBootstrap

# Restore packages
dotnet restore

# Run with .NET Aspire orchestration
dotnet run --project MatureWeb.AppHost
```

### Database Commands

```bash
# Initialize user secrets for database credentials
dotnet user-secrets init --project Northwind.DataContext

# Set database credentials (development)
dotnet user-secrets set "Database:MY_SQL_USR" "your_username" --project Northwind.DataContext
dotnet user-secrets set "Database:MY_SQL_PWD" "your_password" --project Northwind.DataContext

# Entity Framework migrations (if needed)
dotnet ef migrations add MigrationName --project Northwind.DataContext
dotnet ef database update --project Northwind.DataContext
```

## Architecture Overview

### Core Projects
- **Northwind.EntityModels**: Contains all EF Core entity classes (Category, Product, Order, etc.)
- **Northwind.DataContext**: DbContext, database configuration, and connection management
- **Northwind.Shared**: Shared configuration classes and utilities
- **Northwind.Mvc**: Main web application with controllers, views, and models

### Key Architectural Patterns
- **Layered Architecture**: Clear separation between data, business, and presentation layers
- **Repository Pattern**: Implemented through EF Core DbContext
- **Dependency Injection**: Used throughout for service registration
- **Configuration Pattern**: User secrets for development, environment variables for production
- **Clean Architecture**: Domain models separated from infrastructure concerns

## Configuration Management

### Database Configuration
- **Development**: Uses User Secrets for credentials (`Database:MY_SQL_USR`, `Database:MY_SQL_PWD`)
- **Connection**: SQL Server via Docker (default: `tcp:127.0.0.1,1433`)
- **Database**: Northwind sample database
- **Security**: TrustServerCertificate=true for development

### Configuration Files
- `appsettings.json`: Base configuration (no secrets)
- `appsettings.Development.json`: Development overrides
- `appsettings.Testing.json`: Test environment settings
- User Secrets: Sensitive development credentials
- Environment Variables: Production credentials

## Development Guidelines

### Code Standards
- Follow C# naming conventions (PascalCase for public members, camelCase for parameters)
- Use `_camelCase` for private fields
- Enable nullable reference types (`<Nullable>enable</Nullable>`)
- Use implicit usings (`<ImplicitUsings>enable</ImplicitUsings>`)
- Keep classes focused and under 500 lines

### Testing
- Write unit tests for business logic
- Use xUnit testing framework
- Test configuration classes and data access patterns
- Place tests in `Northwind.UnitTests` project

### Entity Framework
- Use Code First approach with migrations
- Define entities in `Northwind.EntityModels`
- Configure DbContext in `Northwind.DataContext`
- Use proper connection string management

## Package Management

### Centralized Package Versions
All package versions are managed in `Directory.Packages.props`:
- **MicrosoftPackageVersion**: 9.0.5 (used for EF Core, ASP.NET Core)
- **EFCoreVersion**: Inherits from MicrosoftPackageVersion
- **Testing**: xUnit 2.9.3, coverlet.collector 6.0.4

### Key Dependencies
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.AspNetCore.Identity
- Microsoft.Extensions.Configuration.UserSecrets
- xunit and xunit.runner.visualstudio

## Common Development Tasks

### Setting Up Development Environment
1. Ensure .NET 9.0.202 SDK is installed
2. Configure database credentials using User Secrets
3. Copy `appsettings.Example.json` to `appsettings.json`
4. Run `dotnet restore` to restore packages
5. Run `dotnet build` to verify setup

### Adding New Features
1. Create entities in `Northwind.EntityModels` if needed
2. Add DbSet properties to NorthwindContext
3. Create controllers and views in `Northwind.Mvc`
4. Add corresponding unit tests
5. Update documentation

### Running Different Applications
- **Main App**: `dotnet run --project Northwind.Mvc`
- **Bootstrap Demo**: `dotnet run --project ExploringBootstrap`
- **With Aspire**: `dotnet run --project MatureWeb.AppHost`

## Documentation

### Available Documentation
- `code/MatureWeb/README.md`: Project-specific setup and configuration
- `docs/README.md`: General documentation overview
- `docs/general/development/`: Development guidelines and best practices
- `docs/general/architecture/`: Architecture decisions and diagrams

### Documentation Guidelines
- Solution-wide docs go in `docs/` folder
- Project-specific docs go in respective project folders
- Use Markdown for all documentation
- Keep documentation up-to-date with code changes

### Documentation Standards
Follow [AI Documentation Standards](docs/general/guidelines/ai-documentation-standards.md) for detailed formatting rules.

**Priority Overrides**:
- **Always use TOCs** for architecture documents, audits, and investigation reports
- **Always use TOCs** for any document with 3+ sections (strict enforcement)

## Environment Setup

**Prerequisites:**
- .NET 9.0.202 SDK (specified in `global.json`)
- Docker (for SQL Server Edge container)
- Visual Studio 2022 or VS Code with C# extension

**Windows Development:**
- Use PowerShell or Command Prompt for .NET CLI commands
- Docker Desktop for container management
- SQL Server Management Studio (optional) for database administration

## Troubleshooting

### Common Issues
- **Build errors**: Ensure .NET 9.0.202 SDK is installed
- **Database connection**: Verify User Secrets are configured correctly
- **Package conflicts**: Check `Directory.Packages.props` for version conflicts
- **Missing dependencies**: Run `dotnet restore` in solution directory

### Database Issues
- **Connection failures**: Check Docker container is running
- **Authentication errors**: Verify User Secrets configuration
- **Migration issues**: Ensure EF Core tools are installed globally

## Security Notes
- Never commit database credentials to source control
- Use User Secrets for development credentials
- Use environment variables for production deployment
- Follow secure configuration practices in documentation