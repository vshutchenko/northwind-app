namespace NorthwindMvcApp.ViewModels.Category
{
    /// <summary>
    /// Product category view model.
    /// </summary>
    public class CategoryViewModel
    {
        /// <summary>
        /// Gets or sets a category identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a product category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a category description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a category picture.
        /// </summary>
        public byte[] Picture { get; set; }
    }
}
