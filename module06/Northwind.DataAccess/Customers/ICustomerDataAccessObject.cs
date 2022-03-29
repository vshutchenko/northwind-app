using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Customers
{
    /// <summary>
    /// Represents a DAO for Northwind customers.
    /// </summary>
    public interface ICustomerDataAccessObject
    {
        /// <summary>
        /// Inserts a new Northwind customer to a data storage.
        /// </summary>
        /// <param name="customer">An <see cref="CustomerTransferObject"/>.</param>
        /// <returns>A data storage identifier of a new customer.</returns>
        Task<string> InsertCustomerAsync(CustomerTransferObject customer);

        /// <summary>
        /// Deletes a Northwind customer from a data storage.
        /// </summary>
        /// <param name="customerId">A customer identifier.</param>
        /// <returns>True if a customer is deleted; otherwise false.</returns>
        Task<bool> DeleteCustomerAsync(string customerId);

        /// <summary>
        /// Updates a Northwind customer in a data storage.
        /// </summary>
        /// <param name="customer">An <see cref="CustomerTransferObject"/>.</param>
        /// <returns>True if an customer is updated; otherwise false.</returns>
        Task<bool> UpdateCustomerAsync(CustomerTransferObject customer);

        /// <summary>
        /// Finds a Northwind customer using a specified identifier.
        /// </summary>
        /// <param name="customerId">A data storage identifier of an existed customer.</param>
        /// <returns>An <see cref="CustomerTransferObject"/> with specified identifier.</returns>
        Task<CustomerTransferObject> FindCustomerAsync(string customerId);

        /// <summary>
        /// Selects customers using specified offset and limit.
        /// </summary>
        /// <param name="offset">An offset of the first object.</param>
        /// <param name="limit">A limit of returned objects.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="CustomerTransferObject"/>.</returns>
        IAsyncEnumerable<CustomerTransferObject> SelectCustomersAsync(int offset, int limit);
    }
}
