# SQL Edge Container Management

Brief guide for managing Azure SQL Edge container setup and configuration in the project.

## Table of Contents
- [SQL Edge Container Management](#sql-edge-container-management)
  - [Table of Contents](#table-of-contents)
  - [Current State](#current-state)
    - [Container Setup](#container-setup)
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

> **Note**: For a detailed audit of all SQL Edge configurations in the codebase, see the [SQL Edge Configuration Audit](./sql-edge-configuration-audit.md) document.

### Container Setup
Currently, the Azure SQL Edge container setup is handled in two different ways:

1. **Manual Setup (Working)**
   ```powershell
   # PS: repo-root/.
   docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrongPassword' -p 1433:1433 --name azuresqledge `
       -d mcr.microsoft.com/azure-sql-edge
   ```
   - Creates container with proper initialization
   - Sets required EULA acceptance
   - Configures SA password
   - Sets up port mapping

2. **Aspire Configuration (Incomplete)**
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