// Contains a sequence of supliers to display in a html table.

using Northwind.EntityModels; // To use Supplier type

namespace Northwind.Mvc.Models;

public record HomeSuppliersViewModel(IEnumerable<Supplier>? Suppliers);

