using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels.Product
{
    /// <summary>
    /// Product input view model.
    /// </summary>
    public class ProductInputViewModel
    {
        /// <summary>
        /// Gets or sets a product identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a product name.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
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
        [Range(0, int.MaxValue, ErrorMessage = "Should be a positive value.")]
        [Display(Name = "Unit price")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets an amount of units in stock.
        /// </summary>
        [Range(0, short.MaxValue, ErrorMessage = "Should be a positive value.")]
        [Display(Name = "Units in stock")]
        public short? UnitsInStock { get; set; }

        /// <summary>
        /// Gets or sets an amount of units on order.
        /// </summary>
        [Range(0, short.MaxValue, ErrorMessage = "Should be a positive value.")] 
        [Display(Name = "Units on order")]
        public short? UnitsOnOrder { get; set; }

        /// <summary>
        /// Gets or sets a reorder level.
        /// </summary>
        [Range(0, short.MaxValue, ErrorMessage = "Should be a positive value.")]
        [Display(Name = "Reorder level")]
        public short? ReorderLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a product is discontinued.
        /// </summary>
        public bool Discontinued { get; set; }

        /// <summary>
        /// Gets or sets categories list.
        /// </summary>
        [Display(Name = "Categories")]
        public IEnumerable<SelectListItem> CategoryItems { get; set; }
    }
}
