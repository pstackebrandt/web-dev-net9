using System.ComponentModel;
using Aspire.Hosting;

// Builder for the distributed application
// This is the entry point for .NET Aspire application configuration
var builder = DistributedApplication.CreateBuilder(args);

// Configure Azure SQL Edge container as a persistent resource
// This container will be used as the database for the application
IResourceBuilder<ContainerResource> sqlServer = builder
    .AddContainer(name: "azuresqledge", image: "mcr.microsoft.com/azure-sql-edge")
    .WithLifetime(ContainerLifetime.Persistent)
    // Add essential environment variables for container initialization
    .WithEnvironment("ACCEPT_EULA", "Y")
    // Use Developer edition for free development/testing license (Premium is for production)
    .WithEnvironment("MSSQL_PID", "Developer")
    .WithEnvironment("SA_PASSWORD", "absEdel43+-bums")
    // Map standard SQL Server port using WithEndpoint
    .WithEndpoint(port: 1433, targetPort: 1433, name: "sql");

// Add the Northwind MVC project to the application
// Configure it to wait for the SQL Server container to be ready before starting
builder.AddProject<Projects.Northwind_Mvc>("northwind-mvc")
    .WaitFor(sqlServer);

// Build and run the distributed application
// Alternative: await builder.Build().RunAsync(); - for when additional async
// work is needed, async approach is already used under the hood.
builder.Build().Run();
