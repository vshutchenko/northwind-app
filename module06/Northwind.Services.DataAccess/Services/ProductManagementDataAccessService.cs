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
    public class ProductManagementDataAccessService : IProductManagementService
    {
        private readonly NorthwindDataAccessFactory accessFactory;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">Factory to data access objects.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public ProductManagementDataAccessService(NorthwindDataAccessFactory accessFactory, IMapper mapper)
        {
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit)
        {
            await foreach (var transfer in this.accessFactory
                .GetProductDataAccessObject()
                .SelectProductsAsync(offset, limit))
            {
                yield return this.mapper.Map<Product>(transfer);
            }
        }

        /// <inheritdoc/>
        public async Task<Product> GetProductAsync(int productId)
        {
            try
            {
                var product = this.mapper.Map<Product>(await this.accessFactory
                    .GetProductDataAccessObject()
                    .FindProductAsync(productId));
                return product;
            }
            catch (ProductNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<int> CreateProductAsync(Product product)
        {
            product = product ?? throw new ArgumentNullException(nameof(product));

            return await this.accessFactory
                .GetProductDataAccessObject()
                .InsertProductAsync(this.mapper.Map<ProductTransferObject>(product));
        }

        /// <inheritdoc/>
        public Task<bool> DestroyProductAsync(int productId)
        {
            return this.accessFactory.GetProductDataAccessObject().DeleteProductAsync(productId);
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> LookupProductsByNameAsync(IEnumerable<string> names)
        {
            names = names ?? throw new ArgumentNullException(nameof(names));

            await foreach (var transfer in this.accessFactory
                .GetProductDataAccessObject()
                .SelectProductsByNameAsync(names))
            {
                yield return this.mapper.Map<Product>(transfer);
            }
        }

        /// <inheritdoc/>
        public Task<bool> UpdateProductAsync(int productId, Product product)
        {
            product = product ?? throw new ArgumentNullException(nameof(product));

            return this.accessFactory
                .GetProductDataAccessObject()
                .UpdateProductAsync(this.mapper.Map<ProductTransferObject>(product));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> GetProductsForCategoryAsync(int categoryId)
        {
            await foreach (var transfer in this.accessFactory
                .GetProductDataAccessObject()
                .SelectProductByCategoryAsync(new List<int> { categoryId }))
            {
                yield return this.mapper.Map<Product>(transfer);
            }
        }
    }
}
