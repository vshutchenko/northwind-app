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
    public sealed class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryManagementService"/> class.
        /// </summary>
        /// <param name="context">Data base context.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public ProductCategoryManagementService(NorthwindContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateCategoryAsync(ProductCategory productCategory)
        {
            productCategory = productCategory ?? throw new ArgumentNullException(nameof(productCategory));

            productCategory.Id = this.GenerateCategoryId();
            var a = this.mapper.Map<CategoryEntity>(productCategory);
            await this.context.Categories.AddAsync(this.mapper.Map<ProductCategory, CategoryEntity>(productCategory));
            await this.context.SaveChangesAsync();

            return productCategory.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            var existingCategory = await this.context.Categories
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();

            if (existingCategory != null)
            {
                this.context.Remove(existingCategory);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategory> LookupCategoriesByNameAsync(IEnumerable<string> names)
        {
            var query = from c in this.context.Categories
                        from n in names
                        where c.Name.Equals(n, StringComparison.OrdinalIgnoreCase)
                        select c;

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<ProductCategory>(entity);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset, int limit)
        {
            var query = this.context.Categories
                .Skip(offset)
                .Take(limit);

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<ProductCategory>(entity);
            }
        }

        /// <inheritdoc/>
        public async Task<ProductCategory> GetCategoryAsync(int categoryId)
        {
            var categoryEntity = await this.context.Categories
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();

            return this.mapper.Map<ProductCategory>(categoryEntity);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategory productCategory)
        {
            productCategory = productCategory ?? throw new ArgumentNullException(nameof(productCategory));

            var existingCategory = await this.context.Categories
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();

            if (existingCategory != null)
            {
                existingCategory.Name = productCategory.Name;
                existingCategory.Description = productCategory.Description;
                existingCategory.Picture = productCategory.Picture;
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private int GenerateCategoryId()
        {
            int id = this.context.Categories.Count() + 1;
            for (int i = 0; i < int.MaxValue; i++, id++)
            {
                if (!this.context.Categories.Any(c => c.Id == id))
                {
                    return id;
                }
            }

            return 0;
        }
    }
}
