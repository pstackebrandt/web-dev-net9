/*
 * Program.cs
 * ---------
 * Entry point for the Northwind MVC application.
 * 
 * This file configures and launches the ASP.NET Core application using the
 * minimal hosting model introduced in .NET 6. It sets up:
 * - SQLite database with Entity Framework Core
 * - ASP.NET Core Identity for authentication
 * - MVC controllers and Razor views
 * - HTTP pipeline with appropriate middleware
 * - Routing for controllers and Razor pages
 */

// Import required namespaces
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // for SqlConnectionStringBuilder

// Import own namespaces
using Northwind.EntityModels; // for AddNorthwindContext()
using Northwind.Mvc.Data;


#region Application Setup
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Get connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure Entity Framework with SQLite for the application database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure ASP.NET Core Identity system
// Requires email confirmation for new accounts
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

string? sqlServerConnectionString = null;

// Get connection string from configuration
// We don't have a connection string in the appsettings.json file.
// We have different values in the appsettings.json file and in the user secrets.
// We need to combine them into a single connection string.

// We did something like this already in the Northwind.UnitTests project for DatabaseTestBase.
// We will do the equivalent here.

// Add NorthwindContext to the services container
if (sqlServerConnectionString != null)
{
    builder.Services.AddNorthwindContext(sqlServerConnectionString);
} else {
    Console.WriteLine("NorthwindConnection string is missing from configuration");
}

var app = builder.Build();
#endregion

#region Middleware Pipeline
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Show detailed migration errors in development
    app.UseMigrationsEndPoint();
}
else
{
    // Use standard error page in production
    app.UseExceptionHandler("/Home/Error");

    // Enable HTTP Strict Transport Security (HSTS)
    // HSTS is a web security policy mechanism that helps protect websites against
    // protocol downgrade attacks and cookie hijacking.
    // The default HSTS value is 30 days, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Enable routing capability
// - Establishes endpoint routing system required for controllers and Razor Pages
// - Matches incoming HTTP requests to appropriate endpoints 
// - Prepares routing context for downstream middleware (like Authorization)
// Must be called before any middleware that depends on routing information
app.UseRouting();

// Enable authentication and authorization
app.UseAuthorization();
#endregion

#region Route Configuration
// Configure static assets handling
app.MapStaticAssets();

// Configure controller routes
// Default route pattern: /[controller]/[action]/[optional-id]
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Enable Razor Pages with static assets support
app.MapRazorPages()
   .WithStaticAssets();

// Add endpoint to display the environment name
app.MapGet("env/", () =>
    $"Environment: {app.Environment.EnvironmentName}");
#endregion

// Start the application
// @SuppressWarnings("S4462", "S6966") // Suppress warnings about async methods
app.Run();
