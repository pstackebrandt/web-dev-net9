using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using Northwind.EntityModels;
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
    public IActionResult Index()
    {
        /* _logger.LogError("This is a (serious) test error");
        _logger.LogWarning("This is a first warning");
        _logger.LogWarning("This is a second warning");
        _logger.LogInformation("Index method of Home controller called"); */

        HomeIndexViewModel model = new(VisitorCount: Random.Shared.Next(1, 1001),
                                        Categories: _db.Categories.ToList(),
                                        Products: _db.Products.ToList());
        return View(model);
    }

    /// <summary>
    /// Displays the privacy policy page.
    /// </summary>
    /// <returns>Privacy policy view.</returns>
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
