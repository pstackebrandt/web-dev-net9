# ADR-0001: SQL Edge Container Management Strategy

## Status

Accepted

## Context

The project uses Azure SQL Edge for database functionality and needs to support both:
- Development with Aspire orchestration
- Independent project development without Aspire

## Decision

We will use Aspire to manage the SQL Edge container with the following approach:

1. **Primary Container Management**
   - Aspire will be responsible for creating and managing the SQL Edge container
   - Container will be configured with persistent lifetime
   - Standard port (1433) and configuration will be used

2. **Dual-Mode Operation**
   - Projects can run in two modes:
     a. Via Aspire orchestration (primary development mode)
     b. Independently, connecting to the persistent container

3. **Configuration Strategy**
   - Connection strings will be configured to work in both scenarios
   - Standard SQL Server connection patterns will be used
   - Container settings (ports, credentials) will be consistent

## Consequences

### Positive
- Single source of truth for container configuration (Aspire)
- Consistent development environment across team
- Flexibility to run projects with or without Aspire
- Simplified container lifecycle management
- Better integration with Aspire's monitoring and management

### Negative
- Initial container setup requires Aspire run
- Team needs to understand both operational modes
- Need to maintain configuration compatibility for both scenarios

## Implementation Notes

1. Container Configuration:
   ```csharp
   builder.AddContainer("azuresqledge", "mcr.microsoft.com/azure-sql-edge")
       .WithLifetime(ContainerLifetime.Persistent)
       .WithEnvironment("ACCEPT_EULA", "1")
       .WithEnvironment("MSSQL_PID", "Developer")
       .WithEnvironment("SA_PASSWORD", "YourStrongPassword")
       .WithPortBinding(1433, 1433);
   ```

2. Connection String Pattern:
   ```
   Server=localhost,1433;Database=YourDatabase;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True
   ```

## Related Documents
- [SQL Edge Container Management](../../development/sql-edge-container-management.md)