using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.EntityModels;
using Northwind.Shared.Configuration;

namespace Northwind.EntityModels;

/// <summary>
/// Entity Framework Core DbContext for the Northwind database.
/// Provides access to all Northwind entities and manages the database connection.
/// </summary>
public partial class NorthwindContext : DbContext
{
    /// <summary>
    /// Default constructor, creates an unconfigured context.
    /// Configuration will be handled by OnConfiguring if needed.
    /// </summary>
    public NorthwindContext()
    {
    }

    /// <summary>
    /// Constructor that accepts DbContextOptions for configuration.
    /// Used when the context is configured through dependency injection.
    /// </summary>
    /// <param name="options">The options to configure this context instance.</param>
    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    // DbSet properties for each entity in the Northwind database

    /// <summary>View showing products with category info in alphabetical order.</summary>
    public virtual DbSet<AlphabeticalListOfProduct> AlphabeticalListOfProducts { get; set; }

    /// <summary>Categories for Northwind products.</summary>
    public virtual DbSet<Category> Categories { get; set; }

    /// <summary>View showing sales by category for 1997.</summary>
    public virtual DbSet<CategorySalesFor1997> CategorySalesFor1997s { get; set; }

    /// <summary>View showing current active product list.</summary>
    public virtual DbSet<CurrentProductList> CurrentProductLists { get; set; }

    /// <summary>Customers who purchase from Northwind.</summary>
    public virtual DbSet<Customer> Customers { get; set; }

    /// <summary>View showing both customers and suppliers by city.</summary>
    public virtual DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get; set; }

    /// <summary>Demographic categories for customer segmentation.</summary>
    public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; }

    /// <summary>Employees of Northwind Traders.</summary>
    public virtual DbSet<Employee> Employees { get; set; }

    /// <summary>View showing invoice information for orders.</summary>
    public virtual DbSet<Invoice> Invoices { get; set; }

    /// <summary>Customer orders placed with Northwind.</summary>
    public virtual DbSet<Order> Orders { get; set; }

    /// <summary>Line items for each order (products, quantities, prices).</summary>
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    /// <summary>View showing extended order details with product information.</summary>
    public virtual DbSet<OrderDetailsExtended> OrderDetailsExtendeds { get; set; }

    /// <summary>View showing the subtotal for each order.</summary>
    public virtual DbSet<OrderSubtotal> OrderSubtotals { get; set; }

    /// <summary>View showing orders with customer and employee details.</summary>
    public virtual DbSet<OrdersQry> OrdersQries { get; set; }

    /// <summary>Products sold by Northwind Traders.</summary>
    public virtual DbSet<Product> Products { get; set; }

    /// <summary>View showing product sales for 1997.</summary>
    public virtual DbSet<ProductSalesFor1997> ProductSalesFor1997s { get; set; }

    /// <summary>View showing products with above-average prices.</summary>
    public virtual DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrices { get; set; }

    /// <summary>View showing products organized by category.</summary>
    public virtual DbSet<ProductsByCategory> ProductsByCategories { get; set; }

    /// <summary>View showing orders grouped by quarter.</summary>
    public virtual DbSet<QuarterlyOrder> QuarterlyOrders { get; set; }

    /// <summary>Sales regions for territory management.</summary>
    public virtual DbSet<Region> Regions { get; set; }

    /// <summary>View showing sales organized by product category.</summary>
    public virtual DbSet<SalesByCategory> SalesByCategories { get; set; }

    /// <summary>View showing sales totals by amount.</summary>
    public virtual DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; }

    /// <summary>Shipping companies that deliver Northwind orders.</summary>
    public virtual DbSet<Shipper> Shippers { get; set; }

    /// <summary>View showing quarterly sales summary data.</summary>
    public virtual DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; }

    /// <summary>View showing yearly sales summary data.</summary>
    public virtual DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; }

    /// <summary>Suppliers who provide products to Northwind.</summary>
    public virtual DbSet<Supplier> Suppliers { get; set; }

    /// <summary>Sales territories organized by region.</summary>
    public virtual DbSet<Territory> Territories { get; set; }

    /// <summary>
    /// Fallback configuration method (inspired from the book).
    /// It acts as a fallback if the context isn't configured through dependency injection.
    /// Uses direct file access to read appsettings.json since DI services 
    /// aren't available here.
    /// (It doesn't have access to the dependency injection container
    ///  It can't access IServiceCollection or IConfiguration from the DI system)
    /// 
    /// Note: This approach is kept for:
    /// - Learning purposes following the book
    /// - Fallback when DI is not available
    /// - Standalone usage scenarios
    /// TODO: Future improvement - Consider removing this in favor of using only 
    /// AddNorthwindContext in NorthwindContextExtensions.cs for cleaner architecture.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This is a specific implementation by Peter an AI.
        // It's different from the book at p. 36, which hardcoded many values.
        
        if (!optionsBuilder.IsConfigured)
        {
            // Build configuration with environment support and user secrets
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddUserSecrets<NorthwindContext>(optional: true); // Only loads in Development

            IConfiguration configuration = configBuilder.Build();

            // Create connection settings from configuration
            var connectionSettings = new DatabaseConnectionSettings();
            configuration.GetSection("DatabaseConnection").Bind(connectionSettings);
            
            // Get credentials from configuration (base or user secrets)
            try
            {
                connectionSettings.UserID = configuration["Database:MY_SQL_USR"] ?? 
                    throw new InvalidOperationException("Database username not found in configuration");
                connectionSettings.Password = configuration["Database:MY_SQL_PWD"] ?? 
                    throw new InvalidOperationException("Database password not found in configuration");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(
                    "Database credentials are missing. If in development, ensure user secrets are configured. " +
                    "See docs/user-secrets-setup.md for details.", ex);
            }

            // Build connection string
            var builder = DatabaseConnectionBuilder.CreateBuilder(connectionSettings);

            optionsBuilder.UseSqlServer(builder.ConnectionString);
            
            optionsBuilder.LogTo(
                NorthwindContextLogger.WriteLine,
                new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting }
            );
        }
    }

    /// <summary>
    /// Configures the database model and entity relationships using Fluent API.
    /// This method is called when the model for this context is being created.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure views
        modelBuilder.Entity<AlphabeticalListOfProduct>(entity =>
        {
            entity.ToView("Alphabetical list of products");
        });

        modelBuilder.Entity<CategorySalesFor1997>(entity =>
        {
            entity.ToView("Category Sales for 1997");
        });

        modelBuilder.Entity<CurrentProductList>(entity =>
        {
            entity.ToView("Current Product List");

            entity.Property(e => e.ProductId).ValueGeneratedOnAdd();
        });

        // Configure Customer entity and its relationships
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).IsFixedLength();

            // Many-to-many relationship with CustomerDemographic
            entity.HasMany(d => d.CustomerTypes).WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerCustomerDemo",
                    r => r.HasOne<CustomerDemographic>().WithMany()
                        .HasForeignKey("CustomerTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CustomerCustomerDemo"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CustomerCustomerDemo_Customers"),
                    j =>
                    {
                        j.HasKey("CustomerId", "CustomerTypeId").IsClustered(false);
                        j.ToTable("CustomerCustomerDemo");
                        j.IndexerProperty<string>("CustomerId")
                            .HasMaxLength(5)
                            .IsFixedLength();
                        j.IndexerProperty<string>("CustomerTypeId")
                            .HasMaxLength(10)
                            .IsFixedLength()
                            .HasColumnName("CustomerTypeID");
                    });
        });

        modelBuilder.Entity<CustomerAndSuppliersByCity>(entity =>
        {
            entity.ToView("Customer and Suppliers by City");
        });

        // Configure CustomerDemographic entity
        modelBuilder.Entity<CustomerDemographic>(entity =>
        {
            entity.HasKey(e => e.CustomerTypeId).IsClustered(false);

            entity.Property(e => e.CustomerTypeId).IsFixedLength();
        });

        // Configure Employee entity and its relationships
        modelBuilder.Entity<Employee>(entity =>
        {
            // Self-referencing relationship for manager/reports
            entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation).HasConstraintName("FK_Employees_Employees");

            // Many-to-many relationship with Territory
            entity.HasMany(d => d.Territories).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeTerritory",
                    r => r.HasOne<Territory>().WithMany()
                        .HasForeignKey("TerritoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeTerritories_Territories"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeTerritories_Employees"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "TerritoryId").IsClustered(false);
                        j.ToTable("EmployeeTerritories");
                        j.IndexerProperty<string>("TerritoryId")
                            .HasMaxLength(20)
                            .HasColumnName("TerritoryID");
                    });
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToView("Invoices");

            entity.Property(e => e.CustomerId).IsFixedLength();
        });

        // Configure Order entity and its relationships
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.CustomerId).IsFixedLength();
            entity.Property(e => e.Freight).HasDefaultValue(0m);

            // Foreign key relationships
            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Customers");
            entity.HasOne(d => d.Employee).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Employees");
            entity.HasOne(d => d.ShipViaNavigation).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Shippers");
        });

        // Configure OrderDetail entity and its relationships
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK_Order_Details");

            entity.Property(e => e.Quantity).HasDefaultValue((short)1);

            // Foreign key relationships
            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Products");
        });

        modelBuilder.Entity<OrderDetailsExtended>(entity =>
        {
            entity.ToView("Order Details Extended");
        });

        modelBuilder.Entity<OrderSubtotal>(entity =>
        {
            entity.ToView("Order Subtotals");
        });

        modelBuilder.Entity<OrdersQry>(entity =>
        {
            entity.ToView("Orders Qry");

            entity.Property(e => e.CustomerId).IsFixedLength();
        });

        // Configure Product entity and its relationships
        modelBuilder.Entity<Product>(entity =>
        {
            // Default values for product properties
            entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
            entity.Property(e => e.UnitPrice).HasDefaultValue(0m);
            entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
            entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);

            // Foreign key relationships
            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_Products_Categories");
            entity.HasOne(d => d.Supplier).WithMany(p => p.Products).HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<ProductSalesFor1997>(entity =>
        {
            entity.ToView("Product Sales for 1997");
        });

        modelBuilder.Entity<ProductsAboveAveragePrice>(entity =>
        {
            entity.ToView("Products Above Average Price");
        });

        modelBuilder.Entity<ProductsByCategory>(entity =>
        {
            entity.ToView("Products by Category");
        });

        modelBuilder.Entity<QuarterlyOrder>(entity =>
        {
            entity.ToView("Quarterly Orders");

            entity.Property(e => e.CustomerId).IsFixedLength();
        });

        // Configure Region entity
        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).IsClustered(false);

            entity.Property(e => e.RegionId).ValueGeneratedNever();
            entity.Property(e => e.RegionDescription).IsFixedLength();
        });

        modelBuilder.Entity<SalesByCategory>(entity =>
        {
            entity.ToView("Sales by Category");
        });

        modelBuilder.Entity<SalesTotalsByAmount>(entity =>
        {
            entity.ToView("Sales Totals by Amount");
        });

        modelBuilder.Entity<SummaryOfSalesByQuarter>(entity =>
        {
            entity.ToView("Summary of Sales by Quarter");
        });

        modelBuilder.Entity<SummaryOfSalesByYear>(entity =>
        {
            entity.ToView("Summary of Sales by Year");
        });

        // Configure Territory entity and its relationships
        modelBuilder.Entity<Territory>(entity =>
        {
            entity.HasKey(e => e.TerritoryId).IsClustered(false);

            entity.Property(e => e.TerritoryDescription).IsFixedLength();

            // Region relationship
            entity.HasOne(d => d.Region).WithMany(p => p.Territories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Territories_Region");
        });

        // Call any additional configuration in the partial class implementation
        OnModelCreatingPartial(modelBuilder);
    }

    /// <summary>
    /// Partial method that can be implemented in another part of this partial class
    /// to extend the model configuration without modifying this generated file.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model.</param>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
