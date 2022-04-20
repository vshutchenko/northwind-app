using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels.Product
{
    /// <summary>
    /// Product view model.
    /// </summary>
    public class ProductViewModel
    {
        /// <summary>
        /// Gets or sets a product identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a supplier identifier.
        /// </summary>
        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }

        /// <summary>
        /// Gets or sets a category identifier.
        /// </summary>
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a quantity per unit.
        /// </summary>
        [Display(Name = "Quantity per unit")]
        public string QuantityPerUnit { get; set; }

        /// <summary>
        /// Gets or sets a unit price.
        /// </summary>
        [Display(Name = "Unit price")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets an amount of units in stock.
        /// </summary>
        [Display(Name = "Units in stock")]
        public short? UnitsInStock { get; set; }

        /// <summary>
        /// Gets or sets an amount of units on order.
        /// </summary>
        [Display(Name = "Units on order")]
        public short? UnitsOnOrder { get; set; }

        /// <summary>
        /// Gets or sets a reorder level.
        /// </summary>
        [Display(Name = "Reorder level")]
        public short? ReorderLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a product is discontinued.
        /// </summary>
        public bool Discontinued { get; set; }

        /// <summary>
        /// Gets or sets a category name.
        /// </summary>
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
}
