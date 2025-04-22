// Contains a sequence of suppliers to display in a html table.

using Northwind.EntityModels; // To use Supplier type

namespace Northwind.Mvc.Models;

public record HomeSupplierViewModel(int EntitiesAffected, Supplier? Supplier);

