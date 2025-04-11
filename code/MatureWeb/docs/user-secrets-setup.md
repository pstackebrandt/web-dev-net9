# Setting Up User Secrets for Development

A guide for new team members on configuring database credentials securely using .NET User Secrets.

## Table of Contents

- [Setting Up User Secrets for Development](#setting-up-user-secrets-for-development)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Prerequisites](#prerequisites)
  - [Setup Steps](#setup-steps)
  - [Verifying Your Configuration](#verifying-your-configuration)
  - [Troubleshooting](#troubleshooting)

## Introduction

MatureWeb uses .NET's User Secrets feature to manage sensitive configuration data during
development. This keeps credentials out of source control while allowing your local development
environment to connect to databases.

## Prerequisites

- .NET SDK 6.0 or later
- Access to database credentials (contact your team lead)

## Setup Steps

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

4. For test environment credentials (optional):

   ```bash
   dotnet user-secrets set "Test:Database:MY_SQL_USR" "your_test_username" --project Northwind.DataContext
   dotnet user-secrets set "Test:Database:MY_SQL_PWD" "your_test_password" --project Northwind.DataContext
   ```

## Verifying Your Configuration

To verify your secrets are properly set up:

```bash
dotnet user-secrets list --project Northwind.DataContext
```

You should see output like:

```
Database:MY_SQL_USR = your_actual_username
Database:MY_SQL_PWD = your_actual_password
Test:Database:MY_SQL_USR = your_test_username
Test:Database:MY_SQL_PWD = your_test_password
```

## Troubleshooting

- **Secrets not loading**: Ensure you've initialized secrets in the correct project
- **Connection failures**: Verify credentials are correct
- **IDE not recognizing secrets**: Restart your IDE
- **Missing user-secrets-id**: Check if the project file contains a UserSecretsId element

If you need further assistance, contact the development team lead.