using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using Northwind.EntityModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace Northwind.Mvc.Controllers;

/// <summary>
/// Controller handling the basic website pages including home page,
/// privacy policy, and error handling.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NorthwindContext _db;

    /// <summary>
    /// Initializes a new instance of the HomeController class.
    /// </summary>
    /// <param name="logger">Logger to record diagnostic information.</param>
    /// <param name="db">Northwind database context for data access.</param>
    public HomeController(ILogger<HomeController> logger, NorthwindContext db)
    {
        _logger = logger;
        _db = db;
    }

    /// <summary>
    /// Displays the home page with a random visitor count
    /// and lists of categories and products.
    /// </summary>
    /// <returns>View with home page data.</returns>
    /// <remarks>
    /// This method implements specialized error handling for database connection issues.
    /// If the database is unavailable (e.g., Docker container not running), it will 
    /// display a user-friendly DatabaseError view with troubleshooting instructions
    /// instead of the generic error page.
    /// </remarks>
    public IActionResult Index()
    {
        /* _logger.LogError("This is a (serious) test error");
        _logger.LogWarning("This is a first warning");
        _logger.LogWarning("This is a second warning");
        _logger.LogInformation("Index method of Home controller called"); */

        try
        {
            _logger.LogInformation("Attempting to access database in HomeController.Index");
            HomeIndexViewModel model = new(
                VisitorCount: Random.Shared.Next(1, 1001),
                Categories: _db.Categories.ToList(),
                Products: _db.Products.ToList());
            
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Exception details: {ExceptionType}, Message: {Message}", 
                ex.GetType().FullName, ex.Message);
            
            // Let's catch any database-related exception without the custom condition
            if (ex is SqlException || ex is DbUpdateException || 
                ex.Message.Contains("database", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("sql", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("connect", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("connection", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("network", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase) ||
                HasDatabaseRelatedInnerException(ex))
            {
                _logger.LogError(ex, "Database connection failed. Is the SQL Server Docker container running?");
                ViewData["ErrorTitle"] = "Database Connection Error";
                ViewData["ErrorMessage"] = "Could not connect to the Northwind database. " +
                    "Please ensure that the SQL Server Docker container is running.";
                ViewData["Solution"] = "Try running: docker start sql-container";
                
                // Return DatabaseError view without redirect
                return View("DatabaseError");
            }
            
            _logger.LogError(ex, "An error occurred while loading the home page");
            return RedirectToAction(nameof(Error));
        }
    }

    /// <summary>
    /// Checks if an exception has database-related inner exceptions.
    /// </summary>
    /// <param name="ex">The exception to check.</param>
    /// <returns>True if a database-related inner exception exists.</returns>
    private bool HasDatabaseRelatedInnerException(Exception ex)
    {
        Exception? currentEx = ex.InnerException;
        while (currentEx != null)
        {
            if (currentEx is SqlException || currentEx is DbUpdateException ||
                currentEx.Message.Contains("database", StringComparison.OrdinalIgnoreCase) ||
                currentEx.Message.Contains("sql", StringComparison.OrdinalIgnoreCase) ||
                currentEx.Message.Contains("connect", StringComparison.OrdinalIgnoreCase) ||
                currentEx.Message.Contains("connection", StringComparison.OrdinalIgnoreCase) ||
                currentEx.Message.Contains("network", StringComparison.OrdinalIgnoreCase) ||
                currentEx.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Database-related inner exception found: {ExceptionType}, Message: {Message}",
                    currentEx.GetType().FullName, currentEx.Message);
                return true;
            }
            currentEx = currentEx.InnerException;
        }
        return false;
    }

    /// <summary>
    /// Displays the privacy policy page.
    /// </summary>
    /// <returns>Privacy policy view.</returns>
    [Route("private")]
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Displays an error page with request identification details.
    /// </summary>
    /// <returns>Error view with request details.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
