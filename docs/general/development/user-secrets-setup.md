# Setting Up User Secrets for Development

A guide for new team members on configuring database credentials securely using .NET User Secrets.

## Table of Contents

- [Setting Up User Secrets for Development](#setting-up-user-secrets-for-development)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Prerequisites](#prerequisites)
  - [Setup Steps for Main Application](#setup-steps-for-main-application)
  - [Setup Steps for Tests](#setup-steps-for-tests)
  - [Managing User Secrets](#managing-user-secrets)
  - [Verifying Your Configuration](#verifying-your-configuration)
  - [Troubleshooting](#troubleshooting)

## Introduction

MatureWeb uses .NET's User Secrets feature to manage sensitive configuration data during
development. This keeps credentials out of source control while allowing your local development
environment to connect to databases.

## Prerequisites

- .NET SDK 6.0 or later
- Access to database credentials (contact your team lead)

## Setup Steps for Main Application

1. Open a terminal in the solution root directory (`code/MatureWeb/`)

2. Initialize user secrets for the DataContext project:

   ```bash
   dotnet user-secrets init --project Northwind.DataContext
   ```

3. Add your database credentials:

   ```bash
   dotnet user-secrets set "Database:MY_SQL_USR" "your_actual_username" --project Northwind.DataContext
   dotnet user-secrets set "Database:MY_SQL_PWD" "your_actual_password" --project Northwind.DataContext
   ```

   Replace `your_actual_username` and `your_actual_password` with the actual database credentials.

## Setup Steps for Tests

The test project uses the same connection settings and credentials as the main application, but must be configured separately:

1. Open a terminal in the solution root directory (`code/MatureWeb/`)

2. Initialize user secrets for the test project:

   ```bash
   dotnet user-secrets init --project Northwind.UnitTests
   ```

3. Add your database credentials for tests:

   ```bash
   dotnet user-secrets set "Database:MY_SQL_USR" "your_actual_username" --project Northwind.UnitTests
   dotnet user-secrets set "Database:MY_SQL_PWD" "your_actual_password" --project Northwind.UnitTests
   ```

   For development and testing, you can use the same credentials as the main application.

4. If using different credentials for testing (recommended for production databases):

   ```bash
   dotnet user-secrets set "Database:MY_SQL_USR" "your_test_username" --project Northwind.UnitTests
   dotnet user-secrets set "Database:MY_SQL_PWD" "your_test_password" --project Northwind.UnitTests
   ```

## Managing User Secrets

For development, you can manage your user secrets with the following commands:

```bash
# View all secrets for a project
dotnet user-secrets list --project Northwind.Mvc

# Remove a specific secret
dotnet user-secrets remove "Key:Name" --project Northwind.Mvc

# Clear all secrets for a project
dotnet user-secrets clear --project Northwind.Mvc
```

When working from a different directory, specify the full path to the project:

```bash
# From the solution root (code/MatureWeb/)
dotnet user-secrets list --project Northwind.Mvc

# From the repository root
dotnet user-secrets list --project code/MatureWeb/Northwind.Mvc
```

Secrets are stored in your user profile in:
- Windows: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
- macOS/Linux: `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`

The `user_secrets_id` is a GUID that's added to your project file when you initialize user secrets.

## Verifying Your Configuration

To verify your secrets are properly set up:

```bash
dotnet user-secrets list --project Northwind.DataContext
```

You should see output like:

```
Database:MY_SQL_USR = your_actual_username
Database:MY_SQL_PWD = your_actual_password
```

Similarly, verify test secrets:

```bash
dotnet user-secrets list --project Northwind.UnitTests
```

## Troubleshooting

- **Secrets not loading**: Ensure you've initialized secrets in the correct project
- **Connection failures**: Verify credentials are correct
- **IDE not recognizing secrets**: Restart your IDE
- **Missing user-secrets-id**: Check if the project file contains a UserSecretsId element
- **Test failures**: Ensure you've set up secrets for the test project
- **Different configurations**: If a test is failing only on your machine, check that your user
  secrets match the expected values

If you need further assistance, contact the development team lead.