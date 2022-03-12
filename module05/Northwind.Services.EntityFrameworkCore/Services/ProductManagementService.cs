using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.Services
{
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementService"/> class.
        /// </summary>
        /// <param name="context">Data base context.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public ProductManagementService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<int> CreateProductAsync(Product product)
        {
            product = product ?? throw new ArgumentNullException(nameof(product));

            product.Id = this.GenerateProductId();
            await this.context.Products.AddAsync(product);
            await this.context.SaveChangesAsync();

            return product.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyProductAsync(int productId)
        {
            var existingProduct = await this.context.Products
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (existingProduct != null)
            {
                this.context.Remove(existingProduct);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<Product> LookupProductsByNameAsync(IEnumerable<string> names)
        {
            return (from p in this.context.Products
                    from n in names
                    where p.Name.Equals(n, StringComparison.OrdinalIgnoreCase)
                    select p).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit)
        {
            return this.context.Products
                .Skip(offset)
                .Take(limit)
                .AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<Product> GetProductsForCategoryAsync(int categoryId)
        {
            return this.context.Products
                .Where(p => p.Id == categoryId)
                .AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public async Task<Product> GetProductAsync(int productId)
        {
            var product = await this.context.Products
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            return product;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            product = product ?? throw new ArgumentNullException(nameof(product));

            var existingProduct = await this.context.Products
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.UnitsOnOrder = product.UnitsOnOrder;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.SupplierId = product.SupplierId;
                existingProduct.ReorderLevel = product.ReorderLevel;
                existingProduct.QuantityPerUnit = product.QuantityPerUnit;
                existingProduct.Discontinued = product.Discontinued;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.QuantityPerUnit = product.QuantityPerUnit;
                existingProduct.UnitsInStock = product.UnitsInStock;

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private int GenerateProductId()
        {
            int id = this.context.Products.Count() + 1;
            for (int i = 0; i < int.MaxValue; i++, id++)
            {
                if (!this.context.Products.Any(p => p.Id == id))
                {
                    return id;
                }
            }

            return 0;
        }
    }
}
