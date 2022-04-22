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
    /// Represents a SQL Server-tailored DAO for Northwind product categories.
    /// </summary>
    public sealed class ProductCategorySqlServerDataAccessObject : IProductCategoryDataAccessObject
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategorySqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        public ProductCategorySqlServerDataAccessObject(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc/>
        public Task<int> InsertProductCategoryAsync(ProductCategoryTransferObject productCategory)
        {
            productCategory = productCategory ?? throw new ArgumentNullException(nameof(productCategory));

            return InsertAsync();

            async Task<int> InsertAsync()
            {
                await using var sqlCommand = new SqlCommand("InsertCategory", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.AddRange(GetCategoryParameters(productCategory));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int id = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync(), CultureInfo.InvariantCulture);

                return id;
            }
        }

        /// <inheritdoc/>
        public Task<bool> DeleteProductCategoryAsync(int productCategoryId)
        {
            if (productCategoryId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(productCategoryId));
            }

            return DeleteAsync();

            async Task<bool> DeleteAsync()
            {
                await using var sqlCommand = new SqlCommand("DeleteCategoryById", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", productCategoryId));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int deletedCount = await sqlCommand.ExecuteNonQueryAsync();

                return deletedCount > 0;
            }
        }

        /// <inheritdoc/>
        public Task<bool> UpdateProductCategoryAsync(ProductCategoryTransferObject productCategory)
        {
            productCategory = productCategory ?? throw new ArgumentNullException(nameof(productCategory));

            return UpdateAsync();

            async Task<bool> UpdateAsync()
            {
                await using var sqlCommand = new SqlCommand("UpdateCategory", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", productCategory.Id));
                sqlCommand.Parameters.AddRange(GetCategoryParameters(productCategory));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int updatedCount = await sqlCommand.ExecuteNonQueryAsync();

                return updatedCount > 0;
            }
        }

        /// <inheritdoc/>
        public Task<ProductCategoryTransferObject> FindProductCategoryAsync(int productCategoryId)
        {
            if (productCategoryId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(productCategoryId));
            }

            return FindAsync();

            async Task<ProductCategoryTransferObject> FindAsync()
            {
                await using var sqlCommand = new SqlCommand("GetCategoryById", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", productCategoryId));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return CreateCategoryTransferObject(reader);
                }
                else
                {
                    throw new ProductCategoryNotFoundException(productCategoryId);
                }
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<ProductCategoryTransferObject> SelectProductCategoriesAsync(int offset, int limit)
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

            async IAsyncEnumerable<ProductCategoryTransferObject> SelectAsync()
            {
                await using var sqlCommand = new SqlCommand("GetCategories", this.connection)
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
                    yield return CreateCategoryTransferObject(reader);
                }
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<ProductCategoryTransferObject> SelectProductCategoriesByNameAsync(IEnumerable<string> productCategoryNames)
        {
            productCategoryNames = productCategoryNames ?? throw new ArgumentNullException(nameof(productCategoryNames));

            return SelectAsync();

            async IAsyncEnumerable<ProductCategoryTransferObject> SelectAsync()
            {
                await using var sqlCommand = new SqlCommand("GetCategoriesByName", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                foreach (var name in productCategoryNames)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.Add(new SqlParameter("name", name));

                    await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        yield return CreateCategoryTransferObject(reader);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync()
        {
            await using var sqlCommand = new SqlCommand("GetCategoriesCount", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            if (this.connection.State != ConnectionState.Open)
            {
                await this.connection.OpenAsync();
            }

            return Convert.ToInt32(await sqlCommand.ExecuteScalarAsync(), CultureInfo.InvariantCulture);
        }

        private static SqlParameter[] GetCategoryParameters(ProductCategoryTransferObject category) =>
            new[]
            {
                new SqlParameter("categoryName", category.Name),
                new SqlParameter("description", category.Description),
                new SqlParameter("picture", category.Picture),
            };

        private static ProductCategoryTransferObject CreateCategoryTransferObject(SqlDataReader reader) =>
            new ProductCategoryTransferObject
            {
                Id = (int)reader["CategoryID"],
                Name = (string)reader["CategoryName"],
                Description = reader["Description"] == DBNull.Value ? null : (string)reader["Description"],
                Picture = reader["Picture"] == DBNull.Value ? null : (byte[])reader["Picture"],
            };
    }
}
