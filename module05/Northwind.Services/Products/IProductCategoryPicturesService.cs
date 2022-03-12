using System.IO;
using System.Threading.Tasks;

namespace Northwind.Services.Products
{
    /// <summary>
    /// Represents a management service for pictures.
    /// </summary>
    public interface IProductCategoryPicturesService
    {
        /// <summary>
        /// Gets a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>Product category picture.</returns>
        Task<byte[]> GetPictureAsync(int categoryId);

        /// <summary>
        /// Update a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <returns>True if a product category exists; otherwise false.</returns>
        Task<bool> UpdatePictureAsync(int categoryId, Stream stream);

        /// <summary>
        /// Destroy a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>True if a product category exists; otherwise false.</returns>
        Task<bool> DestroyPictureAsync(int categoryId);
    }
}
