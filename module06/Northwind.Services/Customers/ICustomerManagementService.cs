using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Customers
{
    /// <summary>
    /// Represents a management service for customers.
    /// </summary>
    public interface ICustomerManagementService
    {
        /// <summary>
        /// Gets a collection of customers using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="Customer"/>.</returns>
        IAsyncEnumerable<Customer> GetCustomersAsync(int offset, int limit);

        /// <summary>
        /// Gets a customer with specified identifier.
        /// </summary>
        /// <param name="customerId">A customer identifier.</param>
        /// <returns>Returns emloyee or null if customer does not exist.</returns>
        Task<Customer> GetCustomerAsync(string customerId);

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customer">An <see cref="Customer"/> to create.</param>
        /// <returns>Id of created customer.</returns>
        Task<string> CreateCustomerAsync(Customer customer);

        /// <summary>
        /// Destroys an existed customer.
        /// </summary>
        /// <param name="customerId">A customer identifier.</param>
        /// <returns>True if a customer is destroyed; otherwise false.</returns>
        Task<bool> DestroyCustomerAsync(string customerId);

        /// <summary>
        /// Updates a customer.
        /// </summary>
        /// <param name="customerId">A customer identifier.</param>
        /// <param name="customer">A <see cref="Customer"/>.</param>
        /// <returns>True if an employee is updated; otherwise false.</returns>
        Task<bool> UpdateCustomerAsync(string customerId, Customer customer);

        /// <summary>
        /// Gets customers count.
        /// </summary>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        public Task<int> CountAsync();
    }
}
