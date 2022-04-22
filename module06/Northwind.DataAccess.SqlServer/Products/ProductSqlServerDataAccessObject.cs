using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using Northwind.DataAccess.Products;

#pragma warning disable CA2000
namespace Northwind.DataAccess.SqlServer.Products
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind products.
    /// </summary>
    public sealed class ProductSqlServerDataAccessObject : IProductDataAccessObject
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        public ProductSqlServerDataAccessObject(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc/>
        public Task<int> InsertProductAsync(ProductTransferObject product)
        {
            product = product ?? throw new ArgumentNullException(nameof(product));

            return InsertAsync();

            async Task<int> InsertAsync()
            {
                await using var sqlCommand = new SqlCommand("InsertProduct", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.AddRange(GetProductParameters(product));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int id = await sqlCommand.ExecuteNonQueryAsync();

                return id;
            }
        }

        /// <inheritdoc/>
        public Task<bool> DeleteProductAsync(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(productId));
            }

            return DeleteAsync();

            async Task<bool> DeleteAsync()
            {
                await using var sqlCommand = new SqlCommand("DeleteProductById", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", productId));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int deletedCount = await sqlCommand.ExecuteNonQueryAsync();

                return deletedCount > 0;
            }
        }

        /// <inheritdoc/>
        public Task<bool> UpdateProductAsync(ProductTransferObject product)
        {
            product = product ?? throw new ArgumentNullException(nameof(product));

            return UpdateAsync();

            async Task<bool> UpdateAsync()
            {
                await using var sqlCommand = new SqlCommand("UpdateProduct", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", product.Id));
                sqlCommand.Parameters.AddRange(GetProductParameters(product));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int updatedCount = await sqlCommand.ExecuteNonQueryAsync();

                return updatedCount > 0;
            }
        }

        /// <inheritdoc/>
        public Task<ProductTransferObject> FindProductAsync(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(productId));
            }

            return FindAsync();

            async Task<ProductTransferObject> FindAsync()
            {
                await using var sqlCommand = new SqlCommand("GetProductById", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", productId));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return CreateProductTransferObject(reader);
                }
                else
                {
                    throw new ProductNotFoundException(productId);
                }
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<ProductTransferObject> SelectProductsAsync(int offset, int limit)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Must be greater than zero or equals zero.", nameof(offset));
            }

            if (limit < 1)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(limit));
            }

            return SelectAsync();

            async IAsyncEnumerable<ProductTransferObject> SelectAsync()
            {
                await using var sqlCommand = new SqlCommand("GetProducts", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("offset", offset));
                sqlCommand.Parameters.Add(new SqlParameter("limit", limit));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    yield return CreateProductTransferObject(reader);
                }
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<ProductTransferObject> SelectProductsByNameAsync(IEnumerable<string> productNames)
        {
            productNames = productNames ?? throw new ArgumentNullException(nameof(productNames));

            return SelectAsync();

            async IAsyncEnumerable<ProductTransferObject> SelectAsync()
            {
                await using var sqlCommand = new SqlCommand("GetProductsByName", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                foreach (var name in productNames)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.Add(new SqlParameter("name", name));

                    await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        yield return CreateProductTransferObject(reader);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<ProductTransferObject> SelectProductByCategoryAsync(IEnumerable<int> collectionOfCategoryId)
        {
            collectionOfCategoryId = collectionOfCategoryId ?? throw new ArgumentNullException(nameof(collectionOfCategoryId));

            return SelectAsync();

            async IAsyncEnumerable<ProductTransferObject> SelectAsync()
            {
                await using var sqlCommand = new SqlCommand("GetProductsByCategory", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                foreach (var id in collectionOfCategoryId)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.Add(new SqlParameter("categoryId", id));

                    await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        yield return CreateProductTransferObject(reader);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync()
        {
            await using var sqlCommand = new SqlCommand("GetProductsCount", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            if (this.connection.State != ConnectionState.Open)
            {
                await this.connection.OpenAsync();
            }

            return Convert.ToInt32(await sqlCommand.ExecuteScalarAsync(), CultureInfo.InvariantCulture);
        }

        private static SqlParameter[] GetProductParameters(ProductTransferObject product) =>
            new[]
            {
                        new SqlParameter("productName", product.Name),
                        new SqlParameter("supplierId", product.SupplierId),
                        new SqlParameter("categoryId", product.CategoryId),
                        new SqlParameter("quantityPerUnit", product.QuantityPerUnit),
                        new SqlParameter("unitPrice", product.UnitPrice),
                        new SqlParameter("unitsInStock", product.UnitsInStock),
                        new SqlParameter("unitsOnOrder", product.UnitsOnOrder),
                        new SqlParameter("reorderLevel", product.ReorderLevel),
                        new SqlParameter("discontinued", product.Discontinued),
            };

        private static ProductTransferObject CreateProductTransferObject(SqlDataReader reader) =>
            new ProductTransferObject
            {
                Id = (int)reader["ProductID"],
                Name = (string)reader["ProductName"],
                SupplierId = reader["SupplierID"] == DBNull.Value ? null : (int?)reader["SupplierID"],
                CategoryId = reader["CategoryID"] == DBNull.Value ? null : (int?)reader["CategoryID"],
                QuantityPerUnit = reader["QuantityPerUnit"] == DBNull.Value ? null : (string)reader["QuantityPerUnit"],
                UnitPrice = reader["UnitPrice"] == DBNull.Value ? null : (decimal?)reader["UnitPrice"],
                UnitsInStock = reader["UnitsInStock"] == DBNull.Value ? null : (short?)reader["UnitsInStock"],
                UnitsOnOrder = reader["UnitsOnOrder"] == DBNull.Value ? null : (short?)reader["UnitsOnOrder"],
                ReorderLevel = reader["ReorderLevel"] == DBNull.Value ? null : (short?)reader["ReorderLevel"],
                Discontinued = (bool)reader["Discontinued"],
            };
    }
}
