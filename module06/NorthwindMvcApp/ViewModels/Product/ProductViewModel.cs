using System.ComponentModel;

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
        public int? SupplierId { get; set; }

        /// <summary>
        /// Gets or sets a category identifier.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a quantity per unit.
        /// </summary>
        [DisplayName("Quantity per unit")]
        public string QuantityPerUnit { get; set; }

        /// <summary>
        /// Gets or sets a unit price.
        /// </summary>
        [DisplayName("Unit price")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets an amount of units in stock.
        /// </summary>
        [DisplayName("Units in stock")]
        public short? UnitsInStock { get; set; }

        /// <summary>
        /// Gets or sets an amount of units on order.
        /// </summary>
        [DisplayName("Units on order")]
        public short? UnitsOnOrder { get; set; }

        /// <summary>
        /// Gets or sets a reorder level.
        /// </summary>
        [DisplayName("Reorder level")]
        public short? ReorderLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a product is discontinued.
        /// </summary>
        public bool Discontinued { get; set; }

        /// <summary>
        /// Gets or sets a category name.
        /// </summary>
        [DisplayName("Category")]
        public string CategoryName { get; set; }
    }
}
