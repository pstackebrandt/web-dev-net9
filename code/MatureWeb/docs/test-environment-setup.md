# Test Environment Setup

Quick setup guide for the Northwind test environment.

## Table of Contents

- [Test Environment Setup](#test-environment-setup)
  - [Table of Contents](#table-of-contents)
  - [Docker Configuration](#docker-configuration)
  - [User Secrets Configuration](#user-secrets-configuration)
  - [Troubleshooting](#troubleshooting)

## Docker Configuration

The project uses Azure SQL Edge in Docker for database tests:

- **Create new container**:
  ```bash
  docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrongPassword' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
  ```

- **Start existing container**:
  ```bash
  docker start azuresqledge
  ```

- **Verify connection**:
  ```bash
  # Check container logs
  docker logs azuresqledge
  
  # Check port mapping
  docker port azuresqledge
  
  # Ensure port 1433 is mapped correctly
  ```

## User Secrets Configuration

Tests using `DatabaseTestBase` require database credentials configured as user secrets:

1. **Initialize user secrets** (if needed):
   ```bash
   dotnet user-secrets init --project Northwind.UnitTests
   ```

2. **Set database credentials**:
   ```bash
   dotnet user-secrets set "Database:MY_SQL_USR" "sa" --project Northwind.UnitTests
   dotnet user-secrets set "Database:MY_SQL_PWD" "YourStrongPassword" --project Northwind.UnitTests
   ```

3. **Verify configuration**:
   ```bash
   dotnet user-secrets list --project Northwind.UnitTests
   ```

> **Important**: Use the same password in both Docker container and user secrets configurations.

## Troubleshooting

- **SQL Server not starting**: Check Docker logs with `docker logs azuresqledge`
- **Connection failures**: Verify port 1433 is mapped with `docker port azuresqledge`
- **Missing user secrets**: Confirm credentials match your Docker container setup
- **Test failures**: See [Test Configuration Guide](./test-configuration-guide.md)