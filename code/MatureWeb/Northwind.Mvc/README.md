# Northwind MVC Application

ASP.NET Core MVC application for the Northwind database.

## Table of Contents
- [Northwind MVC Application](#northwind-mvc-application)
  - [Table of Contents](#table-of-contents)
  - [Setup](#setup)
  - [Configuration](#configuration)
  - [Features](#features)
  - [Development](#development)
  - [Project Structure](#project-structure)

## Setup

1. Clone the repository
2. Navigate to the project directory:
   ```
   cd code/MatureWeb/Northwind.Mvc
   ```
3. Create your settings files:
   - Copy `appsettings.Example.json` to `appsettings.json`
   - Copy `appsettings.Development.Example.json` to `appsettings.Development.json`
4. Initialize and set up user secrets for development:
   ```
   dotnet user-secrets init
   dotnet user-secrets set "Database:MY_SQL_USR" "sa"
   dotnet user-secrets set "Database:MY_SQL_PWD" "your_password"
   ```
   This keeps sensitive information out of source control and configuration files.

5. Restore dependencies:
   ```
   dotnet restore
   ```
6. Run the application:
   ```
   dotnet run --launch-profile https
   ```

   Alternative: When in the parent MatureWeb directory:
   ```
   dotnet run --project Northwind.Mvc --launch-profile https
   ```
7. Access the application at:
   ```
   https://localhost:5020/
   https://localhost:5021/
   ```

## Configuration

The application uses SQLite by default with the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "DataSource=app.db;Cache=Shared"
}
```

## Features

- Identity authentication and authorization
- Product catalog
- Order management

## Development

Make sure to update your database during development:

```
dotnet ef database update
```

## Project Structure

- `Areas/Identity`: Authentication and user management
- `Controllers`: Application controllers
- `Data`: Database context and migrations
- `Models`: Data models and view models
- `Views`: Application views
- `wwwroot`: Static files (CSS, JS, images)