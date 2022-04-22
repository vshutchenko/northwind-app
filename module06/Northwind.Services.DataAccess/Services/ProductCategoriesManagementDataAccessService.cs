using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.Services.DataAccess.Services
{
    /// <inheritdoc/>
    public class ProductCategoriesManagementDataAccessService : IProductCategoryManagementService
    {
        private readonly NorthwindDataAccessFactory accessFactory;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoriesManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">Factory to data access objects.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public ProductCategoriesManagementDataAccessService(NorthwindDataAccessFactory accessFactory, IMapper mapper)
        {
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset, int limit)
        {
            await foreach (var transfer in this.accessFactory
                .GetProductCategoryDataAccessObject()
                .SelectProductCategoriesAsync(offset, limit))
            {
                yield return this.mapper.Map<ProductCategory>(transfer);
            }
        }

        /// <inheritdoc/>
        public async Task<ProductCategory> GetCategoryAsync(int categoryId)
        {
            try
            {
                return this.mapper.Map<ProductCategory>(await this.accessFactory
                    .GetProductCategoryDataAccessObject()
                    .FindProductCategoryAsync(categoryId));
            }
            catch (ProductCategoryNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public Task<int> CreateCategoryAsync(ProductCategory productCategory)
        {
            productCategory = productCategory ?? throw new ArgumentNullException(nameof(productCategory));

            return this.accessFactory
                .GetProductCategoryDataAccessObject()
                .InsertProductCategoryAsync(this.mapper.Map<ProductCategoryTransferObject>(productCategory));
        }

        /// <inheritdoc/>
        public Task<bool> DestroyCategoryAsync(int categoryId)
        {
            return this.accessFactory
                .GetProductCategoryDataAccessObject()
                .DeleteProductCategoryAsync(categoryId);
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategory> LookupCategoriesByNameAsync(IEnumerable<string> names)
        {
            names = names ?? throw new ArgumentNullException(nameof(names));

            await foreach (var transfer in this.accessFactory
                .GetProductCategoryDataAccessObject()
                .SelectProductCategoriesByNameAsync(names))
            {
                yield return this.mapper.Map<ProductCategory>(transfer);
            }
        }

        /// <inheritdoc/>
        public Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategory productCategory)
        {
            productCategory = productCategory ?? throw new ArgumentNullException(nameof(productCategory));

            return this.accessFactory
                .GetProductCategoryDataAccessObject()
                .UpdateProductCategoryAsync(this.mapper.Map<ProductCategoryTransferObject>(productCategory));
        }

        /// <inheritdoc/>
        public Task<int> CountAsync()
        {
            return this.accessFactory
                .GetProductCategoryDataAccessObject()
                .CountAsync();
        }
    }
}
