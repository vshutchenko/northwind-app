using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels.Category
{
    /// <summary>
    /// Product category input view model.
    /// </summary>
    public class CategoryInputViewModel
    {
        /// <summary>
        /// Gets or sets a category identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a product category name.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a category description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets new category picture.
        /// </summary>
        [DisplayName("New picture")]
        public IFormFile NewPicture { get; set; }

        /// <summary>
        /// Gets or sets a category picture.
        /// </summary>
        [DisplayName("Current picture")]
        public byte[] Picture { get; set; }
    }
}
