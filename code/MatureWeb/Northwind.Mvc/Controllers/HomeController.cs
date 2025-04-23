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

    public IActionResult ProductDetail(int? id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!id.HasValue)
        {
            return BadRequest("You must provide a product ID in the route, for example, /Home/ProductDetail/21 .");
        }

        Product? model = _db.Products.Include(p => p.Category)
            .SingleOrDefault(p => p.ProductId == id);

        if (model is null)
        {
            return NotFound($"Product with ID {id} not found.");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ProductsThatCostMoreThan(decimal? price)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!price.HasValue)
        {
            return BadRequest("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50 .");
        }

        IEnumerable<Product> model = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.UnitPrice > price);

        if (!model.Any())
        {
            return NotFound($"No products cost more than {price:C}.");
        }

        // Format currency using web server's culture.
        ViewData["MaxPrice"] = price.Value.ToString("C");

        // We can override the seach path convention.
        return View("Views/Home/CostlyProducts.cshtml", model);
    }

    /// <summary>
    /// Displays a list of suppliers in a table.
    /// </summary>
    /// <returns>A view with a orderedlist of suppliers.</returns>
    public IActionResult Suppliers()
    {
        HomeSuppliersViewModel model = new(_db.Suppliers
     .OrderBy(c => c.Country)
     .ThenBy(c => c.CompanyName));

        return View(model);
    }

    [HttpGet]
    [Route("/home/addsupplier")]
    public IActionResult AddSupplier()
    {
        HomeSupplierViewModel model = new(0, new Supplier());

        return View(model);
    }

    [HttpPost]
    [Route("/home/addsupplier")]
    [ValidateAntiForgeryToken]
    public IActionResult AddSupplier(Supplier supplier)
    {
        int affected = 0;

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _db.Suppliers.Add(supplier);
        affected = _db.SaveChanges();

        HomeSupplierViewModel model = new(affected, supplier);

        if (affected == 0) // Supplier was not added
        {
            // Views\Home\AddSupplier.cshtml
            return View(model);
        }
        else // Supplier was added successfully
        {
            return RedirectToAction(nameof(Suppliers));
        }
    }

    [HttpGet]
    public IActionResult EditSupplier(int? id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!id.HasValue)
        {
            return BadRequest("You must provide a supplier ID in the route, for example, /Home/EditSupplier/1 .");
        }

        Supplier? supplierInDb = _db.Suppliers.Find(id);

        if (supplierInDb is null)
        {
            return NotFound($"Supplier with ID {id} not found.");
        }

        HomeSupplierViewModel model = new(1, supplierInDb);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditSupplier(Supplier supplier)
    {
        int affected = 0;

        if (ModelState.IsValid)
        {
            Supplier? supplierInDb = _db.Suppliers.Find(supplier.SupplierId);

            if (supplierInDb is not null)
            {
                supplierInDb.CompanyName = supplier.CompanyName;
                supplierInDb.Country = supplier.Country;
                supplierInDb.Phone = supplier.Phone;

                /*
                // Other properties not in the HTML form would be updated here:
                // ContactName, ContactTitle, Address, City, Region, PostalCode, Fax, etc.
                */

                affected = _db.SaveChanges();
            }
        }

        HomeSupplierViewModel model = new(affected, supplier);

        if (affected == 0) // Supplier was not updated
        {
            // Views\Home\EditSupplier.cshtml
            return View(model);
        }
        else // Supplier was updated successfully
        {
            return RedirectToAction(nameof(Suppliers));
        }
    }

    [HttpGet]
    public IActionResult DeleteSupplier(int? id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Supplier? supplierInDb = _db.Suppliers.Find(id);

        HomeSupplierViewModel model = new(supplierInDb is null ? 0 : 1, supplierInDb);

        // Views\Home\DeleteSupplier.cshtml
        return View(model);
    }

    [HttpPost("/home/deletesupplier/{id:int?}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteSupplierPost(int? id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int affected = 0;

        Supplier? supplierInDb = _db.Suppliers.Find(id);

        if (supplierInDb is not null)
        {
            _db.Suppliers.Remove(supplierInDb);
            affected = _db.SaveChanges();
        }

        HomeSupplierViewModel model = new(affected, supplierInDb);

        if (affected == 0) // Supplier was not deleted   
        {
            // Views\Home\DeleteSupplier.cshtml
            return View(model);
        }
        else // Supplier was deleted successfully
        {
            return RedirectToAction(nameof(Suppliers));
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
    /// Displays a page with a form to submit a Thing.
    /// </summary>
    /// <remarks> For GET and other HTTP methods but POST.</remarks>
    /// <returns>Model binding view.</returns>
    [Route("modelbinding")]
    public IActionResult ModelBinding()
    {
        return View();
    }

    /// <summary>
    /// Handles the submission of a Thing form.
    /// </summary>
    /// <param name="thing">The Thing object to submit.</param>
    /// <returns>Model binding view with validation results.</returns>
    [HttpPost]
    public IActionResult ModelBinding(Thing thing)
    {
        HomeModelBindingViewModel model = new(
            Thing: thing,
            HasErrors: !ModelState.IsValid,
            ValidationErrors: ModelState.Values.SelectMany(state => state.Errors)
                .Select(error => error.ErrorMessage)
        );

        return View(model);
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
