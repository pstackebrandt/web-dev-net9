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
4. Restore dependencies:
   ```
   dotnet restore
   ```
5. Run the application:
   ```
   dotnet run --launch-profile https
   ```
6. Access the application at:
   ```
   https://localhost:5020/
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