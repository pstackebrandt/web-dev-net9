# Mature Web

## Overview

This project is a training and example mvc web application
that uses the Northwind database.

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

1. Copy `appsettings.Example.json` to `appsettings.json`
and fill in your actual credentials:
   ```json
   {
     "Database": {
       "MY_SQL_USR": "your_username",
       "MY_SQL_PWD": "your_password"
     }
   }
   ```
2. Add `appsettings.json` to your `.gitignore` file to prevent committing real credentials

## Security Best Practices

- Never commit real credentials to source control
- Use different credentials for development and production
- Regularly rotate credentials
- Use the principle of least privilege for database access