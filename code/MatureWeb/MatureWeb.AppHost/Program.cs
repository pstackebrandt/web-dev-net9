var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Northwind_Mvc>("northwind-mvc");

builder.Build().Run();
