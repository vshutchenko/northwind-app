using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Northwind.DataAccess.Customers;

#pragma warning disable CA2000
namespace Northwind.DataAccess.SqlServer.Customers
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind customers.
    /// </summary>
    public sealed class CustomerSqlServerDataAccessObject : ICustomerDataAccessObject
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerSqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        public CustomerSqlServerDataAccessObject(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc/>
        public Task<string> InsertCustomerAsync(CustomerTransferObject customer)
        {
            customer = customer ?? throw new ArgumentNullException(nameof(customer));

            return InsertAsync();

            async Task<string> InsertAsync()
            {
                await using var sqlCommand = new SqlCommand("InsertCustomer", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.AddRange(GetCustomerParameters(customer));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                string insertedId = (await sqlCommand.ExecuteScalarAsync()).ToString();

                return insertedId;
            }
        }

        /// <inheritdoc/>
        public Task<bool> DeleteCustomerAsync(string customerId)
        {
            customerId = customerId ?? throw new ArgumentNullException(nameof(customerId));

            return DeleteAsync();

            async Task<bool> DeleteAsync()
            {
                await using var sqlCommand = new SqlCommand("DeleteCustomerById", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", customerId));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int deletedCount = await sqlCommand.ExecuteNonQueryAsync();

                return deletedCount > 0;
            }
        }

        /// <inheritdoc/>
        public Task<bool> UpdateCustomerAsync(CustomerTransferObject customer)
        {
            customer = customer ?? throw new ArgumentNullException(nameof(customer));

            return UpdateAsync();

            async Task<bool> UpdateAsync()
            {
                await using var sqlCommand = new SqlCommand("UpdateCustomer", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", customer.Id));
                sqlCommand.Parameters.AddRange(GetCustomerParameters(customer));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int updatedCount = await sqlCommand.ExecuteNonQueryAsync();

                return updatedCount > 0;
            }
        }

        /// <inheritdoc/>
        public Task<CustomerTransferObject> FindCustomerAsync(string customerId)
        {
            if (customerId is null)
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            return FindAsync();

            async Task<CustomerTransferObject> FindAsync()
            {
                await using var sqlCommand = new SqlCommand("GetCustomerById", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", customerId));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return CreateCustomerTransferObject(reader);
                }
                else
                {
                    throw new CustomerNotFoundException(customerId);
                }
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<CustomerTransferObject> SelectCustomersAsync(int offset, int limit)
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

            async IAsyncEnumerable<CustomerTransferObject> SelectAsync()
            {
                await using var sqlCommand = new SqlCommand("GetCustomers", this.connection)
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
                    yield return CreateCustomerTransferObject(reader);
                }
            }
        }

        private static SqlParameter[] GetCustomerParameters(CustomerTransferObject customer) =>
            new[]
            {
                new SqlParameter("companyName", customer.CompanyName),
                new SqlParameter("contactName", customer.ContactName),
                new SqlParameter("contactTitle", customer.ContactTitle),
                new SqlParameter("address", customer.Address),
                new SqlParameter("city", customer.City),
                new SqlParameter("region", customer.Region),
                new SqlParameter("postalCode", customer.PostalCode),
                new SqlParameter("country", customer.Country),
                new SqlParameter("phone", customer.Phone),
                new SqlParameter("fax", customer.Fax),
            };

        private static CustomerTransferObject CreateCustomerTransferObject(SqlDataReader reader) =>
            new CustomerTransferObject
            {
                Id = (string)reader["CustomerID"],
                CompanyName = (string)reader["CompanyName"],

                ContactName = reader["ContactName"] == DBNull.Value ? null : (string)reader["ContactName"],
                ContactTitle = reader["ContactTitle"] == DBNull.Value ? null : (string)reader["ContactTitle"],

                Address = reader["Address"] == DBNull.Value ? null : (string)reader["Address"],
                City = reader["City"] == DBNull.Value ? null : (string)reader["City"],
                Region = reader["Region"] == DBNull.Value ? null : (string)reader["Region"],

                PostalCode = reader["PostalCode"] == DBNull.Value ? null : (string)reader["PostalCode"],
                Country = reader["Country"] == DBNull.Value ? null : (string)reader["Country"],
                Phone = reader["Phone"] == DBNull.Value ? null : (string)reader["Phone"],
                Fax = reader["Fax"] == DBNull.Value ? null : (string)reader["Fax"],
            };
    }
}
