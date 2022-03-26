using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.Services
{
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementService"/> class.
        /// </summary>
        /// <param name="context">Data base context.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public ProductManagementService(NorthwindContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateProductAsync(Product product)
        {
            product = product ?? throw new ArgumentNullException(nameof(product));

            product.Id = this.GenerateProductId();
            await this.context.Products.AddAsync(this.mapper.Map<ProductEntity>(product));
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
        public async IAsyncEnumerable<Product> LookupProductsByNameAsync(IEnumerable<string> names)
        {
            var query = from p in this.context.Products
                        from n in names
                        where p.Name.Equals(n, StringComparison.OrdinalIgnoreCase)
                        select p;

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<Product>(entity);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit)
        {
            var query = this.context.Products
                .Skip(offset)
                .Take(limit);

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<Product>(entity);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> GetProductsForCategoryAsync(int categoryId)
        {
            var query = this.context.Products.Where(p => p.Id == categoryId);

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<Product>(entity);
            }
        }

        /// <inheritdoc/>
        public async Task<Product> GetProductAsync(int productId)
        {
            var productEntity = await this.context.Products
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            return this.mapper.Map<Product>(productEntity);
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
