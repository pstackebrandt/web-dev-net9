# SQL Edge Container Management

Brief guide for managing Azure SQL Edge container setup and configuration in the project.

> **Note**: For the architectural decision regarding container management strategy, see [ADR-0001](../architecture/decisions/0001-sql-edge-container-management.md).

## Table of Contents
- [SQL Edge Container Management](#sql-edge-container-management)
  - [Table of Contents](#table-of-contents)
  - [Current State](#current-state)
    - [Container Setup](#container-setup)
    - [Database Installation](#database-installation)
      - [Manual Setup (Working)](#manual-setup-working)
      - [Database Installation (Working)](#database-installation-working)
    - [Port Configuration Notes](#port-configuration-notes)
      - [Aspire Configuration (Incomplete)](#aspire-configuration-incomplete)
    - [Known Issues](#known-issues)
  - [Improvement Tasks](#improvement-tasks)
    - [1. Aspire Integration Enhancement](#1-aspire-integration-enhancement)
    - [2. Container Initialization Strategy](#2-container-initialization-strategy)
    - [3. Test Environment Setup](#3-test-environment-setup)
    - [4. Documentation Consolidation](#4-documentation-consolidation)
    - [5. Solution-wide Consistency](#5-solution-wide-consistency)
  - [Decision Points](#decision-points)
  - [Quick Start Guide for Aspire Testing](#quick-start-guide-for-aspire-testing)
    - [Prerequisites](#prerequisites)
    - [Initial Setup Steps](#initial-setup-steps)
    - [Verification Steps](#verification-steps)
    - [Troubleshooting](#troubleshooting)
  - [Next Steps](#next-steps)
  - [References](#references)

## Current State

> **Note**: For a detailed audit of all SQL Edge configurations in the codebase,
> see the [SQL Edge Configuration Audit](./sql-edge-configuration-audit.md) document.

### Container Setup
Currently, the Azure SQL Edge container setup is handled in two different ways:

### Database Installation
After setting up the container, the Northwind database can be installed using the provided SQL script.

----;

#### Manual Setup (Working)

**Get sql server edge container running**

```powershell
docker pull mcr.microsoft.com/azure-sql-edge:latest
```

**Run the container**
Recommended by Price:
```powershell
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=YourStrongPassword' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
```

> **Note**: `--cap-add SYS_PTRACE` grants the container process tracing capabilities, enabling debugging tools and performance
> profilers to work properly within the SQL Edge container. This capability is commonly needed for database diagnostics
> and development scenarios.
>
> ⚠️ **Warning**: Do not use `--cap-add SYS_PTRACE` in production environments as it increases security risks by allowing
 process inspection that could potentially expose sensitive data.

**Verification**
Container status check shows successful creation:
```powershell
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" | findstr -i azuresqledge
```
Results:
```
azuresqledge            Up 39 minutes   0.0.0.0:1433->1433/tcp, [::]:1433->1433/tcp
```
- Container is running with proper port mapping
- Standard SQL Server port (1433) accessible from localhost

- Creates container with proper initialization
- Sets required EULA acceptance
- Configures SA password
- Sets up port mapping

#### Database Installation (Working)

**Using SQL Server Extension in VS Code/Cursor** (Recommended)

1. **Connect to Container**
   - Open SQL Server extension panel
   - Verify connection to "Azure SQL Edge in Docker"

2. **Execute Installation Script**
   - Right-click on your Azure SQL Edge connection
   - Select "New Query"
   - Open the SQL script file:
     - Press Ctrl+O to open file
     - Navigate to: `scripts/sql-scripts/Northwind4AzureSqlEdgeDocker.sql`
     - Click Open
   - Execute the script: Press Ctrl+Shift+E or click Execute button

3. **Verification**
   - Northwind database appears in connection tree
   - Tables folder contains Categories, Products, Orders, etc.
   - Database is fully populated with sample data

> **Note**: The script is 2MB in size and may take 1-2 minutes to execute completely.

**Terminal Installation Issues**

Terminal-based installation using `docker exec` with piped SQL content may fail due to:
- Binary data segments in the SQL script file
- Character encoding issues during pipe operations
- Terminal limitations with large script files

The SQL Server extension method is more reliable as it handles the script execution properly within the SQL Server context.

**Database Removal and Reinstallation**

To remove and reinstall the database:

1. **Remove Database**:
   
   **Option A: Using SQL Server Extension (Recommended)**
   - Right-click on "Northwind" database in connection tree
   - Select "Delete Database" or use New Query with: `DROP DATABASE IF EXISTS Northwind;`
   
   **Option B: Using Command Line**
   ```powershell
   docker exec -it azuresqledge /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "my-sa-password" -Q "DROP DATABASE IF EXISTS Northwind;"
   ```
   > **Note**: Replace "my-sa-password" with your actual SA password (check user secrets if needed)

2. **Reinstall Database**:
   - Follow the "Database Installation" steps above

### Port Configuration Notes
The Azure SQL Edge container shows some interesting behavior regarding port configuration:

1. **Exposed Ports**
   - **1433/tcp**: Standard SQL Server port for database connections
   - **1401/tcp**: Extensibility Framework port for R, Python, and ML services
   - Only port 1433 is needed for basic database operations

2. **Internal vs External Ports**
   - The container logs show internal service (launchpadd) attempting to connect on port 1431
   - However, the SQL Server instance itself listens on the standard port 1433
   - Both port mappings (1431 or 1433) work, but 1433 is recommended

3. **Best Practice**
   - Use the standard SQL Server port 1433 for external mapping
   - Expose port 1401 only if using extensibility features (R, Python, ML)
   - Ignore launchpadd warnings about port 1431 as they don't affect functionality
   - This maintains compatibility with standard SQL Server tools and configurations

4. **Connection String**
   ```
   Server=localhost,1433;Database=master;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True
   ```

#### Aspire Configuration (Incomplete)
   ```csharp
   builder.AddContainer(name: "azuresqledge", image: "mcr.microsoft.com/azure-sql-edge")
       .WithLifetime(ContainerLifetime.Persistent);
   ```
- Missing essential configuration parameters
- No EULA acceptance
- No SA password
- No port mapping
- Container is set as persistent

### Known Issues

1. **Aspire Integration**
   - Aspire doesn't fully initialize the SQL Edge container
   - No validation of container readiness beyond existence
   - Missing error handling for uninitialized container

2. **Test Environment**
   - Test projects assume existing, initialized container
   - No consistent approach for test database setup
   - Documentation spread across multiple files

3. **Documentation**
   - Setup instructions in multiple locations
   - Inconsistent container management approaches
   - No clear guidance for development vs. test scenarios

## Improvement Tasks

### 1. Aspire Integration Enhancement
- [ ] Add proper container initialization parameters to Aspire configuration
- [ ] Implement container readiness validation
- [ ] Add proper error handling and user feedback
- [ ] Test container lifecycle management
- [ ] Document Aspire-specific setup and configuration

### 2. Container Initialization Strategy
- [ ] Evaluate need for Aspire-managed initialization
- [ ] Design consistent initialization approach
- [ ] Create initialization scripts if needed
- [ ] Document initialization process
- [ ] Add validation and error handling

### 3. Test Environment Setup
- [ ] Define test container management strategy
- [ ] Update test project configurations
- [ ] Create test-specific setup documentation
- [ ] Implement test database initialization
- [ ] Add test environment validation

### 4. Documentation Consolidation
- [ ] Consolidate setup instructions
- [ ] Create clear development workflow documentation
- [ ] Document test environment setup
- [ ] Add troubleshooting guide
- [ ] Update README with quick start information

### 5. Solution-wide Consistency
- [ ] Audit all SQL Edge container references
- [ ] Standardize configuration approaches
- [ ] Update connection string management
- [ ] Implement consistent error handling
- [ ] Add logging and monitoring

## Decision Points

1. **Initialization Responsibility**
   - [ ] Decide if Aspire should handle initialization
   - [ ] Document decision and rationale
   - [ ] Plan implementation based on decision

2. **Development Workflow**
   - [ ] Define preferred development setup process
   - [ ] Document manual vs. automated steps
   - [ ] Create developer quick start guide

3. **Testing Strategy**
   - [ ] Decide on test database approach
   - [ ] Define test data management
   - [ ] Document test environment setup

## Quick Start Guide for Aspire Testing

### Prerequisites
1. Docker Desktop installed and running
2. .NET 9 SDK installed
3. Azure SQL Edge container not running (if exists, will be managed by Aspire)

### Initial Setup Steps
1. **Set up SQL Edge container manually first time**
   ```powershell
   # Create and start the container
   docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrongPassword' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
   
   # Verify container is running
   docker ps | findstr azuresqledge
   ```

2. **Configure User Secrets**
   ```powershell
   # Navigate to the MVC project directory
   cd code/MatureWeb/Northwind.Mvc
   
   # Set up user secrets
   dotnet user-secrets set "Database:MY_SQL_USR" "sa"
   dotnet user-secrets set "Database:MY_SQL_PWD" "YourStrongPassword"
   ```

3. **Run Aspire Application**
   ```powershell
   # Navigate to the AppHost project
   cd ../MatureWeb.AppHost
   
   # Run the application
   dotnet run
   ```

### Verification Steps
1. Check Aspire dashboard (typically at http://localhost:15888)
2. Verify SQL Edge container status
3. Check MVC application connectivity

### Troubleshooting
- If Aspire fails to start container: Use `docker logs azuresqledge`
- If database connection fails: Verify user secrets match container password
- If port conflicts: Ensure no other SQL Server instance is using port 1433
- If database installation fails via terminal: Use SQL Server extension method instead

## Next Steps

1. **Immediate Actions**
   - [ ] **Manual Container Setup and Documentation**
      - [ ] Create container with required parameters
      ```powershell
      docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrongPassword' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
      ```
      - [ ] Verify container is running and accessible
      - [ ] Document actual used configuration values
      - [ ] Update quick start guide with verified values
   - [x] Create quick start guide for Aspire testing
   - [ ] Test Aspire with existing container
      - [ ] Verify container management
      - [ ] Check connection string handling
      - [ ] Monitor resource usage
   - [ ] Document any issues encountered with Aspire
      - [ ] Container lifecycle issues
      - [ ] Connection problems
      - [ ] Configuration conflicts
   - [ ] Create initial Aspire configuration backup
      - [ ] Save working configuration
      - [ ] Document any manual interventions needed

2. **Short-term Goals**
   - Consolidate existing documentation
   - Create consistent setup instructions
   - Improve error handling and feedback

3. **Long-term Goals**
   - Implement chosen initialization strategy
   - Update all related documentation
   - Create comprehensive testing guide

## References

- [SQL Edge Configuration Audit](./sql-edge-configuration-audit.md) - Detailed audit of all SQL Edge configurations
- [Test Environment Setup](./testing/test-environment-setup.md)
- [Aspire Configuration](../../code/MatureWeb/MatureWeb.AppHost/Program.cs)
- [Database Connection Settings](../../code/MatureWeb/Northwind.Shared/Configuration/DatabaseConnectionSettings.cs)