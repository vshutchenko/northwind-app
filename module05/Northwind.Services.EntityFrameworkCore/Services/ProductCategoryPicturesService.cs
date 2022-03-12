using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.Products;

#pragma warning disable CA2000
namespace Northwind.Services.EntityFrameworkCore.Services
{
    /// <summary>
    /// Represents a picture management service.
    /// </summary>
    public sealed class ProductCategoryPicturesService : IProductCategoryPicturesService
    {
        private readonly int maxFileSize;
        private readonly NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryPicturesService"/> class.
        /// </summary>
        /// <param name="context">Data base context.</param>
        /// <param name="maxFileSize">Maximum file size.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public ProductCategoryPicturesService(NorthwindContext context, int maxFileSize)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            if (maxFileSize < 0)
            {
                throw new ArgumentException("file size cannot be negative");
            }

            this.maxFileSize = maxFileSize;
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetPictureAsync(int categoryId)
        {
            var existingCategory = await this.context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

            if (existingCategory is null || existingCategory.Picture is null)
            {
                return null;
            }

            return existingCategory.Picture;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyPictureAsync(int categoryId)
        {
            var existingCategory = await this.context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

            if (existingCategory is null)
            {
                return false;
            }

            existingCategory.Picture = null;
            await this.context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePictureAsync(int categoryId, Stream stream)
        {
            stream = stream ?? throw new ArgumentNullException(nameof(stream));

            var existingCategory = await this.context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

            if (existingCategory is null || stream.Length > this.maxFileSize)
            {
                return false;
            }

            byte[] picture = new byte[stream.Length];

            await using MemoryStream ms = new MemoryStream(picture);

            ms.Seek(0, SeekOrigin.Begin);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(ms);
            await stream.FlushAsync();

            existingCategory.Picture = picture;
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
