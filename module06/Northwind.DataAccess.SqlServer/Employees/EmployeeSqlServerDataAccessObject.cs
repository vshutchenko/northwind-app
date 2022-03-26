using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Northwind.DataAccess.Employees;

#pragma warning disable CA2000
namespace Northwind.DataAccess.SqlServer.Employees
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind employees.
    /// </summary>
    public sealed class EmployeeSqlServerDataAccessObject : IEmployeeDataAccessObject
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        public EmployeeSqlServerDataAccessObject(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc/>
        public Task<int> InsertEmployeeAsync(EmployeeTransferObject employee)
        {
            employee = employee ?? throw new ArgumentNullException(nameof(employee));

            return InsertAsync();

            async Task<int> InsertAsync()
            {
                await using var sqlCommand = new SqlCommand("InsertEmployee", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.AddRange(GetEmployeeParameters(employee));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int insertedCount = await sqlCommand.ExecuteNonQueryAsync();

                return insertedCount;
            }
        }

        /// <inheritdoc/>
        public Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(employeeId));
            }

            return DeleteAsync();

            async Task<bool> DeleteAsync()
            {
                await using var sqlCommand = new SqlCommand("DeleteEmployeeById", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", employeeId));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int deletedCount = await sqlCommand.ExecuteNonQueryAsync();

                return deletedCount > 0;
            }
        }

        /// <inheritdoc/>
        public Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee)
        {
            employee = employee ?? throw new ArgumentNullException(nameof(employee));

            return UpdateAsync();

            async Task<bool> UpdateAsync()
            {
                await using var sqlCommand = new SqlCommand("UpdateEmployee", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", employee.Id));
                sqlCommand.Parameters.AddRange(GetEmployeeParameters(employee));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                int updatedCount = await sqlCommand.ExecuteNonQueryAsync();

                return updatedCount > 0;
            }
        }

        /// <inheritdoc/>
        public Task<EmployeeTransferObject> FindEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(employeeId));
            }

            return FindAsync();

            async Task<EmployeeTransferObject> FindAsync()
            {
                await using var sqlCommand = new SqlCommand("GetEmployeeById", this.connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                sqlCommand.Parameters.Add(new SqlParameter("id", employeeId));

                if (this.connection.State != ConnectionState.Open)
                {
                    await this.connection.OpenAsync();
                }

                await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return CreateEmployeeTransferObject(reader);
                }
                else
                {
                    throw new EmployeeNotFoundException(employeeId);
                }
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<EmployeeTransferObject> SelectEmployeesAsync(int offset, int limit)
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

            async IAsyncEnumerable<EmployeeTransferObject> SelectAsync()
            {
                await using var sqlCommand = new SqlCommand("GetEmployees", this.connection)
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
                    yield return CreateEmployeeTransferObject(reader);
                }
            }
        }

        private static SqlParameter[] GetEmployeeParameters(EmployeeTransferObject employee) =>
            new[]
            {
                new SqlParameter("address", employee.Address),
                new SqlParameter("city", employee.City),
                new SqlParameter("region", employee.Region),
                new SqlParameter("birthDate", employee.BirthDate),
                new SqlParameter("country", employee.Country),
                new SqlParameter("extension", employee.Extension),
                new SqlParameter("firstName", employee.FirstName),
                new SqlParameter("lastName", employee.LastName),
                new SqlParameter("hireDate", employee.HireDate),
                new SqlParameter("postalCode", employee.PostalCode),
                new SqlParameter("homePhone", employee.HomePhone),
                new SqlParameter("titleOfCourtesy", employee.TitleOfCourtesy),
                new SqlParameter("title", employee.Title),
                new SqlParameter("photoPath", employee.PhotoPath),
                new SqlParameter("reportsTo", employee.ReportsTo),
                new SqlParameter("notes", employee.Notes),
                new SqlParameter("image", employee.Photo),
            };

        private static EmployeeTransferObject CreateEmployeeTransferObject(SqlDataReader reader) =>
            new EmployeeTransferObject
            {
                Id = (int)reader["EmployeeID"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],

                Address = reader["Address"] == DBNull.Value ? null : (string)reader["Address"],
                City = reader["City"] == DBNull.Value ? null : (string)reader["City"],
                Region = reader["Region"] == DBNull.Value ? null : (string)reader["Region"],
                BirthDate = reader["BirthDate"] == DBNull.Value ? null : (DateTime?)reader["BirthDate"],
                Country = reader["Country"] == DBNull.Value ? null : (string)reader["Country"],
                Extension = reader["Extension"] == DBNull.Value ? null : (string)reader["Extension"],

                HireDate = reader["HireDate"] == DBNull.Value ? null : (DateTime?)reader["HireDate"],
                PostalCode = reader["PostalCode"] == DBNull.Value ? null : (string)reader["PostalCode"],
                Photo = reader["Photo"] == DBNull.Value ? null : (byte[])reader["Photo"],
                HomePhone = reader["HomePhone"] == DBNull.Value ? null : (string)reader["HomePhone"],
                TitleOfCourtesy = reader["TitleOfCourtesy"] == DBNull.Value ? null : (string)reader["TitleOfCourtesy"],
                Title = reader["Title"] == DBNull.Value ? null : (string)reader["Title"],

                PhotoPath = reader["PhotoPath"] == DBNull.Value ? null : (string)reader["PhotoPath"],
                ReportsTo = reader["ReportsTo"] == DBNull.Value ? null : (int?)reader["ReportsTo"],
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
            };
    }
}
