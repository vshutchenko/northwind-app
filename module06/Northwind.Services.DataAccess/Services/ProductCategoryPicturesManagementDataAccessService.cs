using System;
using System.IO;
using System.Threading.Tasks;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

#pragma warning disable CA2000 // Dispose objects before losing scope
namespace Northwind.Services.DataAccess.Services
{
    /// <inheritdoc/>
    public class ProductCategoryPicturesManagementDataAccessService : IProductCategoryPicturesService
    {
        private const int ReservedBytes = 78;
        private readonly NorthwindDataAccessFactory accessFactory;
        private readonly int maxFileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryPicturesManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">Factory to data access objects.</param>
        /// <param name="maxFileSize">Maximum file size.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        /// <exception cref="ArgumentException">Throws if <paramref name="maxFileSize"/> is less than 0.</exception>
        public ProductCategoryPicturesManagementDataAccessService(NorthwindDataAccessFactory accessFactory, int maxFileSize)
        {
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));
            if (maxFileSize < 0)
            {
                throw new ArgumentException("file size cannot be negative");
            }

            this.maxFileSize = maxFileSize;
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetPictureAsync(int categoryId)
        {
            try
            {
                var transfer = await this.accessFactory
                    .GetProductCategoryDataAccessObject()
                    .FindProductCategoryAsync(categoryId);

                return transfer.Picture.Length == 0
                    ? Array.Empty<byte>()
                    : transfer.Picture[ReservedBytes..];
            }
            catch (ProductCategoryNotFoundException)
            {
                return Array.Empty<byte>();
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyPictureAsync(int categoryId)
        {
            try
            {
                var categoryDao = this.accessFactory.GetProductCategoryDataAccessObject();
                var transfer = await categoryDao.FindProductCategoryAsync(categoryId);
                transfer.Picture = Array.Empty<byte>();
                await categoryDao.UpdateProductCategoryAsync(transfer);
                return true;
            }
            catch (ProductCategoryNotFoundException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePictureAsync(int categoryId, Stream stream)
        {
            stream = stream ?? throw new ArgumentNullException(nameof(stream));
            if (stream.Length > this.maxFileSize)
            {
                return false;
            }

            var categoryDao = this.accessFactory.GetProductCategoryDataAccessObject();

            try
            {
                var transfer = await categoryDao.FindProductCategoryAsync(categoryId);

                byte[] picture = new byte[stream.Length + ReservedBytes];

                await using MemoryStream ms = new MemoryStream(picture);
                stream.Seek(0, SeekOrigin.Begin);
                ms.Seek(ReservedBytes, SeekOrigin.Begin);
                await stream.CopyToAsync(ms);
                await stream.FlushAsync();

                transfer.Picture = picture;
                await categoryDao.UpdateProductCategoryAsync(transfer);

                return true;
            }
            catch (ProductCategoryNotFoundException)
            {
                return false;
            }
        }
    }
}
